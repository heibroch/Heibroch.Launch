using System.Collections.Generic;

namespace Heibroch.Launch.Interfaces
{
    public interface IShortcutCollection<TKey, TShortcut>
    {
        void Remove(TKey shortcutKey);

        void Filter(TKey searchKey);

        List<KeyValuePair<TKey, ILaunchShortcut>> QueryResults { get; }

        Dictionary<string, ILaunchShortcut> Shortcuts { get; set; }

        TKey CurrentQuery { get; }
    }
}
