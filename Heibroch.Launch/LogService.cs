using Heibroch.Common;
using Heibroch.Launch.Events;
using System.Diagnostics;

namespace Heibroch.Launch
{
    public interface ILogService
    {

    }

    internal class LogService : ILogService
    {
        private readonly IEventBus eventBus;

        public LogService(IEventBus eventBus)
        {
            this.eventBus = eventBus;

            this.eventBus.Subscribe<LogEntryPublished>(OnLogEntryPublished);
        }

        private void OnLogEntryPublished(LogEntryPublished obj)
        {
            try
            {
                EventLog.WriteEntry(obj.Title, obj.Message, obj.EventLogEntryType);
            }
            catch
            {
                //Suppress
            }
        }
    }
}
