using Heibroch.Common;
using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Heibroch.Launch
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IInternalMessageBus internalMessageBus;

        public SettingsRepository(IInternalMessageBus internalMessageBus) => this.internalMessageBus = internalMessageBus;

        public SortedList<string, string> Settings { get; } = new SortedList<string, string>();

        public void Load(string directoryPath = null, bool clear = true)
        {
            if (clear)
                Settings.Clear();

            directoryPath = directoryPath ?? $"{Constants.RootPath}";

            if (!Directory.Exists(directoryPath))
            {
                internalMessageBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"Could not locate directory\r\n{Environment.StackTrace}", EventLogEntryType.Information));

                if (directoryPath == Constants.RootPath)
                    Directory.CreateDirectory(Constants.RootPath);

                return;
            }

            //Add files
            var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(Constants.SettingFileExtension)).ToList();
            if (!files.Any())
            {
                internalMessageBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"No files found in directory\r\n{Environment.StackTrace}", EventLogEntryType.Error));
                var defaultFilePath = directoryPath + "Settings" + Constants.SettingFileExtension;

                var fileStream = File.Create(defaultFilePath);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.Write($"{Constants.SettingNames.Modifier1};Control\r\n" +
                                   $"{Constants.SettingNames.Modifier2};Shift\r\n" +
                                   $"{Constants.SettingNames.Key};Space\r\n" +
                                   $"{Constants.SettingNames.UseStickySearch};true");

                streamWriter.Flush();
                streamWriter.Dispose();

                files.Add(defaultFilePath);
            }

            var file = files.First();
            var lines = File.ReadAllLines(file).ToList();

            //Add setting in case it's missing
            if (!lines.Any(x => x.Contains(Constants.SettingNames.UseStickySearch)))
                lines.Add($"{Constants.SettingNames.UseStickySearch};false");

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
        }

        /// <summary> 
        /// </summary>
        /// <param name="modifier1">System.Windows.Input.ModifierKeys</param>
        /// <param name="modifier2">System.Windows.Input.ModifierKeys</param>
        /// <param name="key">System.Windows.Forms.Keys</param>
        /// <param name="useStickySearch"></param>
        /// <param name="filePath"></param>
        public void Save(string modifier1, string modifier2, string key, bool useStickySearch, string filePath = null)
        {
            filePath = filePath ?? $"{Constants.RootPath}{Constants.SettingFileName}{Constants.SettingFileExtension}";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier1};{modifier1}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier2};{modifier2}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Key};{key}");
            stringBuilder.AppendLine($"{Constants.SettingNames.UseStickySearch};{useStickySearch}");

            File.WriteAllText(filePath, stringBuilder.ToString());

            Load();
        }
    }
}
