#pragma warning disable CS0436
#pragma warning disable CS9216

namespace System.Threading;

internal partial class Lock
{
    public bool IsHeldByCurrentThread => Monitor.IsEntered(this);

    public void Enter() => Monitor.Enter(this);

    public bool TryEnter() => Monitor.TryEnter(this);

    public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

    public bool TryEnter(int millisecondsTimeout) =>
        TryEnter(TimeSpan.FromMilliseconds(millisecondsTimeout));

    public void Exit() => Monitor.Exit(this);

    public Scope EnterScope()
    {
#if NETFRAMEWORK && !NET40_OR_GREATER
        // Older versions of the framework don't have the overload of Monitor.Enter(...) that accepts a ref bool
        Monitor.Enter(this);
        return new(this);
#else
        var acquiredLock = false;
        try
        {
            Monitor.Enter(this, ref acquiredLock);
            return new(this);
        }
        // Ensure that the lock is released if the owning thread is aborted.
        // Implementation reference:
        // https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/blob/c28041f1e22e561d5cde040704abeeb8d9a18649/Backport.System.Threading.Lock/PreNet5Lock.cs#L112-L125
        // MIT License, Mark Cilia Vincenti
        // https://github.com/Tyrrrz/PolyShim/pull/10#issuecomment-2381456516
        catch (ThreadAbortException)
        {
            if (acquiredLock)
                Monitor.Exit(this);

            throw;
        }
#endif
    }
}

internal partial class Lock
{
    public readonly ref struct Scope(Lock owner)
    {
        public void Dispose() => owner.Exit();
    }
}
