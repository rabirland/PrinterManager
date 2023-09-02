using PrinterManager.Responses;
using PrinterManager.Serialization;
using System.Text;

namespace PrinterManager.CommandSerializers;

/// <summary>
/// A <see cref="IPrinterSerializer"/> that converts commands to GCodes.
/// </summary>
public class GCodePrinterSerializer : IPrinterSerializer
{
    private readonly GCodeCommandTemplate[] commandTemplates;
    private readonly GCodeResponseTemplate[] responseTemplates;

    public GCodePrinterSerializer(GCodeCommandTemplate[] commandTemplates, GCodeResponseTemplate[] responseTemplates)
    {
        this.commandTemplates = commandTemplates;
        this.responseTemplates = responseTemplates;
    }

    public SerializeResult TryDeserialize(Span<byte> response)
    {
        var endOfLine = response.IndexOfAny((byte)'\n', (byte)'\r');

        if (endOfLine == -1)
        {
            return SerializeResult.NoChunk;
        }

        var chunkData = response[0..endOfLine];
        var responseText = Encoding.ASCII.GetString(chunkData);

        foreach (var template in responseTemplates)
        {
            var success = GCodeSerializer.TryDeserialize(responseText, template, out var result);

            if (success)
            {
                return new SerializeResult((IPrinterResponse)result, chunkData.Length, true);
            }
        }

        return SerializeResult.CreateUnknownChunk(chunkData.ToArray());
    }

    public Span<byte> Serialize(object command)
    {
        var template = commandTemplates.First(t => t.RequestType == command.GetType());

        var gcode = Serialization.GCodeSerializer.Serialize(command, template);
        return Encoding.ASCII.GetBytes(gcode);
    }
}
