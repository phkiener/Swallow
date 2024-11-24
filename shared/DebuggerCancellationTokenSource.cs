using System.Diagnostics;

namespace Swallow.Shared;

/// <summary>
/// A special <see cref="CancellationTokenSource"/> that ignores any timeouts or cancellations when a debugger is attached.
/// </summary>
internal sealed class DebuggerCancellationTokenSource : IDisposable
{
    private readonly CancellationTokenSource? cancellationTokenSource = Debugger.IsAttached ? null : new CancellationTokenSource();

    /// <inheritdoc cref="CancellationTokenSource.IsCancellationRequested"/>
    public bool IsCancellationRequested => cancellationTokenSource?.IsCancellationRequested ?? false;

    /// <inheritdoc cref="CancellationTokenSource.CancelAfter(TimeSpan)"/>
    public void CancelAfter(TimeSpan delay) => cancellationTokenSource?.CancelAfter(delay);

    /// <inheritdoc cref="CancellationTokenSource.CancelAfter(int)"/>
    public void CancelAfter(int millisecondsDelay) => cancellationTokenSource?.CancelAfter(millisecondsDelay);

    /// <inheritdoc cref="CancellationTokenSource.Cancel()"/>
    public void Cancel() => cancellationTokenSource?.Cancel();

    /// <inheritdoc cref="CancellationTokenSource.Cancel(bool)"/>
    public void Cancel(bool throwOnFirstException) => cancellationTokenSource?.Cancel(throwOnFirstException);

    /// <inheritdoc cref="CancellationTokenSource.CancelAsync()"/>
    public Task CancelAsync() => cancellationTokenSource?.CancelAsync() ?? Task.CompletedTask;

    /// <inheritdoc cref="CancellationTokenSource.TryReset()"/>
    public bool TryReset() => cancellationTokenSource?.TryReset() ?? false;

    /// <inheritdoc cref="CancellationTokenSource.Token"/>
    public CancellationToken Token => cancellationTokenSource?.Token ?? CancellationToken.None;

    /// <inheritdoc cref="CancellationTokenSource.Dispose()"/>
    public void Dispose()
    {
        cancellationTokenSource?.Dispose();
    }
}
