namespace UI;

/// <summary>
/// Extension methods for <see cref="bool"/>.
/// </summary>
public static class BoolExtensions
{
    /// <summary>
    /// Functional programming style function for the ternary operator.
    /// </summary>
    /// <typeparam name="T">The type of return value.</typeparam>
    /// <param name="condition">The condition to check.</param>
    /// <param name="pass">The value to return if the condition is <see langword="true"/>.</param>
    /// <param name="fail">The value to return if the condition is <see langword="false"/>. The default value is the default of <typeparamref name="T"/>.</param>
    /// <returns></returns>
    public static T? If<T>(this bool condition, T? pass, T? fail = default)
    {
        return condition
            ? pass
            : fail;
    }
}
