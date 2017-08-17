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
        private static RegistryKey _regkeySettings;
        private static RegistryKey _regkeySettingsWritable;
        private static RegistryKey _regkeyMachinePolicy;
        private static RegistryKey _regkeyUserPolicy;

        private static RegistryKey SettingsKeyReadOnly
        {
            get
            {
                try
                {
                    if (_regkeySettings == null)
                    {
                        _regkeySettings = Registry.LocalMachine.OpenSubKey(strBaseSettingsKey);
                    }
                    else
                    {
                        try
                        {
                            int x = _regkeySettings.ValueCount;
                        }
                        catch // catch handler for a key that is missing and no longer valid
                        {
                            _regkeySettings = Registry.LocalMachine.OpenSubKey(strBaseSettingsKey);
                        }
                    }
                }
                catch
                {
                    // could not open registry key
                }

                return _regkeySettings;
            }
        }

        private static RegistryKey SettingsKeyWriteable
        {
            get
            {
                try
                {

                    if (_regkeySettingsWritable == null)
                    {

                        _regkeySettingsWritable = Registry.LocalMachine.CreateSubKey(strBaseSettingsKey);
                    }
                    else
                    {
                        try
                        {
                            int x = _regkeySettingsWritable.ValueCount;
                        }
                        catch // catch handler for a key that is missing and no longer valid
                        {
                            _regkeySettingsWritable = Registry.LocalMachine.CreateSubKey(strBaseSettingsKey);
                        }
                    }
                }
                catch
                {
                    // could not open registry key
                }

                return _regkeySettingsWritable;

            }

        }

        private static RegistryKey MachinePolicyKey
        {
            get
            {
                if (_regkeyMachinePolicy == null)
                {
                    _regkeyMachinePolicy = Registry.LocalMachine.OpenSubKey(strBasePolicyKey, false);
                }
                else
                {
                    try
                    {
                        int x = _regkeyMachinePolicy.ValueCount;
                    }
                    catch // catch handler for a key that is missing and no longer valid
                    {
                        _regkeyMachinePolicy = Registry.LocalMachine.OpenSubKey(strBasePolicyKey, false);
                    }
                }
                return _regkeyMachinePolicy;
            }

        }

        private static RegistryKey UserPolicyKey
        {
            get
            {
                if (_regkeyUserPolicy == null)
                {
                    _regkeyUserPolicy = Registry.CurrentUser.OpenSubKey(strBasePolicyKey, false);
                }
                else
                {
                    try
                    {
                        int x = _regkeyUserPolicy.ValueCount;
                    }
                    catch // catch handler for a key that is missing and no longer valid
                    {
                        _regkeyUserPolicy = Registry.CurrentUser.OpenSubKey(strBasePolicyKey, false);
                    }
                }

                return _regkeyUserPolicy;
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

        private static void SaveSetting(string ValueName, object value, RegistryValueKind valuetype)
        {
            SettingsKeyWriteable.SetValue(ValueName, value, valuetype);
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
            set
            {
                SaveSetting("IdleLimit", value, RegistryValueKind.DWord);
            }
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
            set
            {
                SaveSetting("Enabled", Convert.ToInt32(value), RegistryValueKind.DWord);
            }
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
            set
            {
                SaveSetting("Action", (int)value, RegistryValueKind.DWord);
            }
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
            set
            {
                SaveSetting("IgnoreDisplayRequested", Convert.ToInt32(value), RegistryValueKind.DWord);
            }
        }


        public static void Release()
        {
            if (_regkeyMachinePolicy != null)
            {
                _regkeyMachinePolicy.Close();
                _regkeyMachinePolicy = null;
            }

            if (_regkeyUserPolicy != null)
            {
                _regkeyUserPolicy.Close();
                _regkeyUserPolicy = null;
            }

            if (_regkeySettings != null)
            {
                _regkeySettings.Close();
                _regkeySettings = null;
            }
        }

        public static void CreateStartupRegKey()
        {
            RegistryKey _HKLMRun = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            string cmdline = "\"" + System.Windows.Forms.Application.ExecutablePath + "\" /start";

            _HKLMRun.SetValue("Lithnet.idlelogoff", cmdline);
        }

        public static void DeleteStartupRegKey()
        {
            RegistryKey _HKLMRun = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            _HKLMRun.DeleteValue("Lithnet.idlelogoff", false);
        }
    }
}
