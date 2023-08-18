namespace PrinterManager.Responses;

/// <summary>
/// An empty response, just an "ok" from the printer
/// </summary>
public readonly record struct OkResponse() : IPrinterResponse;

public readonly record struct TemperatureStateResponse(float HotendCurrent, float HotendTarget, float BedCurrent, float BedTarget) : IPrinterResponse;
public readonly record struct Settings(
    DistanceUnits unit,
    TemperatureUnits TemperatureUnit,
    float FilamentDiameter,
    EStepSettings ESteps,
    FeedratesSettings Feedrates,
    MaxAccelerationSettings MaxAcceleration,
    AccelerationSettings Acceleration,
    float MaxFeedrateForPrintMoves,
    float MinFeedrateForTravelMoves,
    float MinimumSegmentTime,
    float XAxisMaxJerk,
    float YAxisMaxJerk,
    float ZAxisMaxJerk,
    float EAxisMaxJerk,
    HomeOffsetSettings HomeOffset,
    float BedLevelingEnabled,
    float BedLevelingZFadeHeight);

public readonly record struct EStepSettings(
    float XAxisStepsPerMM,
    float YAxisStepsPerMM,
    float ZAxisStepsPerMm,
    float EAxisStepsPerMm);

public readonly record struct FeedratesSettings(
    float XAxisMaxFeedrate,
    float YAxisMaxFeedrate,
    float ZAxisMaxFeedrate,
    float EAxisMAxFeedrate);

public readonly record struct MaxAccelerationSettings(
    float XAxisMaxAcceleration,
    float YAxisMaxAcceleration,
    float ZAxisMaxAcceleration,
    float EAxisMaxAcceleration);

public readonly record struct AccelerationSettings(
    float PrintingStartAcceleration,
    float RetractStartAcceleration,
    float TravelStartAcceleration);

public readonly record struct HomeOffsetSettings(
    float XAxisHomeOffset,
    float YAxisHomeOffset,
    float ZAxisHomeOffset);

public enum DistanceUnits
{
    Millimeter,
    Inch,
}

public enum TemperatureUnits
{
    Celsius,
    Kelvin,
    Fahrenheit,
}