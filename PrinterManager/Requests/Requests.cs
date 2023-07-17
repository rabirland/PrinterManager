namespace PrinterManager.Requests;

public readonly record struct GetTemperatures(bool IncludeRedundantSensor = false, int HotendIndex = 0) : IPrinterRequest;
public readonly record struct AutoReportTemperatures(int Seconds) : IPrinterRequest;