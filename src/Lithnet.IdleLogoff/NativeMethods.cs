using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace Lithnet.idlelogoff
{
    public class NativeMethods
    {
        internal const int SePrivilegeEnabled = 0x00000002;
        internal const int TokenQuery = 0x00000008;
        internal const int TokenAdjustPrivileges = 0x00000020;
        internal const string SeShutdownName = "SeShutdownPrivilege";

        [DllImport("powrprof.dll")]
        private static extern int CallNtPowerInformation(
            PowerInformationLevel informationLevel,
            IntPtr lpInputBuffer,
            int nInputBufferSize,
            out ExecutionState state,
            int nOutputBufferSize
        );


        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int OpenProcessToken(IntPtr processHandle, int desiredAccess, ref IntPtr tokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LookupPrivilegeValue(string systemName, string name, ref long luid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, ref TokenPrivileges newState, int bufferLength, IntPtr previousState, IntPtr length);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool GetLastInputInfo(ref LastInputInfo lastInputInfo);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ExitWindowsEx(ShutdownFlags flags, int reason);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        public static bool IsDisplayRequested()
        {
            ExecutionState state;

            int retval = CallNtPowerInformation(PowerInformationLevel.SystemExecutionState, IntPtr.Zero, 0, out state, sizeof(ExecutionState));

            if (retval != 0)
            {
                throw new Win32Exception(retval);
            }

            return (state.HasFlag(ExecutionState.DisplayRequired));
        }

        public static int GetLastInputTime()
        {
            LastInputInfo info = new LastInputInfo();
            info.cbSize = Marshal.SizeOf(info);

            if (!GetLastInputInfo(ref info))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return info.dwTime;
        }

        public static void LogOffUser()
        {
            Mutex mutex = null;
            bool hasLock = false;

            try
            {
                MutexAccessRule ace = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
                MutexSecurity mutexSecurity = new MutexSecurity();
                mutexSecurity.AddAccessRule(ace);
                mutex = new Mutex(false, "Global\\lithnet.idlelogoff", out bool createdNew, mutexSecurity);

                mutex.WaitOne();
                hasLock = true;

                ShutdownFlags flags;
                bool elevated = false;
                bool isLastSession = false;

                if (Settings.Action == IdleTimeoutAction.Reboot || Settings.Action == IdleTimeoutAction.Shutdown)
                {
                    try
                    {
                        ElevatePrivileges();
                        elevated = true;
                    }
                    catch (Exception ex)
                    {
                        EventLogging.TryLogEvent($"Could not get workstation shutdown permissions. Logging off instead\n{ex}", EventLogging.EvtRestartfailed, EventLogEntryType.Error);
                    }

                    Process[] p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

                    isLastSession = p.Length <= 1;

                    if (!isLastSession)
                    {
                        EventLogging.TryLogEvent($"{Settings.Action} will not be performed as other sessions are still active. Logging off instead", EventLogging.EvtSessioninuse, EventLogEntryType.Warning);
                    }
                }

                if (isLastSession && elevated && Settings.Action == IdleTimeoutAction.Shutdown)
                {
                    flags = ShutdownFlags.Shutdown | ShutdownFlags.Force;
                }
                else if (isLastSession && elevated && Settings.Action == IdleTimeoutAction.Reboot)
                {
                    flags = ShutdownFlags.Reboot | ShutdownFlags.Force;
                }
                else
                {
                    flags = ShutdownFlags.Logoff | ShutdownFlags.Force;
                }

                if (Settings.Debug)
                {
                    EventLogging.TryLogEvent($"Performed {flags} for user {Environment.UserName}", 0, EventLogEntryType.Information);
                }
                else
                {
                    if (!ExitWindowsEx(flags, 0))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
            }
            finally
            {
                if (hasLock)
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        private static void ElevatePrivileges()
        {
            IntPtr currentProcess = GetCurrentProcess();
            IntPtr tokenHandle = IntPtr.Zero;

            try
            {
                int result = OpenProcessToken(currentProcess, NativeMethods.TokenAdjustPrivileges | NativeMethods.TokenQuery, ref tokenHandle);

                if (result == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                TokenPrivileges tokenPrivileges;
                tokenPrivileges.PrivilegeCount = 1;
                tokenPrivileges.Luid = 0;
                tokenPrivileges.Attributes = NativeMethods.SePrivilegeEnabled;

                result = LookupPrivilegeValue(null, NativeMethods.SeShutdownName, ref tokenPrivileges.Luid);
                if (result == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                result = AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
                if (result == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (currentProcess != IntPtr.Zero)
                {
                    CloseHandle(currentProcess);
                }

                if (tokenHandle != IntPtr.Zero)
                {
                    CloseHandle(tokenHandle);
                }
            }
        }
    }
}