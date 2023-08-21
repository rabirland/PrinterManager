using PrinterManager.Requests;
using PrinterManager.Responses;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PrinterManager.Serialization;

/// <summary>
/// Serializeds requests to GCode commands.
/// </summary>
public static class GCodeSerializer
{
    /// <summary>
    /// Serializes a request into a GCode command.
    /// </summary>
    /// <typeparam name="T">The type of request.</typeparam>
    /// <param name="commandConfiguration">The command configuration.</param>
    /// <returns>The generated GCode command.</returns>
    public static string Serialize<T>(T request, GCodeCommandTemplate template)
        where T : IPrinterRequest
    {
        return Serialize((object)request, template);
    }

    public static string Serialize(object request, GCodeCommandTemplate template)
    {
        string command = template.GCode;

        foreach (var param in template.Parameters)
        {
            bool include = false;

            /// Get the property bound to this parameter.
            var property = request.GetType().GetProperty(param.Name, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new Exception($"Invalid GCode template, the property {param.Name} is not found on type {request.GetType().Name}");
            }

            var propertyValue = property.GetValue(request);

            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Only add the parameter if the value is not null.
                include = object.Equals(propertyValue, null) == false;
            }

            if (include)
            {
                string valueStr = propertyValue is float f ? f.ToString("0.00")
                    : propertyValue is double d ? d.ToString("0.00")
                    : propertyValue is bool b ? (b ? "1" : "0")
                    : propertyValue?.ToString() ?? string.Empty;

                command += $" {param.Prefix}{valueStr}";
            }
        }

        return command;
    }

    public static bool TryDeserialize<T>(string response, GCodeResponseTemplate template, out T obj)
        where T : new()
    {
        obj = default;
        var match = template.Regex.Match(response);

        if (match.Success == false)
        {
            return false;
        }

        var ret = DeserializeIntoObject(typeof(T), match, out var result);
        obj = (T)result;
        return ret;
    }

    public static bool TryDeserialize(string response, GCodeResponseTemplate template, out object obj)
    {
        obj = default;
        var match = template.Regex.Match(response);

        if (match.Success == false)
        {
            return false;
        }

        var ret = DeserializeIntoObject(template.TargetType, match, out var result);
        obj = result;
        return ret;
    }

    private static bool DeserializeIntoObject(Type targetType, Match match, out object result)
    {
        result = null;

        var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        object? ret = targetType.GetConstructor(Array.Empty<Type>())?.Invoke(null);

        if (ret == null)
        {
            throw new Exception($"Type '{targetType.Name}' is missing the default constructor");
        }

        foreach (var property in properties)
        {
            if (property.SetMethod == null)
            {
                continue;
            }

            var group = match.Groups[property.Name];

            if (group == null)
            {
                continue;
            }

            object value = 0;

            if (property.PropertyType == typeof(string))
            {
                value = group.Value;
            }
            else if (property.PropertyType == typeof(byte))
            {
                value = byte.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(sbyte))
            {
                value = sbyte.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(short))
            {
                value = short.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(ushort))
            {
                value = ushort.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(int))
            {
                value = int.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(uint))
            {
                value = uint.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(long))
            {
                value = long.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(ulong))
            {
                value = ulong.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(float))
            {
                value = float.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(double))
            {
                value = double.Parse(group.Value);
            }
            else if (property.PropertyType == typeof(bool))
            {
                value = string.IsNullOrEmpty(group.Value) == false;
            }
            else if (property.PropertyType.IsAssignableTo(typeof(IPrinterResponse)))
            {
                value = DeserializeIntoObject(property.PropertyType, match, out value);
            }
            else
            {
                throw new Exception($"Can not deserialize into property of type '{property.PropertyType.Name}'");
            }

            property.SetValue(ret, value);
        }

        result = ret;
        return true;
    }
}
