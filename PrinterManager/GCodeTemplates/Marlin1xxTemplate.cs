using PrinterManager.Requests;
using PrinterManager.Serializer;

namespace PrinterManager.GCodeTemplates;

public static class Marlin1xxTemplate
{
    public readonly static GCodeCommandTemplate[] Template = new GCodeTemplateBuilder()
        .AddType<GetTemperatures>("M105")
            .WithParameter(c => c.IncludeRedundantSensor, "R").Flag()
            .WithParameter(c => c.HotendIndex, "T")

        .AddType<AutoReportTemperatures>("M155")
            .WithParameter(c => c.Seconds, "S").ForceInclude()
        .Build();
}
