using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Plugin
{
    public class ShortcutsLoadedEvent : IInternalEvent
    {
        public bool LogEvent { get; set; }
    }
}
