namespace PrinterManager.Poco;
public readonly record struct TemperatureReport(float HotendCurrent, float HotendTarget, float BedCurrent, float BedTarget) : IPrinterResponse;