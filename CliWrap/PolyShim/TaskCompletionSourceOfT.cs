#pragma warning disable CS0436

using System;
using System.Threading;
using System.Threading.Tasks;
internal static class MemberPolyfills_Net50_TaskCompletionSourceOfT
{
    extension<T>(TaskCompletionSource<T> source)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.taskcompletionsource-1.setcanceled#system-threading-tasks-taskcompletionsource-1-setcanceled(system-threading-cancellationtoken)
        public void SetCanceled(CancellationToken cancellationToken)
        {
            if (!source.TrySetCanceled(cancellationToken))
            {
                throw new InvalidOperationException(
                    "The task is already completed, canceled, or failed."
                );
            }
        }
    }
}