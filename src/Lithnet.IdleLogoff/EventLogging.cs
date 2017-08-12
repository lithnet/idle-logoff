namespace Lithnet.idlelogoff
{
    using System;
    using System.Diagnostics;

    public class EventLogging
    {
        internal const int EVT_REGISTEREDSOURCE = 1;
        internal const int EVT_TIMERSTARTED = 2;
        internal const int EVT_LOGOFFEVENT = 3;
        internal const int EVT_TIMERINTERVALCHANGED = 4;
        internal const int EVT_LOGOFFFAILED = 5;
        internal const int EVT_RESTARTFAILED = 6;
        internal static string evtSource = "Lithnet.idlelogoff";
        internal static bool LogEnabled = true;

        public static void InitEventLog()
        {
            if (!IsEventSourceRegistered())
            {
                if (!TryRegisterEventSource())
                {
                    LogEnabled = false;
                }
            }
        }

        public static void RegisterEventSource()
        {
            if (!EventLog.SourceExists(evtSource))
            {
                EventLog.CreateEventSource(evtSource, "Application");
                TryLogEvent("The event log source was registered", EVT_REGISTEREDSOURCE);
            }
        }

        public static bool IsEventSourceRegistered()
        {
            try
            {
                if (EventLog.SourceExists(evtSource))
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;

        }

        public static bool TryRegisterEventSource()
        {
            try
            {
                if (!IsEventSourceRegistered())
                {
                    RegisterEventSource();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void LogEvent(string eventText, int eventId)
        {
            LogEvent(eventText, eventId, EventLogEntryType.Information);
        }

        public static void LogEvent(string eventText, int eventID, EventLogEntryType entryType)
        {
            Trace.WriteLine($"{entryType}: {eventID}: {eventText}");
            if (LogEnabled)
            {
                EventLog.WriteEntry(evtSource, eventText, entryType, eventID);
            }
        }

        public static bool TryLogEvent(string eventText, int eventID, EventLogEntryType entryType)
        {
            try
            {
                LogEvent(eventText, eventID, entryType);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryLogEvent(string eventText, int eventID)
        {
            return TryLogEvent(eventText, eventID, EventLogEntryType.Information);
        }
    }
}
