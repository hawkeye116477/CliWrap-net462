using System;
using System.Collections.Generic;
using System.Threading;

namespace CliWrap.PowerKit;

file class DelegateDisposable(Action dispose) : IDisposable
{
    private Action? _dispose = dispose;

    public void Dispose() =>
        // Idempotency
        Interlocked.Exchange(ref _dispose, null)?.Invoke();
}

/// <summary>
/// Provides utility methods for creating and composing <see cref="IDisposable" /> instances.
/// </summary>
internal static class Disposable
{
    /// <summary>
    /// Gets a disposable that performs no action when disposed.
    /// </summary>
    public static IDisposable Null { get; } = Create(() => { });

    /// <summary>
    /// Creates a disposable that invokes the specified action when disposed.
    /// </summary>
    /// <remarks>
    /// The returned disposable is idempotent and invokes the action at most once.
    /// </remarks>
    public static IDisposable Create(Action dispose) => new DelegateDisposable(dispose);
}