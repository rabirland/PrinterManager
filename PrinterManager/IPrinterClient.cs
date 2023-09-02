using PrinterManager.Responses;
using System.Reactive.Subjects;

namespace PrinterManager;

/// <summary>
/// Communicates with the printer, expecting a response.
/// </summary>
public interface IPrinterClient
{
    /// <summary>
    /// An observable that emits the received messages from the printer.
    /// </summary>
    public IObservable<IPrinterResponse> OnResponse { get; }

    /// <summary>
    /// An obsevable that emits the received messages that could not be parsed.
    /// </summary>
    public IObservable<byte[]> UnknownResponse { get; }

    /// <summary>
    /// The communicator used by the client.
    /// </summary>
    public ICommunicator Communicator { get; }

    /// <summary>
    /// Sends a command to the printer and does <b>not</b> wait for a response.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>No exception will be raised regardless of the printer's response.</remarks>
    void Send(object request);

    /// <summary>
    /// Sends a command to the printer and waits for an "ok" response.
    /// </summary>
    void SendOk(object request);

    /// <summary>
    /// Sends a command to the printer and waits for a specific response.
    /// </summary>
    /// <typeparam name="TResponse">The type of response to wait for.</typeparam>
    /// <param name="command">The command to send.</param>
    TResponse SendWaitResponse<TResponse>(object request)
        where TResponse : IPrinterResponse, new();
}
