using System.Numerics;

namespace Swallow.Blazor.Charts;

public sealed record DataSeries<TKey, TValue>(string Name, IEnumerable<KeyValuePair<TKey, TValue>> Values)
    where TKey : IEquatable<TKey>, IComparable<TKey>
    where TValue : INumber<TValue>;
