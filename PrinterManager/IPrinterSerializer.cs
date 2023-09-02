using PrinterManager.Responses;

namespace PrinterManager;

/// <summary>
/// Serializes a <see cref="IPrinterRequest"/> to a series of bytes.
/// </summary>
public interface IPrinterSerializer
{
    /// <summary>
    /// Serializes the command into a printer data stream.
    /// </summary>
    /// <param name="command">The command object. Must be <see cref="IPrinterRequest"/>.</param>
    /// <returns>The data stream content.</returns>
    Span<byte> Serialize(object command);

    /// <summary>
    /// Attempts to deserialize the printer's response into the first matching template.
    /// </summary>
    /// <param name="response">The response of the printer.</param>
    /// <returns>The result.</returns>
    SerializeResult TryDeserialize(Span<byte> response);
}
