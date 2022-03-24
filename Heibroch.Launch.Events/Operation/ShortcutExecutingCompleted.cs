using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Events
{
    public class ShortcutExecutingCompleted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;

        public string ShortcutKey { get; set; }

        public ILaunchShortcut? LaunchShortcut { get; set; }

        public override string ToString() => $"Shortcut execution of {ShortcutKey} completed";
    }
}
