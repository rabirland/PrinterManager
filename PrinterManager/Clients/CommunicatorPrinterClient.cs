using PrinterManager.Responses;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrinterManager.Clients;

/// <summary>
/// A <see cref="IPrinterClient"/> that uses an <see cref="ICommunicator"/>.
/// </summary>
public class CommunicatorPrinterClient : IPrinterClient, IDisposable
{
    private readonly IPrinterSerializer commandSerializer;
    private readonly Subject<IPrinterResponse> parsedResponses = new();
    private readonly Subject<byte[]> unknownResponses = new();

    private bool isRunning = false;

    public CommunicatorPrinterClient(
        ICommunicator communicator,
        IPrinterSerializer commandSerializer)
    {
        this.Communicator = communicator;
        this.commandSerializer = commandSerializer;

        OnResponse = parsedResponses.AsObservable();
        UnknownResponse = unknownResponses.AsObservable();

        isRunning = true;
        Task.Factory.StartNew(BackgroundWorker, TaskCreationOptions.LongRunning);
    }

    public void Dispose()
    {
        isRunning = false;
    }

    /// <inheritdoc/>
    public ICommunicator Communicator { get; }

    public IObservable<IPrinterResponse> OnResponse { get; }

    public IObservable<byte[]> UnknownResponse { get; }

    /// <inheritdoc/>
    public void Send(object request)
    {
        var data = commandSerializer.Serialize(request);
        Communicator.Send(data);
    }

    /// <inheritdoc/>
    public void SendOk(object request)
    {
        SendWaitResponse<OkResponse>(request);
    }

    /// <inheritdoc/>
    public TResponse SendWaitResponse<TResponse>(object request)
        where TResponse : IPrinterResponse, new()
    {
        var listenTask = AwaitResponse<TResponse>();

        Send(request);

        return listenTask.Result;
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

        Communicator.OnInput += listener;
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
            Communicator.OnInput -= listener;
        }
    }

    private void BackgroundWorker()
    {
        List<byte> buffer = new List<byte>();

        while (isRunning)
        {
            var available = Communicator.Read();
            buffer.AddRange(available);
        }

        var serializeResult = commandSerializer.TryDeserialize(buffer.ToArray()); // TODO: Optimize to not need ToArray()

        buffer.RemoveRange(0, serializeResult.ByteCount);

        if (serializeResult.Success)
        {
            parsedResponses.OnNext(serializeResult.Response);
        }
        else if (serializeResult.Response is SerializeResult.UnknownResponse ur)
        {
            unknownResponses.OnNext(ur.data);
        }
        else
        {
            // TODO: Figure out what to do
        }
    }
}
