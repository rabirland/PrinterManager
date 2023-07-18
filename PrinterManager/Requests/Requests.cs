namespace PrinterManager.Requests;

public readonly record struct Temperature(bool? IncludeRedundantSensor, int? HotendIndex) : IPrinterRequest;
public readonly record struct AutoReportTemperatures(int Seconds) : IPrinterRequest;
public readonly record struct BedLevelingState(bool? CenterMeshOnMean, int? LoadMeshIndex, bool? IsEnabled, int? PrintFormat, bool? Verbose, float? ZFadeHeight) : IPrinterRequest;