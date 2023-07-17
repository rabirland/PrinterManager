using PrinterManager.Poco;
using PrinterManager.Requests;

namespace PrinterManager;

/// <summary>
/// A printer specific manager.
/// </summary>
public interface IPrinterManager
{
    /// <summary>
    /// The communicator to talk with the printer.
    /// </summary>
    ICommunicator Communicator { get; }

    /// <summary>
    /// Fired when a message is received from the printer.
    /// </summary>
    event Action<IPrinterResponse>? Messages;

    /// <summary>
    /// Sends a request/command to the printer.
    /// </summary>
    /// <typeparam name="T">The type of request/command to send.</typeparam>
    /// <param name="command">The request/command.</param>
    void SendCommand<T>(T command)
        where T : IPrinterRequest;
}
