using System.Numerics;
using Microsoft.AspNetCore.Components;

namespace Swallow.Charts;

public enum Interpolation
{
    Linear, StepStart, StepEnd
}

/// <summary>
/// A chart representing one or more <see cref="DataSeries{TKey,TValue}"/> as lines.
/// </summary>
/// <typeparam name="TKey">Type of the key of each data point.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
public sealed partial class LineChart<TKey, TValue> : ComponentBase
    where TKey : IEquatable<TKey>, IComparable<TKey>
    where TValue : INumber<TValue>
{
    [Parameter]
    [EditorRequired]
    public required IEnumerable<DataSeries<TKey, TValue>> Data { get; set; }

    [Parameter]
    public Interpolation Interpolation { get; set; } = Interpolation.Linear;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?> AdditionalAttributes { get; set; } = new Dictionary<string, object?>();

    private TValue maxValue = TValue.Zero;
    private int valueCount = 0;

    protected override void OnParametersSet()
    {
        maxValue = Data.Max(static series => series.Values.Max(static value => value.Value)) ?? TValue.Zero;
        valueCount = Data.Max(static series => series.Values.Count());
    }
}

