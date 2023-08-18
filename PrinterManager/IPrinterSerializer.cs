using PrinterManager.Responses;

namespace PrinterManager;

/// <summary>
/// Serializes a <see cref="IPrinterRequest"/> to a series of bytes.
/// </summary>
public interface IPrinterSerializer
{
    Span<byte> Serialize(object command);

    T Deserialize<T>(Span<byte> response)
        where T : IPrinterResponse, new();
}
