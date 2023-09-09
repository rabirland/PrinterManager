using System.Text.RegularExpressions;

namespace PrinterManager;

/// <summary>
/// Extension methods for <see cref="GroupCollection"/>.
/// </summary>
internal static class GroupCollectionExtensions
{
    /// <summary>
    /// Finds a group by it's name, case-insensitive.
    /// </summary>
    /// <param name="source">The source collection.</param>
    /// <param name="name">The name of the group to look for.</param>
    /// <returns>The group that matches it's name to <paramref name="name"/> case insensitive or <see langword="null"/> if no match is found.</returns>
    public static Group? FindGroup(this IEnumerable<Group> source, string name)
    {
        foreach (var group in source)
        {
            if (group.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            {
                return group;
            }
        }

        return null;
    }
}
