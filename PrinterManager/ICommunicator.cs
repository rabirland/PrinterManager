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
    void Send(Span<byte> command);

    /// <summary>
    /// Read all the available data from the communicator.
    /// </summary>
    /// <returns>The read data.</returns>
    byte[] Read();
}
