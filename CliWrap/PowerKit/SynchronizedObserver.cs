using System;
using System.Threading;
namespace CliWrap.PowerKit;

internal class SynchronizedObserver<T>(IObserver<T> observer) : IObserver<T>
{
    private readonly Lock _lock = new();

    /// <inheritdoc />
    public void OnCompleted()
    {
        lock (_lock)
        {
            observer.OnCompleted();
        }
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        lock (_lock)
        {
            observer.OnError(error);
        }
    }

    /// <inheritdoc />
    public void OnNext(T value)
    {
        lock (_lock)
        {
            observer.OnNext(value);
        }
    }
}
