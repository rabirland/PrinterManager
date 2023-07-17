using PrinterManager.Requests;
using PrinterManager.Serializer;

namespace PrinterManager.Templates;

public static class Marlin1xxTemplate
{
    public readonly static GCodeCommandTemplate[] Template = new[]
    {
        new GCodeCommandTemplate(typeof(GetTemperatures), "M105", new[]
        {
            new GCodeCommandParameter(nameof(GetTemperatures.IncludeRedundantSensor), "R", false, true),
            new GCodeCommandParameter(nameof(GetTemperatures.HotendIndex), "T", false, false),
        }),

        new GCodeCommandTemplate(typeof(AutoReportTemperatures), "M155", new[]
        {
            new GCodeCommandParameter(nameof(AutoReportTemperatures.Seconds), "S", true, false),
        }),
    };
}
