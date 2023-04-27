namespace Heibroch.Launch.Interfaces
{
    public interface ISettingsRepository
    {
        /// <summary>
        /// Loads in the persisted settings
        /// </summary>
        void Load();

        /// <summary>
        /// Saves the settings to file
        /// </summary>
        void Save(string modifier1, string modifier2, string key, bool useStickySearch, bool showMostUsed, bool logInfo, bool logWarnings, bool logErrors);

        /// <summary>
        /// 
        /// </summary>
        SortedList<string, string> Settings { get; }
    }
}