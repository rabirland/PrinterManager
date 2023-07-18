using PrinterManager.Responses;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PrinterManager.Serialization;

// Parses messages coming from the printer.
public class GCodeParser
{
    public static IPrinterResponse Parse(string response, GCodeResponseTemplate[] templates)
    {
        foreach (var template in templates)
        {
            var match = template.Regex.Match(response);

            if (match.Success == false)
            {
                continue;
            }

            var constructors = template.TargetType.GetConstructors();
            var emptyConstructor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);
            if (constructors.Length == 0 || emptyConstructor != null)
            {
                return ConstructFromProperties(match, template.TargetType);
            }
            else
            {
                return ConstructFromConstructor(match, constructors[0], template.TargetType);
            }
        }

        throw new Exception("Can not find a template that matches the response");
    }

    private static IPrinterResponse ConstructFromProperties(Match match, Type targetType)
    {
        var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var ret = (IPrinterResponse?)Activator.CreateInstance(targetType);

        if (ret == null)
        {
            throw new Exception("Could not create response object");
        }

        for (int i = 0; i < match.Groups.Count; i++)
        {
            var group = match.Groups[i];
            var property = properties.FirstOrDefault(p => string.Equals(p.Name, group.Name, StringComparison.InvariantCultureIgnoreCase));

            if (property == null)
            {
                throw new Exception($"Invalid template regex, the property {group.Name} is missing from the type {targetType.Name}");
            }

            if (property.SetMethod == null)
            {
                throw new Exception($"Invalid template regex, the property {group.Name} on type {targetType.Name} is missing a setter");
            }

            var value = ParseType(property.PropertyType, group.Value);
            property.SetValue(ret, value);
        }

        return ret;
    }

    private static IPrinterResponse ConstructFromConstructor(Match match, ConstructorInfo constructor, Type targetType)
    {
        var constructorParameters = constructor.GetParameters();
        object?[] parameterValues = new object[constructorParameters.Length];
        var groups = match.Groups.Cast<Group>();

        for (int i = 0; i < constructorParameters.Length; i++)
        {
            var parameter = constructorParameters[i];
            var group = groups.FirstOrDefault(g => string.Equals(parameter.Name, g.Name, StringComparison.InvariantCultureIgnoreCase));

            if (group != null)
            {
                parameterValues[i] = ParseType(parameter.ParameterType, group.Value);
            }
            else
            {
                parameterValues[i] = DefaultValue(parameter.ParameterType);
            }
        }

        var ret = Activator.CreateInstance(targetType, parameterValues);

        if (ret == null)
        {
            throw new Exception("Could not create response object");
        }

        return (IPrinterResponse)ret;
    }

    private static object? ParseType(Type targetType, string value)
    {
        if (targetType == typeof(byte)) return byte.Parse(value);
        else if (targetType == typeof(short)) return short.Parse(value);
        else if (targetType == typeof(int)) return int.Parse(value);
        else if (targetType == typeof(long)) return long.Parse(value);

        else if (targetType == typeof(sbyte)) return sbyte.Parse(value);
        else if (targetType == typeof(ushort)) return ushort.Parse(value);
        else if (targetType == typeof(uint)) return uint.Parse(value);
        else if (targetType == typeof(ulong)) return ulong.Parse(value);

        else if (targetType == typeof(float)) return float.Parse(value);
        else if (targetType == typeof(double)) return double.Parse(value);

        else if (targetType == typeof(bool)) return value is "true" or "1";

        if (targetType == typeof(byte?)) return string.IsNullOrEmpty(value) ? null : byte.Parse(value);
        if (targetType == typeof(short?)) return string.IsNullOrEmpty(value) ? null : short.Parse(value);
        if (targetType == typeof(int?)) return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        if (targetType == typeof(long?)) return string.IsNullOrEmpty(value) ? null : long.Parse(value);

        if (targetType == typeof(sbyte?)) return string.IsNullOrEmpty(value) ? null : sbyte.Parse(value);
        if (targetType == typeof(ushort?)) return string.IsNullOrEmpty(value) ? null : ushort.Parse(value);
        if (targetType == typeof(uint?)) return string.IsNullOrEmpty(value) ? null : uint.Parse(value);
        if (targetType == typeof(ulong?)) return string.IsNullOrEmpty(value) ? null : ulong.Parse(value);

        if (targetType == typeof(float?)) return string.IsNullOrEmpty(value) ? null : float.Parse(value);
        if (targetType == typeof(double?)) return string.IsNullOrEmpty(value) ? null : double.Parse(value);

        if (targetType == typeof(bool?)) return string.IsNullOrEmpty(value) ? null : value is "true" or "1";

        else throw new Exception($"Unknown value type: {targetType.Name}");
    }

    private static object? DefaultValue(Type targetType)
    {
        if (targetType.IsValueType)
        {
            return Activator.CreateInstance(targetType);
        }

        return null;
    }
}
