using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Events
{
    public class ShortcutAddingCompleted : IInternalMessage
    {
        public ShortcutAddingCompleted(ILaunchShortcut launchShortcut)
        {
            LaunchShortcut = launchShortcut;
        }

        public bool LogPublish { get; set; } = true;

        public ILaunchShortcut LaunchShortcut { get; }
    }
}
