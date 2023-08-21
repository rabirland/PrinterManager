using PrinterManager.Responses;

namespace PrinterManager;

/// <summary>
/// Communicates with the printer, expecting a response.
/// </summary>
public interface IPrinterClient
{
    /// <summary>
    /// Sends a command to the printer and does <b>not</b> wait for a response.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>No exception will be raised regardless of the printer's response.</remarks>
    void Send(object request);

    /// <summary>
    /// Sends a command to the printer and waits for an "ok" response.
    /// </summary>
    void BufferCommand(object request);

    /// <summary>
    /// Sends a command to the printer and waits for a specific response.
    /// </summary>
    /// <typeparam name="TResponse">The type of response to wait for.</typeparam>
    /// <param name="command">The command to send.</param>
    TResponse SendWaitResponse<TResponse>(object request)
        where TResponse : IPrinterResponse, new();
}
