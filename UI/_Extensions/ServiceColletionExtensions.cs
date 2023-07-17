using Microsoft.Extensions.DependencyInjection;

namespace UI;

public static class ServiceColletionExtensions
{
    /// <summary>
    /// Registers every type with all their interfaces that implements/inherits <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The root type to register.</typeparam>
    /// <param name="collection">The service collection.</param>
    public static void RegisterAllScoped<T>(this ServiceCollection collection)
    {
        var types = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)))
            .ToArray();

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var interf in interfaces)
            {
                collection.AddScoped(interf, type);
            }
        }
    }
}
