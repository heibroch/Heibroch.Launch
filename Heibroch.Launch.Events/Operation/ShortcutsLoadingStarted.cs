using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ShortcutsLoadingStarted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;
    }
}
