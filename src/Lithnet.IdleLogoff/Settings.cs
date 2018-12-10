using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Lithnet.idlelogoff
{
    public static class Settings
    {
        private static string strBaseSettingsKey = "Software\\Lithnet\\IdleLogOff";
        private static string strBasePolicyKey = "Software\\Policies\\Lithnet\\IdleLogOff";
        private static RegistryKey regkeySettings;
        private static RegistryKey regkeySettingsWritable;
        private static RegistryKey regkeyMachinePolicy;
        private static RegistryKey regkeyUserPolicy;

        private static RegistryKey SettingsKeyReadOnly
        {
            get
            {
                try
                {
                    if (Settings.regkeySettings == null)
                    {
                        Settings.regkeySettings = Registry.LocalMachine.OpenSubKey(strBaseSettingsKey);
                    }
                    else
                    {
                        try
                        {
                            int x = Settings.regkeySettings.ValueCount;
                        }
                        catch // catch handler for a key that is missing and no longer valid
                        {
                            Settings.regkeySettings = Registry.LocalMachine.OpenSubKey(strBaseSettingsKey);
                        }
                    }
                }
                catch
                {
                    // could not open registry key
                }

                return Settings.regkeySettings;
            }
        }

        private static RegistryKey SettingsKeyWriteable
        {
            get
            {
                try
                {

                    if (Settings.regkeySettingsWritable == null)
                    {

                        Settings.regkeySettingsWritable = Registry.LocalMachine.CreateSubKey(strBaseSettingsKey);
                    }
                    else
                    {
                        try
                        {
                            int x = Settings.regkeySettingsWritable.ValueCount;
                        }
                        catch // catch handler for a key that is missing and no longer valid
                        {
                            Settings.regkeySettingsWritable = Registry.LocalMachine.CreateSubKey(strBaseSettingsKey);
                        }
                    }
                }
                catch
                {
                    // could not open registry key
                }

                return Settings.regkeySettingsWritable;

            }

        }

        private static RegistryKey MachinePolicyKey
        {
            get
            {
                if (Settings.regkeyMachinePolicy == null)
                {
                    Settings.regkeyMachinePolicy = Registry.LocalMachine.OpenSubKey(strBasePolicyKey, false);
                }
                else
                {
                    try
                    {
                        int x = Settings.regkeyMachinePolicy.ValueCount;
                    }
                    catch // catch handler for a key that is missing and no longer valid
                    {
                        Settings.regkeyMachinePolicy = Registry.LocalMachine.OpenSubKey(strBasePolicyKey, false);
                    }
                }
                return Settings.regkeyMachinePolicy;
            }

        }

        private static RegistryKey UserPolicyKey
        {
            get
            {
                if (Settings.regkeyUserPolicy == null)
                {
                    Settings.regkeyUserPolicy = Registry.CurrentUser.OpenSubKey(strBasePolicyKey, false);
                }
                else
                {
                    try
                    {
                        int x = Settings.regkeyUserPolicy.ValueCount;
                    }
                    catch // catch handler for a key that is missing and no longer valid
                    {
                        Settings.regkeyUserPolicy = Registry.CurrentUser.OpenSubKey(strBasePolicyKey, false);
                    }
                }

                return Settings.regkeyUserPolicy;
            }
        }

        private static object GetPolicyOrSetting(string name)
        {
            object value = null;

            if (MachinePolicyKey != null)
            {
                value = MachinePolicyKey.GetValue(name, null);
                if (value != null)
                {
                    return value;
                }
            }

            if (UserPolicyKey != null)
            {
                value = UserPolicyKey.GetValue(name, null);
                if (value != null)
                {
                    return value;
                }
            }

            if (SettingsKeyReadOnly != null)
            {
                value = SettingsKeyReadOnly.GetValue(name, null);
            }

            return value;
        }

        public static bool IsSettingFromPolicy(string name)
        {
            object value = null;

            if (MachinePolicyKey != null)
            {
                value = MachinePolicyKey.GetValue(name, null);
                if (value != null)
                {
                    return true;
                }
            }

            if (UserPolicyKey != null)
            {
                value = UserPolicyKey.GetValue(name, null);
                if (value != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static void SaveSetting(string valueName, object value, RegistryValueKind valuetype)
        {
            SettingsKeyWriteable.SetValue(valueName, value, valuetype);
        }

        public static int IdleLimit
        {
            get
            {
                object regvalue = null;
                int retval = 60;

                regvalue = GetPolicyOrSetting("IdleLimit");
                if (regvalue != null)
                {
                    try
                    {
                        retval = (int)regvalue;
                    }
                    catch
                    {
                        //unable to cast from an object to a string
                    }
                }

                return retval;
            }
            set => SaveSetting("IdleLimit", value, RegistryValueKind.DWord);
        }

        public static int WarningPeriod
        {
            get
            {
                object regvalue = null;
                int retval = 0;

                regvalue = GetPolicyOrSetting("WarningPeriod");
                if (regvalue != null)
                {
                    try
                    {
                        retval = (int)regvalue;
                    }
                    catch
                    {
                        //unable to cast from an object to a string
                    }
                }

                return retval;
            }
            set => SaveSetting("WarningPeriod", value, RegistryValueKind.DWord);
        }

        public static bool Debug
        {
            get
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    return true;
                }

                object value = null;
                bool status = false;

                value = GetPolicyOrSetting("Debug");
                if (value != null)
                {
                    try
                    {
                        if ((int) value == 1)
                        {
                            status = true;
                        }
                    }
                    catch
                    {
                        //unable to cast 
                    }

                }

                return status;
            }
        }

        public static bool Enabled
        {
            get
            {
                object value = null;
                bool status = false;

                value = GetPolicyOrSetting("Enabled");
                if (value != null)
                {
                    try
                    {
                        if ((int)value == 1)
                        {
                            status = true;
                        }
                    }
                    catch
                    {
                        //unable to cast 
                    }

                }
                return status;
            }
            set => SaveSetting("Enabled", Convert.ToInt32(value), RegistryValueKind.DWord);
        }

        public static IdleTimeoutAction Action
        {
            get
            {
                int retval = 0;

                object regvalue = Settings.GetPolicyOrSetting("Action");
                if (regvalue != null)
                {
                    try
                    {
                        retval = (int)regvalue;
                    }
                    catch
                    {
                        //unable to cast from an object to a string
                    }
                }

                return (IdleTimeoutAction)retval;
            }
            set => SaveSetting("Action", (int)value, RegistryValueKind.DWord);
        }

        public static bool IgnoreDisplayRequested
        {
            get
            {
                object value = null;
                bool status = false;

                value = GetPolicyOrSetting("IgnoreDisplayRequested");
                if (value != null)
                {
                    try
                    {
                        if ((int)value == 1)
                        {
                            status = true;
                        }
                    }
                    catch
                    {
                        //unable to cast 
                    }

                }
                return status;
            }
            set => SaveSetting("IgnoreDisplayRequested", Convert.ToInt32(value), RegistryValueKind.DWord);
        }


        public static void Release()
        {
            if (Settings.regkeyMachinePolicy != null)
            {
                Settings.regkeyMachinePolicy.Close();
                Settings.regkeyMachinePolicy = null;
            }

            if (Settings.regkeyUserPolicy != null)
            {
                Settings.regkeyUserPolicy.Close();
                Settings.regkeyUserPolicy = null;
            }

            if (Settings.regkeySettings != null)
            {
                Settings.regkeySettings.Close();
                Settings.regkeySettings = null;
            }
        }

        public static void CreateStartupRegKey()
        {
            RegistryKey hklmRun = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            string cmdline = "\"" + System.Windows.Forms.Application.ExecutablePath + "\" /start";

            hklmRun.SetValue("Lithnet.idlelogoff", cmdline);
        }

        public static void DeleteStartupRegKey()
        {
            RegistryKey hklmRun = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            hklmRun.DeleteValue("Lithnet.idlelogoff", false);
        }
    }
}
