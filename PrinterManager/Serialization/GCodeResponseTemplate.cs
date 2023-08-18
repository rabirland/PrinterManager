using PrinterManager.Responses;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PrinterManager.Serialization;

/// <summary>
/// A parser template for messages coming from the printer.
/// </summary>
/// <param name="TargetType">The target type to deserialize the response to.</param>
/// <param name="Regex">The parser regex. The names of the groups should match the properties of the target type.</param>
public readonly record struct GCodeResponseTemplate(Type TargetType, Regex Regex)
{
    public static GCodeResponseTemplate Create<T>([StringSyntax("regex")] string regex)
        where T : IPrinterResponse
    {
        return new GCodeResponseTemplate(typeof(T), new Regex(regex, RegexOptions.Compiled));
    }
}
