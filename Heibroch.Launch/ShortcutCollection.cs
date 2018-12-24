using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Heibroch.Launch
{
    public interface IShortcutCollection
    {
        void Load(string filePath = null);

        void Save(string filePath = null);

        void Add(string shortcutName, string shortcutPath);

        void Remove(string shortcutName);

        void Filter(string searchString);

        SortedList<string, string> QueryResults { get; }

        string CurrentQuery { get; }
    }

    public class ShortcutCollection : IShortcutCollection
    {
        public Dictionary<string, string> Shortcuts = new Dictionary<string, string>();
        
        public void Load(string filePath = null)
        {
            Shortcuts.Clear();
            filePath = filePath ?? $"{Defaults.ShortcutPath}\\{Defaults.ShortcutFileName}";

            if (!File.Exists(filePath))
            {
                Save(filePath);
            }

            var lines = File.ReadAllLines(filePath);
            
            foreach (var line in lines)
            {
                var values = line.Split(';');
                Shortcuts.Add(values[0], values[1]);
            }
        }

        public void Save(string filePath = null)
        {
            filePath = filePath ?? $"{Defaults.ShortcutPath}\\{Defaults.ShortcutFileName}";
            
            var stringBuilder = new StringBuilder();
            foreach (var shortcut in Shortcuts)
            {
                stringBuilder.AppendLine($"{shortcut.Key};{shortcut.Value}");
            }

            File.WriteAllText(filePath, stringBuilder.ToString());
        }

        public void Add(string shortcutName, string shortcutPath)
        {
            if (Shortcuts.ContainsKey(shortcutName))
                Shortcuts[shortcutName] = shortcutPath;
            else
                Shortcuts.Add(shortcutName, shortcutPath);
        }

        public void Remove(string shortcutName)
        {
            if (!Shortcuts.ContainsKey(shortcutName)) return;
            Shortcuts.Remove(shortcutName);
        }

        public void Filter(string searchString)
        {
            var originalSearchString = searchString;
            searchString = searchString.ToLower();

            //It should be an empty list if no query string
            if (string.IsNullOrWhiteSpace(searchString))
            {
                QueryResults = new SortedList<string, string>();
            }
            //Redo the list
            else
            {
                QueryResults = new SortedList<string, string>(Shortcuts
                    .Where(x => x.Key.ToLower().StartsWith(searchString) || x.Key.ToLower().Contains(searchString))
                    .ToDictionary(z => z.Key, y => y.Value));
            }

            CurrentQuery = originalSearchString;
        }

        public string CurrentQuery { get; private set; }

        public SortedList<string, string> QueryResults { get; private set; } = new SortedList<string, string>();
    }
}
