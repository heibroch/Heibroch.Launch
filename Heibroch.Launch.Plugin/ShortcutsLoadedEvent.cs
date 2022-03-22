using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Plugin
{
    public class ShortcutsLoadedEvent : IInternalMessage
    {
        public bool LogPublish { get; set; }
    }
}
