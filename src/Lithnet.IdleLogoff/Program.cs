using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Lithnet.idlelogoff
{
    public static class Program
    {
        private static int lastTicks;
        private static DateTime lastDateTime;
        private static Timer eventTimer;
        private static int inTimer;
        private static bool backgroundMode;
        private static int initialTime;

        [STAThread]
        public static void Main()
        {
            try
            {
                EventLogging.InitEventLog();
                ValidateCommandLineArgs();

                if (backgroundMode)
                {
                    InitTimer();
                    Application.Run();
                }
                else
                {
                    Program.LaunchGui();
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
                EventLogging.TryLogEvent($"The application has started. {Settings.Action} will be performed for user {Environment.UserDomainName}\\{Environment.UserName} after being idle for {Settings.IdleLimit} minutes", EventLogging.EVT_TIMERSTARTED);
            }
            else
            {
                EventLogging.TryLogEvent($"The application has started, but is not enabled. User {Environment.UserDomainName}\\{Environment.UserName} will not be logged off automatically", EventLogging.EVT_TIMERSTARTED);
            }

            Program.eventTimer = new Timer();
            Program.eventTimer.Elapsed += EventTimer_Tick;
            Program.eventTimer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
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
                else if (arg.Equals("/register", StringComparison.OrdinalIgnoreCase))
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
                else if (arg.Equals("/start", StringComparison.OrdinalIgnoreCase))
                {
                    backgroundMode = true;
                }
                else if (arg.Equals("/attach", StringComparison.OrdinalIgnoreCase))
                {
                    Debugger.Launch();
                }
                else
                {
                    MessageBox.Show($"An invalid command line argument was specified: {arg}");
                    Environment.Exit(1);
                }
            }
        }
        
        private static void EventTimer_Tick(object sender, EventArgs e)
        {
            if (!Settings.Enabled)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref Program.inTimer, 1, 0) != 0)
            {
                return;
            }

            try
            {
                int logoffidletime = Settings.IdleLimit * 60 * 1000;

                if (initialTime != logoffidletime)
                {
                    EventLogging.TryLogEvent($"Idle timeout limit has changed. {Settings.Action} will be performed for user {Environment.UserDomainName}\\{Environment.UserName}  after {Settings.IdleLimit} minutes", EventLogging.EVT_TIMERINTERVALCHANGED);
                    initialTime = logoffidletime;
                }

                int currentticks = NativeMethods.GetLastInputTime();

                if (!Settings.IgnoreDisplayRequested && NativeMethods.IsDisplayRequested())
                {
                    Trace.WriteLine("An application has requested the system display stay on");
                    Program.lastDateTime = DateTime.Now;
                    Program.lastTicks = currentticks;
                    return;
                }


                if (currentticks != Program.lastTicks)
                {
                    Trace.WriteLine("Input received");
                    Program.lastTicks = currentticks;
                    Program.lastDateTime = DateTime.Now;
                    return;
                }

                if (DateTime.Now.Subtract(Program.lastDateTime).TotalMilliseconds > logoffidletime)
                {
                    EventLogging.TryLogEvent($"User {Environment.UserName} has passed the idle time limit of {Settings.IdleLimit} minutes. Initiating {Settings.Action}.", EventLogging.EVT_LOGOFFEVENT);
                    Program.eventTimer.Stop();

                    try
                    {
                        NativeMethods.LogOffUser();
                    }
                    catch (Exception ex)
                    {
                        EventLogging.TryLogEvent($"An error occurred trying to perform the {Settings.Action} operation\n" + ex.Message, EventLogging.EVT_LOGOFFFAILED);
                    }
                    finally
                    {
                        Application.Exit();
                    }
                }
            }
            finally
            {
                Program.inTimer = 0;
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

        private static void LaunchGui()
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
