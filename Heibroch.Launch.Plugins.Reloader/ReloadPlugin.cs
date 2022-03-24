using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;

namespace Reload
{
    public class ReloadPlugin : ILaunchPlugin
    {
        private ReloadShortcut reloadShortcut;
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;
        private IInternalMessageBus internalMessageBus;

        public ReloadPlugin(IShortcutCollection<string, ILaunchShortcut> shortcutCollection,
                            IInternalMessageBus internalMessageBus)
        {
            this.shortcutCollection = shortcutCollection;
            this.reloadShortcut = new ReloadShortcut(shortcutCollection);
            this.internalMessageBus = internalMessageBus;
            this.internalMessageBus.Subscribe<ShortcutsLoadingCompleted>(OnShortcutsLoadingCompleted);
        }

        private void OnShortcutsLoadingCompleted(ShortcutsLoadingCompleted obj) => shortcutCollection.Add(reloadShortcut);

        public string? ShortcutFilter => null;

        public ILaunchShortcut? CreateShortcut(string title, string description) => null;
    }
}
