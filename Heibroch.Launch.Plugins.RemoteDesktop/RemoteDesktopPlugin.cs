using Heibroch.Launch.Interfaces;
using Heibroch.Launch.Plugins.RemoteDesktop;
using System.Diagnostics;

namespace Reload
{
    public class RemoteDesktopPlugin : ILaunchPlugin
    {
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;

        public RemoteDesktopPlugin(IShortcutCollection<string, ILaunchShortcut> shortcutCollection) => this.shortcutCollection = shortcutCollection;

        public string ShortcutFilter => "[Remote]";

        private void ExecuteShortcut(string title, string description)
        {
            var commandLineArg = description.Remove(0, ShortcutFilter.Length);
            Process.Start("mstsc.exe", "/v:" + commandLineArg);
        }

        public ILaunchShortcut CreateShortcut(string title, string description) => new RemoteDesktopShortcut(ExecuteShortcut, title, description);
    }
}
