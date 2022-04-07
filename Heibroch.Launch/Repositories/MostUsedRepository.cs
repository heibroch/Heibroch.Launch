using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heibroch.Launch
{
    public class MostUsedRepository : IMostUsedRepository
    {
        private readonly IInternalMessageBus internalMessageBus;
        private readonly IPathRepository pathRepository;
        private readonly ISettingsRepository settingsRepository;

        public MostUsedRepository(IInternalMessageBus internalMessageBus, IPathRepository pathRepository, ISettingsRepository settingsRepository)
        {
            this.internalMessageBus = internalMessageBus;
            this.pathRepository = pathRepository;
            this.settingsRepository = settingsRepository;

            this.internalMessageBus.Subscribe<ShortcutExecutingCompleted>(OnShortcutExecutingCompleted);
            this.internalMessageBus.Subscribe<ShortcutsLoadingCompleted>(OnShortcutsLoadingCompleted);
        }

        public List<Tuple<string, int>> ShortcutUseCounts { get; set; } = new List<Tuple<string, int>>();

        private void OnShortcutsLoadingCompleted(ShortcutsLoadingCompleted obj)
        {
            if (!IsMostUsedEnabled) return;

            ShortcutUseCounts.Clear();

            var filePath = FilePath;

            //If no settings file exists, then create one
            if (!File.Exists(filePath))
            {
                var fileStream = File.Create(filePath);
                fileStream.Flush();
                fileStream.Dispose();
            }

            //Parse all most used lines
            var mostUsedLines = File.ReadAllLines(filePath);
            foreach (var mostUsedLine in mostUsedLines)
            {
                var mostUsedLineValues = mostUsedLine.Split(';');
                var mostUsedCount = Convert.ToInt32(mostUsedLineValues[0]);
                var mostUsedKey = mostUsedLineValues[1];
                ShortcutUseCounts.Add(new Tuple<string, int>(mostUsedKey, mostUsedCount));                
            }
            
            //Remove entries that don't have matches
            foreach (var mostUsedShortcut in ShortcutUseCounts)
            {
                var shortcutMatch = obj.Shortcuts.FirstOrDefault(x => x.Key == mostUsedShortcut.Item1);
                if (shortcutMatch.Key == default && shortcutMatch.Value == default)
                    ShortcutUseCounts.Remove(mostUsedShortcut);
            }

            //Remove lowest entries that are above the 10 count
            if (ShortcutUseCounts.Count > 10)
            {
                var remaining = ShortcutUseCounts.Skip(10).ToList();
                foreach (var shortcut in remaining)
                    ShortcutUseCounts.Remove(shortcut);
            }
        }

        private void OnShortcutExecutingCompleted(ShortcutExecutingCompleted obj)
        {
            if (!IsMostUsedEnabled) return;

            var mostUsedShortcutMatch = ShortcutUseCounts.FirstOrDefault(x => x.Item1 == obj.ShortcutKey);
            if (mostUsedShortcutMatch == null)
                ShortcutUseCounts.Add(new Tuple<string, int>(obj.ShortcutKey, 1));
            else
            {
                //Remove item
                ShortcutUseCounts.Remove(mostUsedShortcutMatch);

                //Insert updated item
                ShortcutUseCounts.Add(new Tuple<string, int>(mostUsedShortcutMatch.Item1, mostUsedShortcutMatch.Item2 + 1));
            }

            File.WriteAllText(FilePath, string.Join("\r\n", ShortcutUseCounts.Select(x => $"{x.Item2};{x.Item1}")));
        }

        private string FilePath => $"{pathRepository.AppSettingsDirectory}{Constants.FileNames.MostUsed}{Constants.FileExtensions.MostUsedFileExtension}";

        private bool IsMostUsedEnabled => bool.Parse(settingsRepository.Settings.First(x => x.Key == Constants.SettingNames.ShowMostUsed).Value);
    }
}
