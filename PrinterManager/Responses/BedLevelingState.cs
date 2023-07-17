namespace PrinterManager.Poco;

/// <summary>
/// The bed leveling state.
/// </summary>
/// <param name="mesh">The height map from the ABL.</param>
public readonly record struct BedLevelingState(float[,] mesh) : IPrinterResponse;
