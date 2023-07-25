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
    /// Fired when the state of the connection changes.
    /// </summary>
    public event Action? OnConnectionChanged;

    /// <summary>
    /// Fired when a connection is established.
    /// </summary>
    event Action? OnConnected;

    /// <summary>
    /// Sends a command to the printer.
    /// </summary>
    /// <param name="command">The command to send.</param>
    void Send(string command);
}
