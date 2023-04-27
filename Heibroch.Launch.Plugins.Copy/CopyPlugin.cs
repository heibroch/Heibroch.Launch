using Heibroch.Launch.Interfaces;
using System.Windows;

namespace Heibroch.Launch.Plugins.Copy
{
    public class CopyPlugin : ILaunchPlugin
    {
        public CopyPlugin() { }

        public string ShortcutFilter => "[Copy]";

        public void OnProgramLoad() { }

        public void OnShortcutLoad() { }

        public void OnShortcutLaunched(ILaunchShortcut shortcut) { }

        public void OnProgramLoaded() { }

        public void OnShortcutsLoaded() { }

        private void ExecuteShortcut(string title, string description)
        {
            //var arg = description.Remove(0, ShortcutFilter.Length);

            //If it's a path to a file, then copy it to the clipboard
            //if (File.Exists(arg))
            //{
            //    var stringCollection = new StringCollection();
            //    stringCollection.Add(arg);
            //    Clipboard.SetText(description);
            //    return;
            //}

            TextCopy.ClipboardService.SetText(description.Remove(0, ShortcutFilter.Length));            
        }

        public ILaunchShortcut CreateShortcut(string title, string description) => new CopyShortcut(ExecuteShortcut, title, description);

        public void OnLoadShortcut(string shortcut) => throw new NotImplementedException();
    }
}
