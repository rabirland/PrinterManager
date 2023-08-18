﻿namespace PrinterManager.Requests;

public readonly record struct GetTemperature(bool? IncludeRedundantSensor, int? HotendIndex) : IPrinterRequest;
public readonly record struct SetHotendTemperature(float Temperature, int? HotendIndex = null, float? MaxAutoTemperature = null, bool EnableAutoTemp = false) : IPrinterRequest;
public readonly record struct AutoReportTemperatures(int Seconds) : IPrinterRequest;
public readonly record struct BedLevelingState(bool? CenterMeshOnMean, int? LoadMeshIndex, bool? IsEnabled, int? PrintFormat, bool? Verbose, float? ZFadeHeight) : IPrinterRequest;
public readonly record struct GetCurrentPosition(bool? DetailedInformation, bool? ReportEStepperPosition, bool? RealPositionInformation) : IPrinterRequest;
public readonly record struct AutoReportPositions(int Seconds) : IPrinterRequest;
public readonly record struct SetAxisStepperStepsPerMm(float? AxisX, float? AxisY, float? AxisZ, float? AxisE, int? TargetExtruder) : IPrinterRequest;
public readonly record struct SaveConfiguration() : IPrinterRequest;
public readonly record struct LoadConfiguration() : IPrinterRequest;
public readonly record struct FactoryResetConfiguration() : IPrinterRequest;
public readonly record struct LinearMovement(float? TargetExtruderPosition = null, float? FeedRate = null, float? LaserPower = null, float? TargetXPos = null, float? TargetYPos = null, float? TargetZPos = null) : IPrinterRequest;
public readonly record struct RapidLinearMovement(float? TargetExtruderPosition, float? FeedRate, float? LaserPower, float? TargetXPos, float? TargetYPos, float? TargetZPos) : IPrinterRequest;
public readonly record struct SetUnitsToInches() : IPrinterRequest;
public readonly record struct SetExtruderAbsoluteMode() : IPrinterRequest;
public readonly record struct SetRelativeMode() : IPrinterRequest;
public readonly record struct SetAbsoluteMode() : IPrinterRequest;
public readonly record struct AutoHome(bool? RestoreLevelingStateAfter, bool? SkipTrusedAxes, float? RaiseNozzleDistanceBefore, bool? HomeXAxis, bool? HomeYAxis, bool? HomeZAxis) : IPrinterRequest;
public readonly record struct SetPosition(float? X = null, float? Y = null, float? Z = null, float? E = null) : IPrinterRequest;