using PrinterManager.Requests;
using System.Reflection;

namespace PrinterManager.Serializer;

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

        // Find a constructor that contains *every* parameter from the template, by their name normaliozed to lowercase.
        var constructor = typeof(T)
            .GetConstructors()
            .FirstOrDefault(c =>
                template.Parameters.All(p =>
                    c.GetParameters().Any(cp =>
                        cp.Name?.ToLowerInvariant() == p.Name.ToLowerInvariant())));

        var constructorParameters = constructor != null
            ? constructor.GetParameters()
            : Array.Empty<ParameterInfo>();

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


            // Check if the parameter need to be included in the GCode. If it's not forced, the parameter will be omitted from the GCode command if:
            // * The C# model has a constructor that contains every parameter from the template, by their names.
            // * The constructor parameter has a default value
            // * The property that matches the name of the constructor parameter contains the default value of the constructor parameter.
            if (param.ForceInclude)
            {
                include = true;
            }
            else
            {
                // Get the matching constructor parameter, if any
                ParameterInfo? constructorParameter = null;

                if (constructorParameters.Length > 0)
                {
                    constructorParameter = constructorParameters.FirstOrDefault(p => p.Name?.ToLowerInvariant() == property.Name.ToLowerInvariant());

                    if (constructorParameter == null)
                    {
                        throw new Exception($"Invalid request model. The model has a constructor that defines every parameter from the template, but the property {property.Name} has no matching constructor parameter by it's name.");
                    }
                }

                if (constructorParameter != null && constructorParameter.HasDefaultValue)
                {
                    // Only add the GCode parameter, if the property doesn't match it's constructor parameter's default value, or the constructor parameter has no default value.
                    include = object.Equals(constructorParameter.DefaultValue, propertyValue) == false;
                }
            }

            if (param.Flag)
            {
                if (property.PropertyType != typeof(bool))
                {
                    throw new Exception($"Invalid template, Flag parameter must have a C# property of type bool");
                }

                if (object.Equals(propertyValue, true))
                {
                    command += $" {param.Prefix}";
                }
            }
            else if (include)
            {
                string valueStr = propertyValue is float f ? f.ToString("0.00")
                : propertyValue is double d ? d.ToString("0.00")
                : propertyValue?.ToString() ?? string.Empty;

                command += $" {param.Prefix}{valueStr}";
            }
        }

        return command;
    }
}
