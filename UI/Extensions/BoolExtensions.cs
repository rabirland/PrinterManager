using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Extensions;

/// <summary>
/// Extension methods for <see cref="bool"/>.
/// </summary>
public static class BoolExtensions
{
    /// <summary>
    /// Implements a ternary operator in a functional program style.
    /// </summary>
    /// <typeparam name="T">The type of return value.</typeparam>
    /// <param name="condition">The condition to check.</param>
    /// <param name="whenTrue">The value to return when the condition is <see langword="true"/>.</param>
    /// <param name="whenFalse">The value to return when the condition is <see langword="false"/>.</param>
    /// <returns>Either <paramref name="whenTrue"/> or <paramref name="whenFalse"/> depending on <paramref name="condition"/>.</returns>
    public static T If<T>(this bool condition, T whenTrue, T whenFalse = default)
    {
        if (condition)
        {
            return whenTrue;
        }
        else
        {
            return whenFalse;
        }
    }
}
