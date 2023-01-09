using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;

namespace Reload
{
    public class ReloadPlugin : ILaunchPlugin
    {
        private ReloadShortcut reloadShortcut;
        private IInternalMessageBus internalMessageBus;

        public ReloadPlugin(IInternalMessageBus internalMessageBus)
        {
            this.reloadShortcut = new ReloadShortcut(internalMessageBus);
            this.internalMessageBus = internalMessageBus;
            this.internalMessageBus.Subscribe<ShortcutsLoadingStarted>(OnShortcutsLoadingStarted);
        }

        private void OnShortcutsLoadingStarted(ShortcutsLoadingStarted obj) => internalMessageBus.Publish(new ShortcutAddingStarted(reloadShortcut));

        public string? ShortcutFilter => null;

        public ILaunchShortcut? CreateShortcut(string title, string description) => null;
    }
}
