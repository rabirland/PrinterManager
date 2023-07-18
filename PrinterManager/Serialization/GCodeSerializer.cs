using PrinterManager.Requests;
using System.Reflection;

namespace PrinterManager.Serialization;

/// <summary>
/// Serializeds requests to GCode commands.
/// </summary>
public class GCodeSerializer
{
    /// <summary>
    /// Serializes a request into a GCode command.
    /// </summary>
    /// <typeparam name="T">The type of request.</typeparam>
    /// <param name="commandConfiguration">The command configuration.</param>
    /// <returns>The generated GCode command.</returns>
    public static string Serialize<T>(T request, IEnumerable<GCodeCommandTemplate> templates)
        where T : IPrinterRequest
    {
        var template = templates.First(t => t.RequestType == typeof(T));

        string command = template.GCode;

        foreach (var param in template.Parameters)
        {
            bool include = false;

            /// Get the property bound to this parameter.
            var property = typeof(T).GetProperty(param.Name, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new Exception($"Invalid GCode template, the property {param.Name} is not found on type {typeof(T).Name}");
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
}
