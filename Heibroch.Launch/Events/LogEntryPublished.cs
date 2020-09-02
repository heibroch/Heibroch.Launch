using System.Diagnostics;

namespace Heibroch.Launch.Events
{
    public class LogEntryPublished
    {
        public LogEntryPublished(string title, string message, EventLogEntryType eventLogEntryType)
        {
            Title = title;
            Message = message;
            EventLogEntryType = eventLogEntryType;
        }

        public string Title { get; }
        public string Message { get; }
        public EventLogEntryType EventLogEntryType { get; }
    }
}
