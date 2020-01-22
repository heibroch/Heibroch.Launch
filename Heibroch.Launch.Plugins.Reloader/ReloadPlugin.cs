using Heibroch.Launch.Plugin;

namespace Reload
{
    public class ReloadPlugin : ILaunchPlugin
    {
        private ReloadShortcut reloadShortcut;
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;

        public ReloadPlugin(IShortcutCollection<string, ILaunchShortcut> shortcutCollection)
        {
            this.shortcutCollection = shortcutCollection;
            this.reloadShortcut = new ReloadShortcut(shortcutCollection);
        }

        public string ShortcutFilter => null;

        public ILaunchShortcut CreateShortcut(string title, string description) => null;

        public void OnLoadShortcut(string shortcut) { }

        public void OnProgramLoad() { }

        public void OnProgramLoaded() { }

        public void OnShortcutLaunched(string shortcut) { }

        public void OnShortcutLaunched(ILaunchShortcut shortcut) { }

        public void OnShortcutLoad() { }

        public void OnShortcutsLoaded() { shortcutCollection.Add(reloadShortcut); }
    }
}
