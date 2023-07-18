using PrinterManager.Requests;
using PrinterManager.Serialization;
using PrinterManager.GCodeTemplates;
using PrinterManager.Responses;

namespace PrinterManager.Managers;

/// <summary>
/// Printer manager for Marlin 1.x.x versions.
/// </summary>
public class Marlin1xxPrinterManager : IPrinterManager, IDisposable
{
    private readonly ICommunicator communicator;

    /// <inheritdoc />
    public ICommunicator Communicator => communicator;

    /// <inheritdoc />
    public event Action<IPrinterResponse>? OnMessage;

    public Marlin1xxPrinterManager(ICommunicator communicator)
    {
        this.communicator = communicator;
        this.communicator.OnMessage += OnMessageReceived;
    }

    ~Marlin1xxPrinterManager()
    {
        Dispose();
    }

    public void SendCommand<T>(T command) where T : IPrinterRequest
    {
        var gcode = GCodeSerializer.Serialize(command, Marlin1xxTemplate.CommandTemplate);
        communicator.Send(gcode);
    }

    public void Dispose()
    {
        this.communicator.OnMessage -= OnMessageReceived;
    }

    private void OnMessageReceived(string message)
    {
        var response = GCodeParser.Parse(message, Marlin1xxTemplate.ResponseTemplates);

        if (response != null)
        {
            OnMessage?.Invoke(response);
        }
    }
}
