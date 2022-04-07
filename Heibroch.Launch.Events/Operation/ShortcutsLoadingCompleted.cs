using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Events
{
    public class ShortcutsLoadingCompleted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;

        public Dictionary<string, ILaunchShortcut> Shortcuts { get; set; }
    }
}
