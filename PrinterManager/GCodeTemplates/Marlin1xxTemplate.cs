using PrinterManager.Requests;
using PrinterManager.Serializer;

namespace PrinterManager.GCodeTemplates;

public static class Marlin1xxTemplate
{
    public readonly static GCodeCommandTemplate[] Template = new GCodeTemplateBuilder()
        .AddCommand<Temperature>("M105")
            .WithParameter(c => c.IncludeRedundantSensor, "R")
            .WithParameter(c => c.HotendIndex, "T")

        .AddCommand<AutoReportTemperatures>("M155")
            .WithParameter(c => c.Seconds, "S")

        .AddCommand<BedLevelingState>("M420")
            .WithParameter(c => c.CenterMeshOnMean, "C")
            .WithParameter(c => c.LoadMeshIndex, "L")
            .WithParameter(c => c.IsEnabled, "S")
            .WithParameter(c => c.PrintFormat, "T")
            .WithParameter(c => c.Verbose, "V")
            .WithParameter(c => c.ZFadeHeight, "Z")
        .Build();
}
