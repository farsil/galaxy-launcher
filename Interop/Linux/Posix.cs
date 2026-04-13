using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace GalaxyLauncher.Interop.Linux;

[SupportedOSPlatform("linux")]
public static partial class Posix
{
    [LibraryImport("libc.so.6")]
    private static partial int kill(int pid, int sig);

    public static bool Kill(int processId, int signal)
    {
        return kill(processId, signal) == 0;
    }

    public static bool Kill(int processId, PosixSignal signal)
    {
        return Kill(processId, signal switch
        {
            PosixSignal.SIGHUP => 1,
            PosixSignal.SIGINT => 2,
            PosixSignal.SIGQUIT => 3,
            PosixSignal.SIGTERM => 15,
            PosixSignal.SIGCHLD => 17,
            PosixSignal.SIGCONT => 18,
            PosixSignal.SIGTSTP => 20,
            PosixSignal.SIGTTIN => 21,
            PosixSignal.SIGTTOU => 22,
            PosixSignal.SIGWINCH => 28,
            _ => throw new ArgumentException("Invalid signal", nameof(signal))
        });
    }
}