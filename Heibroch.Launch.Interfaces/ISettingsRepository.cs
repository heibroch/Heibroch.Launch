﻿using System.Collections.Generic;

namespace Heibroch.Launch.Interfaces
{
    public interface ISettingsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        void Load(string directoryPath = null, bool clear = true);

        /// <summary>
        /// Saves the settings to file
        /// </summary>
        /// <param name="modifier1">System.Windows.Input.ModifierKeys</param>
        /// <param name="modifier2">System.Windows.Input.ModifierKeys</param>
        /// <param name="key">System.Windows.Forms.Keys</param>
        /// <param name="useStickySearch"></param>
        /// <param name="filePath"></param> 
        void Save(string modifier1, string modifier2, string key, bool useStickySearch, string filePath = null);

        /// <summary>
        /// 
        /// </summary>
        SortedList<string, string> Settings { get; }
    }
}