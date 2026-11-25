#pragma warning disable CS0436

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

internal static class MemberPolyfills_Net60_Task
{
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.waitasync#system-threading-tasks-task-waitasync(system-timespan-system-threading-cancellationtoken)
        public async Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var cancellationTask = Task.Delay(timeout, cancellationToken);
            var finishedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

            // Finalize and propagate exceptions
            await finishedTask.ConfigureAwait(false);

            if (finishedTask == cancellationTask)
                throw new TimeoutException("The operation has timed out.");
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.waitasync#system-threading-tasks-task-waitasync(system-threading-cancellationtoken)
        public async Task WaitAsync(CancellationToken cancellationToken) =>
            await task.WaitAsync(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.waitasync#system-threading-tasks-task-waitasync(system-timespan)
        public async Task WaitAsync(TimeSpan timeout) =>
            await task.WaitAsync(timeout, CancellationToken.None).ConfigureAwait(false);
    }

    extension<T>(Task<T> task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1.waitasync#system-threading-tasks-task-1-waitasync(system-timespan-system-threading-cancellationtoken)
        public async Task<T> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var cancellationTask = Task.Delay(timeout, cancellationToken);
            var finishedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

            // Finalize and propagate exceptions
            await finishedTask.ConfigureAwait(false);

            if (finishedTask == cancellationTask)
                throw new TimeoutException("The operation has timed out.");

            // If the exception is not thrown, we can safely assume that the main task is completed
            return task.Result;
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1.waitasync#system-threading-tasks-task-1-waitasync(system-threading-cancellationtoken)
        public async Task<T> WaitAsync(CancellationToken cancellationToken) =>
            await task.WaitAsync(Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1.waitasync#system-threading-tasks-task-1-waitasync(system-timespan)
        public async Task<T> WaitAsync(TimeSpan timeout) =>
            await task.WaitAsync(timeout, CancellationToken.None).ConfigureAwait(false);
    }
}
internal static class MemberPolyfills_Net90_Task
{
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach(system-collections-generic-ienumerable((system-threading-tasks-task)))
        public static async IAsyncEnumerable<Task> WhenEach(IEnumerable<Task> tasks)
        {
            var remaining = new HashSet<Task>(tasks);
            while (remaining.Count > 0)
            {
                var completed = await Task.WhenAny(remaining).ConfigureAwait(false);
                remaining.Remove(completed);

                yield return completed;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach(system-threading-tasks-task())
        public static IAsyncEnumerable<Task> WhenEach(params Task[] tasks) =>
            WhenEach((IEnumerable<Task>)tasks);

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach-1(system-collections-generic-ienumerable((system-threading-tasks-task((-0)))))
        public static async IAsyncEnumerable<Task<T>> WhenEach<T>(IEnumerable<Task<T>> tasks)
        {
            var remaining = new HashSet<Task<T>>(tasks);
            while (remaining.Count > 0)
            {
                var completed = await Task.WhenAny(remaining).ConfigureAwait(false);
                remaining.Remove(completed);

                yield return completed;
            }
        }

        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.wheneach#system-threading-tasks-task-wheneach-1(system-threading-tasks-task((-0))())
        public static IAsyncEnumerable<Task<T>> WhenEach<T>(params Task<T>[] tasks) =>
            WhenEach((IEnumerable<Task<T>>)tasks);
    }
}
internal static class MemberPolyfills_NetCore20_Task
{
    extension(Task task)
    {
        // https://learn.microsoft.com/dotnet/api/system.threading.tasks.task.iscompletedsuccessfully
        public bool IsCompletedSuccessfully => task.Status == TaskStatus.RanToCompletion;
    }
}