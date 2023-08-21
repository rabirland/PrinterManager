using PrinterManager.Responses;
using System.Text;

namespace PrinterManager.Clients;

/// <summary>
/// A <see cref="IPrinterClient"/> that uses an <see cref="ICommunicator"/>.
/// </summary>
public class CommunicatorPrinterClient : IPrinterClient
{
    private readonly ICommunicator communicator;
    private readonly IPrinterSerializer commandSerializer;
    private readonly int timeout;

    public CommunicatorPrinterClient(
        ICommunicator communicator,
        IPrinterSerializer commandSerializer,
        int timeout = 10000)
    {
        this.communicator = communicator;
        this.commandSerializer = commandSerializer;
        this.timeout = timeout;
    }

    /// <inheritdoc/>.
    public TResponse SendWaitResponse<TResponse>(object request)
        where TResponse : IPrinterResponse, new()
    {
        var data = commandSerializer.Serialize(request);

        var listenTask = AwaitResponse<TResponse>();
        var timeoutTask = Task.Delay(timeout);

        communicator.Send(data);

        var resultIndex = Task.WaitAny(listenTask, timeoutTask);

        if (resultIndex == 1) // Index of timeout task
        {
            throw new Exception("Timeout");
        }
        else
        {
            return listenTask.Result;
        }
    }

    /// <summary>
    /// Runs a task that waits for a specific response.
    /// </summary>
    /// <typeparam name="T">The response type to expect.</typeparam>
    /// <returns></returns>
    private async Task<T> AwaitResponse<T>()
        where T : IPrinterResponse, new()
    {
        string buffer = string.Empty;
        Action<string> listener = (string msg) => buffer += msg;

        communicator.OnMessage += listener;
        try
        {
            bool success = false;
            IPrinterResponse ret;
            do
            {
                var data = Encoding.ASCII.GetBytes(buffer);
                success = commandSerializer.TryDeserialize(data, out ret);
                await Task.Delay(10);
            } while (success == false);

            if (ret.GetType() != typeof(T))
            {
                throw new Exception("Unexpected response");
            }

            return (T)ret;
           
        }
        finally
        {
            communicator.OnMessage -= listener;
        }
    }
}
