namespace Lithnet.idlelogoff
{
    using System;
    using System.Diagnostics;

    public class EventLogging
    {
        internal const int EvtRegisteredsource = 1;
        internal const int EvtTimerstarted = 2;
        internal const int EvtLogoffevent = 3;
        internal const int EvtTimerintervalchanged = 4;
        internal const int EvtLogofffailed = 5;
        internal const int EvtRestartfailed = 6;
        internal const int EvtSessioninuse = 7;
        internal static string EvtSource = "Lithnet.idlelogoff";
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
            if (!EventLog.SourceExists(EventLogging.EvtSource))
            {
                EventLog.CreateEventSource(EventLogging.EvtSource, "Application");
                TryLogEvent("The event log source was registered", EventLogging.EvtRegisteredsource);
            }
        }

        public static bool IsEventSourceRegistered()
        {
            try
            {
                if (EventLog.SourceExists(EventLogging.EvtSource))
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
                EventLog.WriteEntry(EventLogging.EvtSource, eventText, entryType, eventID);
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
