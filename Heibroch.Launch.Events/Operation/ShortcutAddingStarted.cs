using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Events
{
    public class ShortcutAddingStarted : IInternalMessage
    {
        public ShortcutAddingStarted(ILaunchShortcut launchShortcut)
        {
            LaunchShortcut = launchShortcut;
        }

        public bool LogPublish { get; set; } = true;

        public ILaunchShortcut LaunchShortcut { get; }
    }
}
