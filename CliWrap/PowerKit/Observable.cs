using System;

namespace CliWrap.PowerKit;

file class Observable<T>(Func<IObserver<T>, IDisposable> subscribe) : IObservable<T>
{
    public IDisposable Subscribe(IObserver<T> observer) => subscribe(observer);
}

/// <summary>
/// Provides utility methods for creating <see cref="IObservable{T}" /> instances.
/// </summary>
internal static class Observable
{
    /// <summary>
    /// Creates an observable that invokes the specified subscribe function when subscribed to.
    /// </summary>
    public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe) =>
        new Observable<T>(observer =>
        {
            var autoDetach = new AutoDetachObserver<T>(observer);
            var disposable = subscribe(autoDetach);
            autoDetach.SetSubscription(disposable);
            return autoDetach;
        });

    /// <summary>
    /// Creates an observable that invokes the specified subscribe function when subscribed to,
    /// wrapping the observer in a <see cref="SynchronizedObserver{T}" /> to ensure thread safety.
    /// </summary>
    public static IObservable<T> CreateSynchronized<T>(Func<IObserver<T>, IDisposable> subscribe) =>
        Create<T>(observer => subscribe(new SynchronizedObserver<T>(observer)));
}
