namespace PrinterManager;

/// <summary>
/// A communicator for a device.
/// </summary>
public interface ICommunicator
{
    /// <summary>
    /// Whether the channel is open.
    /// </summary>
    public bool IsOpen { get; }

    /// <summary>
    /// Fired when a printer sends a message.
    /// </summary>
    event Action<string>? OnMessage;

    /// <summary>
    /// Sends a command to the printer.
    /// </summary>
    /// <param name="command">The command to send.</param>
    void Send(string command);
}
