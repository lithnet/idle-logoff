using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;

namespace Lithnet.idlelogoff
{
    class AdminCheck
    {
        public static bool TryRestartElevated(out bool userCanceled)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Application.ExecutablePath;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                startInfo.Arguments = "/attach";
            }

            startInfo.Verb = "runas";
            userCanceled = false;

            try
            {
                Process p = Process.Start(startInfo);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1223)
                {
                    userCanceled = true;
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsRunningAsAdmin()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
