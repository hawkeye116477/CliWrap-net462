#pragma warning disable CS0436

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Management;
using System.Linq;

internal static class MemberPolyfills_Net50_Process
{
    // Task infrastructure is required for async method support
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexitasync
        public async Task WaitForExitAsync(CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource(
#if !NETFRAMEWORK || NET46_OR_GREATER
                TaskCreationOptions.RunContinuationsAsynchronously
#endif
            );

            void OnExited(object? sender, EventArgs args) => tcs.TrySetResult();

            try
            {
                process.EnableRaisingEvents = true;
            }
            // May throw if the process has already exited
            catch when(process.HasExited)
            {
                return;
            }

            process.Exited += OnExited;

            try
            {
                using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken)))
                    await tcs.Task;
            }
            finally
            {
                process.Exited -= OnExited;
            }
        }
    }
}
internal static class MemberPolyfills_Net70_Process
{
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.waitforexit#system-diagnostics-process-waitforexit(system-timespan)
        public bool WaitForExit(TimeSpan timeout)
        {
            var totalMilliseconds = (long)timeout.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            return process.WaitForExit((int)totalMilliseconds);
        }
    }
}

internal static class MemberPolyfills_NetCore30_Process
{
    extension(Process process)
    {
        // https://learn.microsoft.com/dotnet/api/system.diagnostics.process.kill#system-diagnostics-process-kill(system-boolean)
        public void Kill(bool entireProcessTree)
        {
            static void KillProcessTree(int processId)
            {
                using var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_Process WHERE ParentProcessID={processId}"
                );

                using var results = searcher.Get();

                // Kill the parent process
                try
                {
                    using var proc = Process.GetProcessById(processId);
                    if (!proc.HasExited)
                        proc.Kill();
                }
                catch
                {
                    // Do our best and ignore race conditions
                }

                // Kill the descendants
                foreach (var managementObject in results.Cast<ManagementObject>())
                {
                    var childProcessId = Convert.ToInt32(managementObject["ProcessID"]);
                    KillProcessTree(childProcessId);
                }
            }

            if (!entireProcessTree)
            {
                process.Kill();
                return;
            }

            // Currently, this polyfill only supports Windows
            if (!OperatingSystem.IsWindows())
            {
                process.Kill();
                return;
            }

            KillProcessTree(process.Id);
        }
    }
}
