using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Plugin;

namespace Heibroch.Launch.Events
{
    public class ShortcutExecutingStarted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;

        public string ShortcutKey { get; set; }
        
        public ILaunchShortcut LaunchShortcut { get; set; }
    }
}
