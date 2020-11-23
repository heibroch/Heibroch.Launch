using Heibroch.Common;
using Heibroch.Launch.Plugin;

namespace Reload
{
    public class ReloadPlugin : ILaunchPlugin
    {
        private ReloadShortcut reloadShortcut;
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;
        private IEventBus eventBus;

        public ReloadPlugin(IShortcutCollection<string, ILaunchShortcut> shortcutCollection,
                            IEventBus eventBus)
        {
            this.shortcutCollection = shortcutCollection;
            this.reloadShortcut = new ReloadShortcut(shortcutCollection);
            this.eventBus = eventBus;
            this.eventBus.Subscribe<ShortcutsLoadedEvent>(OnShortcutsLoaded);
        }

        public string ShortcutFilter => null;

        public ILaunchShortcut CreateShortcut(string title, string description) => null;

        private void OnShortcutsLoaded(ShortcutsLoadedEvent shortcutsLoadedEvent) => shortcutCollection.Add(reloadShortcut);
    }
}
