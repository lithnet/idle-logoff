namespace Lithnet.idlelogoff
{
    using System.Runtime.InteropServices;
    using System.ComponentModel;

    public class NativeMethods
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool GetLastInputInfo(ref LastInputInfo lastInputInfo);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ExitWindowsEx(ShutdownFlags flags, int reason);

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