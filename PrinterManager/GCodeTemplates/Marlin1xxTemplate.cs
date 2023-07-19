using PrinterManager.Requests;
using PrinterManager.Responses;
using PrinterManager.Serialization;

namespace PrinterManager.GCodeTemplates;

public static class Marlin1xxTemplate
{
    public readonly static GCodeCommandTemplate[] CommandTemplate = new GCodeTemplateBuilder()
        .AddCommand<SetAxisStepperStepsPerMm>("M92")
            .WithParameter(c => c.AxisX, "X")
            .WithParameter(c => c.AxisY, "Y")
            .WithParameter(c => c.AxisZ, "Z")
            .WithParameter(c => c.AxisE, "E")
            .WithParameter(c => c.TargetExtruder, "T")

        .AddCommand<Temperature>("M105")
            .WithParameter(c => c.IncludeRedundantSensor, "R")
            .WithParameter(c => c.HotendIndex, "T")

        .AddCommand<GetCurrentPosition>("M114")
            .WithParameter(c => c.DetailedInformation, "D")
            .WithParameter(c => c.ReportEStepperPosition, "E")
            .WithParameter(c => c.RealPositionInformation, "R")

        .AddCommand<AutoReportTemperatures>("M154")
            .WithParameter(c => c.Seconds, "S")

        .AddCommand<AutoReportTemperatures>("M155")
            .WithParameter(c => c.Seconds, "S")

        .AddCommand<BedLevelingState>("M420")
            .WithParameter(c => c.CenterMeshOnMean, "C")
            .WithParameter(c => c.LoadMeshIndex, "L")
            .WithParameter(c => c.IsEnabled, "S")
            .WithParameter(c => c.PrintFormat, "T")
            .WithParameter(c => c.Verbose, "V")
            .WithParameter(c => c.ZFadeHeight, "Z")

        .AddCommand<SaveConfiguration>("M500")

        .AddCommand<LoadConfiguration>("M501")

        .AddCommand<FactoryResetConfiguration>("M502")

        .Build();

    public readonly static GCodeResponseTemplate[] ResponseTemplates = new[]
    {
        GCodeResponseTemplate.Create<TemperatureStateResponse>("T:(?<HotendCurrent>\\d+(.\\d*)?)\\s*/(?<HotendTarget>\\d+(.\\d*)?)\\s*B:(?<BedCurrent>\\d+(.\\d*)?)\\s*/(?<BedTarget>\\d+(.\\d*)?)"),
    };
}
