using PrinterManager.Responses;

namespace PrinterManager;

/// <summary>
/// A result for <see cref="IPrinterSerializer"/>.
/// </summary>
/// <param name="Response">The serialized response, or <see cref="NullResponse"/> if could not deserialize.</param>
/// <param name="ByteCount">
/// The amount of bytes that makes up the next valid chunk in the data source.
/// <c>0</c> means the data source does not contain a full chunk.
/// A positive number means a chunk is available. If <see cref="Success"/> is <see langword="false"/>,
/// the <see cref="Response"/> should be <see cref="UnknownResponse"/> that contains the unrecognized chunk.
/// </param>
/// <param name="Success"><see langword="true"/> if a response could be serialized, otherwise <see langword="false"/>.</param>
public record SerializeResult(IPrinterResponse Response, int ByteCount, bool Success)
{
    /// <summary>
    /// A response that means no chunk is detected in the data source.
    /// </summary>
    public static SerializeResult NoChunk => new(new UnknownResponse(Array.Empty<byte>()), 0, false);

    /// <summary>
    /// Creates a response where a chunk is detected but not a message.
    /// </summary>
    /// <param name="data">The content of the chunk.</param>
    /// <returns>The constructed result.</returns>
    public static SerializeResult CreateUnknownChunk(byte[] data)
    {
        return new SerializeResult(new UnknownResponse(data), data.Length, false);
    }

    /// <summary>
    /// A container for a type of response that a chunk is recognized but not a message.
    /// </summary>
    /// <param name="data"></param>
    public record UnknownResponse(byte[] data) : IPrinterResponse;
}
