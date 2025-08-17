#nullable enable
using System.Collections.Frozen;

namespace Swallow.Validation.Internal;

internal static class TypeExtensions
{
    private static readonly FrozenDictionary<Type, string> friendlyNames = new Dictionary<Type, string>
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
        [typeof(char)] = "char",
        [typeof(string)] = "string",
        [typeof(void)] = "void",
        [typeof(object)] = "object",
    }.ToFrozenDictionary();

    public static string FriendlyName(this Type type)
    {
        if (type.IsGenericType)
        {
            var friendlyTypeParameters = type.GenericTypeArguments.Select(static t => t.FriendlyName());

            var backtickIndex = type.Name.LastIndexOf('`');
            var baseName = type.Name[..backtickIndex];

            return $"{baseName}<{string.Join(", ", friendlyTypeParameters)}>";
        }

        return friendlyNames.TryGetValue(type, out var friendlyName) ? friendlyName : type.Name;
    }
}
