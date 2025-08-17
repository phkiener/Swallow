#nullable enable
using System.Collections.Frozen;

namespace Swallow.Validation.Internal;

internal static class TypeExtensions
{
    private static readonly FrozenDictionary<Type, string> friendlyNames = new Dictionary<Type, string>
    {
        [typeof(string)] = "string"
    }.ToFrozenDictionary();

    public static string FriendlyName(this Type type)
    {
        return friendlyNames.TryGetValue(type, out var friendlyName) ? friendlyName : type.Name;
    }
}
