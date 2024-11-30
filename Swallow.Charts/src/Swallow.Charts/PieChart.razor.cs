using System.Numerics;
using Microsoft.AspNetCore.Components;

namespace Swallow.Charts;

/// <summary>
/// </summary>
/// <typeparam name="TKey">Type of the key of each data point.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
public sealed partial class PieChart<TKey, TValue> : ComponentBase
    where TKey : IEquatable<TKey>, IComparable<TKey>
    where TValue : INumber<TValue>
{
    [Parameter]
    [EditorRequired]
    public required DataSeries<TKey, TValue> Data { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?> AdditionalAttributes { get; set; } = new Dictionary<string, object?>();

    private IReadOnlyList<PieSlice> slices = [];

    protected override void OnParametersSet()
    {
        var enumeratedValues = Data.Values.ToArray();

        var totalValue = enumeratedValues.Sum(static v => double.CreateChecked(v.Value));

        var seenRadians = 0d;
        var lastPoint = new DoubleCoordinate(0, 1);

        var inProgress = new List<PieSlice>();
        foreach (var point in Data.Values)
        {
            var start = lastPoint;
            var angle = double.CreateChecked(point.Value) / totalValue * 360;

            var radians = double.DegreesToRadians(angle);
            seenRadians += radians;
            var end = new DoubleCoordinate(X: Math.Round(Math.Sin(seenRadians), 3), Y: Math.Round(Math.Cos(seenRadians), 3));

            var slice = new PieSlice(start, end, angle);
            inProgress.Add(slice);
            lastPoint = slice.End;
        }

        slices = inProgress.ToArray();
    }

    private readonly record struct DoubleCoordinate(double X, double Y);

    private readonly record struct PieSlice(DoubleCoordinate Start, DoubleCoordinate End, double Angle);
}


