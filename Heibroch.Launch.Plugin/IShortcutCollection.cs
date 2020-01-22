using System.Collections.Generic;

namespace Heibroch.Launch.Plugin
{
    public interface IShortcutCollection<TKey, TShortcut>
    {
        void Load(string directoryPath = null, bool clear = true);

        void Add(ILaunchShortcut launchShortcut);

        void Remove(TKey shortcutKey);

        void Filter(TKey searchKey);

        SortedList<TKey, ILaunchShortcut> QueryResults { get; }

        Dictionary<string, ILaunchShortcut> Shortcuts { get; }

        TKey CurrentQuery { get; }
    }
}
