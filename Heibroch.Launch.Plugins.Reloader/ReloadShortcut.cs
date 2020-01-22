using Heibroch.Launch.Plugin;
using System;
using System.Collections.Generic;

namespace Reload
{
    public class ReloadShortcut : ILaunchShortcut
    {
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;

        public ReloadShortcut(IShortcutCollection<string, ILaunchShortcut> shortcutCollection) => this.shortcutCollection = shortcutCollection;

        public string Title => "Reload";

        public string Description => "This application command will reload the collection of shortcuts";

        public Action<IEnumerable<KeyValuePair<string, string>>, string> Execute => (IEnumerable<KeyValuePair<string, string>> args, string formattedDescription) => shortcutCollection.Load();
    }
}
