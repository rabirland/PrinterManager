using PrinterManager.Poco;

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
    /// Ask the printer to report it's temperatures.
    /// </summary>
    void QueryTemperatures();

    /// <summary>
    /// Instruct the printer to report the temperature at every <paramref name="interval"/> seconds.
    /// </summary>
    /// <param name="interval">The seconds to wait between reports. Provide <c>0</c> to disable.</param>
    void MonitorTemperatures(int interval);
}
