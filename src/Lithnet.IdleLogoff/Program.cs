using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using lithnet.idlelogoff;
using Timer = System.Windows.Forms.Timer;

namespace Lithnet.idlelogoff
{
    public static class Program
    {
        private static uint lastTicks;

        private static readonly int idleCheckSeconds = 15;

        private static DateTime expectedIdleActionTime;
        private static DateTime expectedWarningTime;

        private static bool isIdle = false;

        private static Timer eventTimer;
        private static int inTimer;
        private static bool backgroundMode;
        private static int initialLogoffIdleInterval;
        private static LogoffWarning warningDialog = new LogoffWarning();

        [STAThread]
        public static void Main()
        {
            try
            {
                EventLogging.InitEventLog();
                Program.ValidateCommandLineArgs();

                if (Program.backgroundMode)
                {
                    Program.InitTimer();
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
            Program.initialLogoffIdleInterval = Settings.IdleLimit * 60 * 1000;

            if (Settings.Enabled)
            {
                EventLogging.TryLogEvent($"The application has started. {Settings.Action} will be performed for user {Environment.UserDomainName}\\{Environment.UserName} after being idle for {Settings.IdleLimit} minutes", EventLogging.EvtTimerstarted);
            }
            else
            {
                EventLogging.TryLogEvent($"The application has started, but is not enabled. User {Environment.UserDomainName}\\{Environment.UserName} will not be logged off automatically", EventLogging.EvtTimerstarted);
            }

            Program.eventTimer = new Timer();
            Program.eventTimer.Tick += Program.EventTimer_Tick;
            Program.eventTimer.Interval = (int)TimeSpan.FromSeconds(Program.idleCheckSeconds).TotalMilliseconds;
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
                        Program.PerformApplicationRegistration();
                        Environment.Exit(0);
                    }
                    catch
                    {
                        Environment.Exit(1);
                    }
                }
                else if (arg.Equals("/start", StringComparison.OrdinalIgnoreCase))
                {
                    Program.backgroundMode = true;
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
                Program.CheckAndSetLogoffIdleInterval();

                uint currentticks = NativeMethods.GetLastInputTime();

                if (!Settings.IgnoreDisplayRequested && NativeMethods.IsDisplayRequested())
                {
                    Trace.WriteLine("An application has requested the system display stay on");
                    Program.ResetIdleStatus(currentticks);
                    return;
                }
                else if (Program.HasInput(currentticks))
                {
                    Trace.WriteLine("Input received");
                    Program.ResetIdleStatus(currentticks);
                    return;
                }

                if (!Program.isIdle)
                {
                    Program.expectedIdleActionTime = DateTime.Now.AddMilliseconds(Settings.IdleLimitMilliseconds);
                    Trace.WriteLine($"Set expected idle action time to: {Program.expectedIdleActionTime}");
                    if (Settings.WarningPeriod > 0)
                    {
                        Program.expectedWarningTime = DateTime.Now.AddMilliseconds(Settings.WarningPeriodMilliseconds);
                        Trace.WriteLine($"Set expected warning time to: {Program.expectedWarningTime}");
                    }

                    Program.isIdle = true;
                }

                if (Settings.WarningPeriod > 0
                    && DateTime.Now >= Program.expectedWarningTime
                    && DateTime.Now < Program.expectedIdleActionTime)
                {
                    Program.ShowWarning();
                }

                if (DateTime.Now >= Program.expectedIdleActionTime)
                {
                    Program.PerformIdleAction();
                }
            }
            finally
            {
                Program.inTimer = 0;
            }
        }

        private static bool HasInput(uint currentTicks)
        {
            return currentTicks != Program.lastTicks;
        }

        private static void CheckAndSetLogoffIdleInterval()
        {
            if (Program.initialLogoffIdleInterval != Settings.IdleLimitMilliseconds)
            {
                EventLogging.TryLogEvent($"Idle timeout limit has changed. {Settings.Action} will be performed for user {Environment.UserDomainName}\\{Environment.UserName}  after {Settings.IdleLimit} minutes", EventLogging.EvtTimerintervalchanged);
                Program.initialLogoffIdleInterval = Settings.IdleLimitMilliseconds;
            }
        }

        private static void PerformIdleAction()
        {
            EventLogging.TryLogEvent($"User {Environment.UserName} has passed the idle time limit of {Settings.IdleLimit} minutes. Initiating {Settings.Action}.", EventLogging.EvtLogoffevent);

            try
            {
                Program.Stop();
                NativeMethods.LogOffUser();
            }
            catch (Exception ex)
            {
                EventLogging.TryLogEvent($"An error occurred trying to perform the {Settings.Action} operation\n" + ex.Message, EventLogging.EvtLogofffailed);
            }
            finally
            {
                Application.Exit();
            }
        }

        private static void Stop()
        {
            Program.eventTimer.Stop();
            Program.HideWarning();
        }

        private static void ResetIdleStatus(uint currentTicks)
        {
            Program.isIdle = false;
            Program.eventTimer.Interval = (int)TimeSpan.FromSeconds(Program.idleCheckSeconds).TotalMilliseconds;
            Program.lastTicks = currentTicks;
            Program.HideWarning();
        }

        private static void ShowWarning()
        {
            if (!Program.warningDialog.Visible)
            {
                Trace.WriteLine("Showing warning");
                Program.eventTimer.Interval = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
                Program.warningDialog.LogoffDateTime = Program.expectedIdleActionTime;
                Program.warningDialog.Show();
                Program.warningDialog.BringToFront();
                Program.warningDialog.Focus();
                Program.warningDialog.TopMost = true;
            }
        }

        private static void HideWarning()
        {
            if (Program.warningDialog.Visible)
            {
                Trace.WriteLine("Hiding warning window");
                Program.warningDialog.Hide();
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
                Application.Run(new FrmSettings());
            }
            else
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    if (AdminCheck.TryRestartElevated(out bool usercanceled))
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
