using System.Runtime.InteropServices;

namespace CliWrap.Native;

internal static class NativeMethods
{
    public static class Unix
    {
        [DllImport("libc", EntryPoint = "kill", SetLastError = true)]
        public static extern int Kill(int pid, int sig);
    }
}
