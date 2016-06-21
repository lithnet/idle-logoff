namespace Lithnet.idlelogoff
{
    using System;
    using System.Windows.Forms;
    using System.Diagnostics;

    public static class Program
    {
        private static int lastTicks;
        private static DateTime lastDateTime;
        private static Timer eventTimer;
        private static bool inTimer = false;
        private static bool backgroundMode = false;
        private static bool debug = false;
        private static int initialTime = 0;

        [STAThread]
        public static void Main()
        {
            try
            {
                if (Debugger.IsAttached)
                {
                    debug = true;
                }

                EventLogging.InitEventLog();
                ValidateCommandLineArgs();

                if (backgroundMode)
                {
                    InitTimer();
                    Application.Run();
                }
                else
                {
                    LaunchGUI();
                }
            }
            catch (Exception ex)
            {
                EventLogging.TryLogEvent($"The program encountered an unexpected error\n{ex.Message}\n{ex.StackTrace}", 9, EventLogEntryType.Error);
            }
        }

        private static void InitTimer()
        {
            int logoffidletime = Settings.IdleLimit * 60 * 1000;
            initialTime = logoffidletime;

            if (Settings.Enabled)
            {
                EventLogging.TryLogEvent("The application has started. User " + Environment.UserDomainName + "\\" + Environment.UserName + " will be logged off after being idle for " + Settings.IdleLimit + " minutes", EventLogging.EVT_TIMERSTARTED);
            }
            else
            {
                EventLogging.TryLogEvent("The application has started, but is not enabled. User " + Environment.UserDomainName + "\\" + Environment.UserName + " will not be logged off automatically", EventLogging.EVT_TIMERSTARTED);
            }

            Program.eventTimer = new Timer();
            Program.eventTimer.Tick += EventTimer_Tick;
            Program.eventTimer.Interval = 60 * 1000;
            Program.eventTimer.Start();
        }

        private static void ValidateCommandLineArgs()
        {
            string[] cmdargs = Environment.GetCommandLineArgs();

            if (cmdargs.Length <= 1)
            {
                return;
            }

            foreach (string arg in cmdargs)
            {
                if (arg == cmdargs[0])
                {
                    //skip over the executable itself
                }
                else if (arg.ToLower() == "/register")
                {
                    try
                    {
                        PerformApplicationRegistration();
                        Environment.Exit(0);
                    }
                    catch
                    {
                        Environment.Exit(1);
                    }
                }
                else if (arg.ToLower() == "/start")
                {
                    backgroundMode = true;
                }
                else
                {
                    MessageBox.Show("An invalid command line argument was specified: " + arg);
                    Environment.Exit(1);
                }
            }


        }

        private static void EventTimer_Tick(object sender, EventArgs e)
        {
            if (inTimer)
            {
                return;
            }

            if (!Settings.Enabled)
            {
                return;
            }

            try
            {
                Program.inTimer = true;

                int logoffidletime = Settings.IdleLimit * 60 * 1000;

                if (initialTime != logoffidletime)
                {
                    EventLogging.TryLogEvent($"Idle timeout limit has changed. User {Environment.UserDomainName}\\{Environment.UserName} will now be logged off after {Settings.IdleLimit} minutes", EventLogging.EVT_TIMERINTERVALCHANGED);
                }

                int currentticks = NativeMethods.GetLastInputTime();

                if (currentticks != Program.lastTicks)
                {
                    Program.lastTicks = currentticks;
                    Program.lastDateTime = DateTime.Now;
                    return;
                }

                if (DateTime.Now.Subtract(Program.lastDateTime).TotalMilliseconds > logoffidletime)
                {
                    EventLogging.TryLogEvent($"User {Environment.UserName} has passed the idle time limit of {Settings.IdleLimit} minutes. Initating logoff.", EventLogging.EVT_LOGOFFEVENT);

                    if (!debug)
                    {
                        try
                        {
                            NativeMethods.LogOffUser();
                            Program.eventTimer.Stop();
                            Program.lastTicks = currentticks;
                            Program.lastDateTime = DateTime.Now;
                        }
                        catch (Exception ex)
                        {
                            EventLogging.TryLogEvent("An error occured trying to log off the user\n" + ex.Message, EventLogging.EVT_LOGOFFFAILED);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Idle time limit reached");
                    }
                }
            }
            finally
            {
                Program.inTimer = false;
            }
        }

        private static void PerformApplicationRegistration()
        {
            try
            {
                EventLogging.RegisterEventSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to register the event source. Ensure you are running the application with administrative rights.\n\n" + ex.Message);
                throw;
            }

            try
            {
                Settings.CreateStartupRegKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to register the application. Ensure you are running the application with administrative rights.\n\n" + ex.Message);
                throw;
            }
        }

        private static void LaunchGUI()
        {
            if (AdminCheck.IsRunningAsAdmin())
            {
                Application.EnableVisualStyles();
                Application.Run(new frmSettings());
            }
            else
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    bool usercanceled;

                    if (AdminCheck.TryRestartElevated(out usercanceled))
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        if (usercanceled)
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            MessageBox.Show("This application must be run with administrative rights", "Lithnet.idlelogoff", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This application must be run with administrative rights", "Lithnet.idlelogoff", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
            }
        }
    }
}
