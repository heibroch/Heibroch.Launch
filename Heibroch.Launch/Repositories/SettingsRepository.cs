using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Heibroch.Launch
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IInternalMessageBus internalMessageBus;
        private readonly IPathRepository pathRepository;

        public SettingsRepository(IInternalMessageBus internalMessageBus, IPathRepository pathRepository)
        {
            this.internalMessageBus = internalMessageBus;
            this.pathRepository = pathRepository;
        }

        public SortedList<string, string> Settings { get; } = new SortedList<string, string>();

        public void Load()
        {
            try
            {
                var filePath = pathRepository.AppSettingsDirectory + Constants.FileNames.Settings + Constants.FileExtensions.SettingFileExtension;
                internalMessageBus.Publish(new SettingsLoadingStarted(filePath));

                Settings.Clear();

                //Create directory if it does not exist
                if (!Directory.Exists(pathRepository.AppSettingsDirectory))
                    Directory.CreateDirectory(pathRepository.AppSettingsDirectory);

                //If no settings file exists, then create one
                if (!File.Exists(filePath))
                {
                    var fileStream = File.Create(filePath);
                    var streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write($"{Constants.SettingNames.Modifier1};Control\r\n" +
                                       $"{Constants.SettingNames.Modifier2};Shift\r\n" +
                                       $"{Constants.SettingNames.Key};Space\r\n" +
                                       $"{Constants.SettingNames.Theme};SpaciousDark.xaml\r\n" +
                                       $"{Constants.SettingNames.UseStickySearch};true\r\n" +
                                       $"{Constants.SettingNames.ShowMostUsed};false\r\n" +
                                       $"{Constants.SettingNames.LogInfo};false\r\n" +
                                       $"{Constants.SettingNames.LogWarnings};false\r\n" +
                                       $"{Constants.SettingNames.LogErrors};false");

                    streamWriter.Flush();
                    streamWriter.Dispose();
                }

                var lines = File.ReadAllLines(filePath).ToList();

                AddLineIfMissing(lines, Constants.SettingNames.Theme, "SpaciousDark.xaml");
                AddLineIfMissing(lines, Constants.SettingNames.UseStickySearch, "false");
                AddLineIfMissing(lines, Constants.SettingNames.ShowMostUsed, "false");
                AddLineIfMissing(lines, Constants.SettingNames.LogInfo, "false");
                AddLineIfMissing(lines, Constants.SettingNames.LogWarnings, "false");
                AddLineIfMissing(lines, Constants.SettingNames.LogErrors, "false");

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.StartsWith("//")) continue;
                    var values = line.Split(';');

                    //Update or add
                    if (Settings.ContainsKey(values[0]))
                        Settings[values[0]] = values[1];
                    else
                        Settings.Add(values[0], values[1]);
                }

                // Update theme
                string theme = Settings[Constants.SettingNames.Theme];
                if (string.IsNullOrEmpty(theme)) return;
                var uri = new Uri(Constants.ThemesPath + "\\" + theme, UriKind.Absolute);
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });

                internalMessageBus.Publish(new SettingsLoadingCompleted(filePath));
            }
            catch (Exception exception)
            {
                internalMessageBus.Publish(new SettingsLoadingFailed(exception));
            }
        }

        private void AddLineIfMissing(List<string> lines, string settingName, string settingValue)
        {
            if (lines.Any(x => x.Contains(settingName))) 
                return;

            lines.Add($"{settingName};{settingValue}");
        }

        /// <summary> 
        /// </summary>
        /// <param name="modifier1">System.Windows.Input.ModifierKeys</param>
        /// <param name="modifier2">System.Windows.Input.ModifierKeys</param>
        /// <param name="key">System.Windows.Forms.Keys</param>
        /// <param name="theme"></param>
        /// <param name="useStickySearch"></param>
        /// <param name="showMostUsed"></param>
        /// <param name="filePath"></param>
        public void Save(string modifier1, string modifier2, string key, string theme, bool useStickySearch, bool showMostUsed, bool logInfo, bool logWarnings, bool logErrors)
        {
            var filePath = $"{pathRepository.AppSettingsDirectory}{Constants.FileNames.Settings}{Constants.FileExtensions.SettingFileExtension}";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier1};{modifier1}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier2};{modifier2}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Key};{key}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Theme};{theme}");
            stringBuilder.AppendLine($"{Constants.SettingNames.UseStickySearch};{useStickySearch}");
            stringBuilder.AppendLine($"{Constants.SettingNames.ShowMostUsed};{showMostUsed}");
            stringBuilder.AppendLine($"{Constants.SettingNames.LogInfo};{logInfo}");
            stringBuilder.AppendLine($"{Constants.SettingNames.LogWarnings};{logWarnings}");
            stringBuilder.AppendLine($"{Constants.SettingNames.LogErrors};{logErrors}");

            File.WriteAllText(filePath, stringBuilder.ToString());

            Load();
        }
    }
}
