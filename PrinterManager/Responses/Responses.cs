namespace PrinterManager.Responses;
public readonly record struct TemperatureStateResponse(float HotendCurrent, float HotendTarget, float BedCurrent, float BedTarget) : IPrinterResponse;