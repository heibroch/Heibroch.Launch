using Heibroch.Infrastructure.Interfaces.MessageBus;
using System.Diagnostics;

namespace Heibroch.Launch.Events
{
    public class LogEntryPublished : IInternalEvent
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

        public bool LogEvent { get; set; } = true;
    }
}
