namespace PrinterManager;

/// <summary>
/// Communicates with the printer, expecting a response.
/// </summary>
public interface IPrinterClient
{
    /// <summary>
    /// Sends a command to the printer an expects a specific response.
    /// </summary>
    /// <param name="command">The command to send.</param>
    TResponse Send<TResponse>(object request);
}
