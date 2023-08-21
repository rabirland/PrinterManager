using PrinterManager.GCodeTemplates;
using PrinterManager.Responses;
using PrinterManager.Serialization;
using System.Text;

namespace PrinterManager.CommandSerializers;

/// <summary>
/// A <see cref="IPrinterSerializer"/> that converts commands to GCodes.
/// </summary>
public class GCodeSerializer : IPrinterSerializer
{
    private readonly GCodeCommandTemplate[] commandTemplates;
    private readonly GCodeResponseTemplate[] responseTemplates;

    public GCodeSerializer(GCodeCommandTemplate[] commandTemplates, GCodeResponseTemplate[] responseTemplates)
    {
        this.commandTemplates = commandTemplates;
        this.responseTemplates = responseTemplates;
    }

    public bool TryDeserialize(Span<byte> response, out IPrinterResponse obj)
    {
        foreach (var template in responseTemplates)
        {
            var success = Serialization.GCodeSerializer.TryDeserialize(Encoding.ASCII.GetString(response), template, out var result);

            if (success)
            {
                obj = (IPrinterResponse)result;
                return true;
            }
        }

        obj = default;
        return false;
    }

    public Span<byte> Serialize(object command)
    {
        var template = commandTemplates.First(t => t.RequestType == command.GetType());

        var gcode = Serialization.GCodeSerializer.Serialize(command, template);
        return Encoding.ASCII.GetBytes(gcode);
    }
}
