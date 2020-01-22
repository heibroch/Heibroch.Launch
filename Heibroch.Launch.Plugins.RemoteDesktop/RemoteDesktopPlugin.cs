using Heibroch.Launch.Plugin;
using Heibroch.Launch.Plugins.RemoteDesktop;
using System;
using System.Diagnostics;

namespace Reload
{
    public class RemoteDesktopPlugin : ILaunchPlugin
    {
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;

        public RemoteDesktopPlugin(IShortcutCollection<string, ILaunchShortcut> shortcutCollection) => this.shortcutCollection = shortcutCollection;

        public string ShortcutFilter => "[Remote]";

        public void OnProgramLoad() { }

        public void OnShortcutLoad() { }

        public void OnShortcutLaunched(ILaunchShortcut shortcut) { }

        public void OnProgramLoaded() { }

        public void OnShortcutsLoaded() { }

        private void ExecuteShortcut(string title, string description)
        {
            var commandLineArg = description.Remove(0, ShortcutFilter.Length);
            Process.Start("mstsc.exe", "/v:" + commandLineArg);
        }

        public ILaunchShortcut CreateShortcut(string title, string description) => new RemoteDesktopShortcut(ExecuteShortcut, title, description);

        public void OnLoadShortcut(string shortcut)
        {
            throw new NotImplementedException();
        }
    }
}
