using PrinterManager.GCodeTemplates;
using PrinterManager.Responses;
using PrinterManager.Serialization;
using System.Text;

namespace PrinterManager.CommandSerializers;

/// <summary>
/// A <see cref="IPrinterSerializer"/> that converts commands to GCodes using the Marlin 1xx template.
/// </summary>
public class Marlin1xxGCodeSerializer : IPrinterSerializer
{
    public T Deserialize<T>(Span<byte> response)
        where T : IPrinterResponse, new()
    {
        var template = Marlin1xxTemplate.ResponseTemplates.First(t => t.TargetType == typeof(T));
        var ret = GCodeSerializer.TryDeserialize<T>(Encoding.ASCII.GetString(response), template, out var result);

        if (ret)
        {
            return result;
        }
        else
        {
            throw new Exception("Could not deserialize response");
        }
    }

    public Span<byte> Serialize(object command)
    {
        var template = Marlin1xxTemplate.CommandTemplate.First(t => t.RequestType == command.GetType());

        var gcode = GCodeSerializer.Serialize(command, template);
        return Encoding.ASCII.GetBytes(gcode);
    }
}
