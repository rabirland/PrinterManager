namespace PrinterManager.Serializer;

/// <summary>
/// A single parameter for the GCode template.
/// </summary>
/// <param name="Name">The name of the field in the C# model.</param>
/// <param name="Prefix">The prefix of the parameter in the GCode.</param>
/// <param name="Flag">The parameter hold no value, only it's prefix should be present. The property value must be a <see langword="bool"/>.</param>
public readonly record struct GCodeCommandParameter(string Name, string Prefix, bool ForceInclude, bool Flag);