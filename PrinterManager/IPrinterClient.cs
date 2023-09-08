using PrinterManager.Requests;
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
    IObservable<IPrinterResponse> OnResponse { get; }

    /// <summary>
    /// An observable that emits the received <b>raw</b> messages from the printer.
    /// </summary>
    IObservable<byte[]> OnResponseData { get; }

    /// <summary>
    /// An obsevable that emits the received messages that could not be parsed.
    /// </summary>
    IObservable<byte[]> UnknownResponse { get; }

    /// <summary>
    /// The communicator used by the client.
    /// </summary>
    ICommunicator Communicator { get; }

    /// <summary>
    /// Sends a command to the printer and does <b>not</b> wait for a response.
    /// </summary>
    /// <param name="request"></param>
    /// <remarks>No exception will be raised regardless of the printer's response.</remarks>
    void Send(IPrinterRequest request);

    /// <summary>
    /// Sends a command to the printer and waits for an "ok" response.
    /// </summary>
    void SendOk(IPrinterRequest request);

    /// <summary>
    /// Sends a command to the printer and waits for a specific response.
    /// </summary>
    /// <typeparam name="TResponse">The type of response to wait for.</typeparam>
    /// <param name="command">The command to send.</param>
    TResponse SendWaitResponse<TResponse>(IPrinterRequest request)
        where TResponse : IPrinterResponse, new();
}
