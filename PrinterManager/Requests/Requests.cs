namespace PrinterManager.Requests;

public readonly record struct Temperature(bool? IncludeRedundantSensor, int? HotendIndex) : IPrinterRequest;
public readonly record struct AutoReportTemperatures(int Seconds) : IPrinterRequest;
public readonly record struct BedLevelingState(bool? CenterMeshOnMean, int? LoadMeshIndex, bool? IsEnabled, int? PrintFormat, bool? Verbose, float? ZFadeHeight) : IPrinterRequest;
public readonly record struct GetCurrentPosition(bool? DetailedInformation, bool? ReportEStepperPosition, bool? RealPositionInformation) : IPrinterRequest;
public readonly record struct AutoReportPositions(int Seconds) : IPrinterRequest;
public readonly record struct SetAxisStepperStepsPerMm(float? AxisX, float? AxisY, float? AxisZ, float? AxisE, int? TargetExtruder) : IPrinterRequest;
public readonly record struct SaveConfiguration() : IPrinterRequest;
public readonly record struct LoadConfiguration() : IPrinterRequest;
public readonly record struct FactoryResetConfiguration() : IPrinterRequest;