using Heibroch.Launch.Interfaces;

namespace Reload
{
    public class ReloadShortcut : ILaunchShortcut
    {
        private IShortcutCollection<string, ILaunchShortcut> shortcutCollection;

        public ReloadShortcut(IShortcutCollection<string, ILaunchShortcut> shortcutCollection) => this.shortcutCollection = shortcutCollection;

        public Action<IEnumerable<KeyValuePair<string, string>>, string> Execute => (IEnumerable<KeyValuePair<string, string>> args, string formattedDescription) =>
        {
            shortcutCollection.Load();
            shortcutCollection.Add(this);
        };
        
        public string Title => "Reload";

        public string Description => "This application command will reload the collection of shortcuts";
    }
}
