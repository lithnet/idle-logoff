using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Lithnet.idlelogoff
{
    public class NativeMethods
    {
        [DllImport("powrprof.dll")]
        private static extern int CallNtPowerInformation(
            POWER_INFORMATION_LEVEL informationLevel,
            IntPtr lpInputBuffer,
            int nInputBufferSize,
            out EXECUTION_STATE state,
            int nOutputBufferSize
        );

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool GetLastInputInfo(ref LastInputInfo lastInputInfo);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ExitWindowsEx(ShutdownFlags flags, int reason);

        public static bool IsDisplayRequested()
        {
            EXECUTION_STATE state;

            int retval = CallNtPowerInformation(POWER_INFORMATION_LEVEL.SystemExecutionState, IntPtr.Zero, 0, out state, sizeof(EXECUTION_STATE));

            if (retval != 0)
            {
                throw new Win32Exception(retval);
            }

            return (state.HasFlag(EXECUTION_STATE.DISPLAY_REQUIRED));
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
            ShutdownFlags flags = ShutdownFlags.Logoff | ShutdownFlags.Force;
            
            if (!ExitWindowsEx(flags, 0))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}