namespace PrinterManager.Serialization;

/// <summary>
/// A single parameter for the GCode template.
/// </summary>
/// <param name="Name">The name of the field in the C# model.</param>
public readonly record struct GCodeCommandParameter(string Name, string Prefix);