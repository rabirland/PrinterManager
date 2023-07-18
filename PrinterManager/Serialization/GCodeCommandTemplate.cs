namespace PrinterManager.Serialization;

/// <summary>
/// A template for a GCode command.
/// </summary>
/// <param name="RequestType">The C# model type that represents the command.</param>
/// <param name="GCode">The GCode.</param>
/// <param name="Parameters">Each parameter binding between the GCode and C# model.</param>
public readonly record struct GCodeCommandTemplate(Type RequestType, string GCode, GCodeCommandParameter[] Parameters);