namespace Heibroch.Launch.Interfaces
{
    public interface ISettingsRepository
    {
        /// <summary>
        /// Loads in the persisted settings
        /// </summary>
        void Load(string directoryPath);

        /// <summary>
        /// Saves the settings to file
        /// </summary>
        /// <param name="modifier1">System.Windows.Input.ModifierKeys</param>
        /// <param name="modifier2">System.Windows.Input.ModifierKeys</param>
        /// <param name="key">System.Windows.Forms.Keys</param>
        /// <param name="useStickySearch"></param>
        /// <param name="showMostUsed"></param>
        /// <param name="filePath"></param> 
        void Save(string modifier1, string modifier2, string key, bool useStickySearch, bool showMostUsed, string filePath = null);

        /// <summary>
        /// 
        /// </summary>
        SortedList<string, string> Settings { get; }
    }
}