namespace PrinterManager.Clients;

/// <summary>
/// A <see cref="IPrinterClient"/> that uses an <see cref="ICommunicator"/>.
/// </summary>
public class CommunicatorClient : IPrinterClient
{
    private readonly ICommunicator communicator;
    private readonly IPrinterSerializer commandSerializer;

    public CommunicatorClient(ICommunicator communicator, IPrinterSerializer commandSerializer)
    {
        this.communicator = communicator;
        this.commandSerializer = commandSerializer;
    }

    public TResponse Send<TResponse>(object request)
    {
        var data = commandSerializer.Serialize(request);
        communicator.Send(data);
    }

    private async Task AwaitResponse()
    {

    }
}
