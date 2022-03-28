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

        public void Load(string directoryPath)
        {
            try
            {
                var filePath = directoryPath + "Settings" + Constants.SettingFileExtension;
                internalMessageBus.Publish(new SettingsLoadingStarted(filePath));

                Settings.Clear();

                //Create directory if it does not exist
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                //If no settings file exists, then create one
                var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(Constants.SettingFileExtension)).ToList();
                if (!files.Any())
                {
                    var fileStream = File.Create(filePath);
                    var streamWriter = new StreamWriter(fileStream);
                    streamWriter.Write($"{Constants.SettingNames.Modifier1};Control\r\n" +
                                       $"{Constants.SettingNames.Modifier2};Shift\r\n" +
                                       $"{Constants.SettingNames.Key};Space\r\n" +
                                       $"{Constants.SettingNames.UseStickySearch};true " +
                                       $"{Constants.SettingNames.ShowMostUsed};false");

                    streamWriter.Flush();
                    streamWriter.Dispose();

                    files.Add(filePath);
                }

                var file = files.First();
                var lines = File.ReadAllLines(file).ToList();

                //Add setting in case it's missing
                if (!lines.Any(x => x.Contains(Constants.SettingNames.UseStickySearch)))
                    lines.Add($"{Constants.SettingNames.UseStickySearch};false");

                //Add setting in case it's missing
                if (!lines.Any(x => x.Contains(Constants.SettingNames.ShowMostUsed)))
                    lines.Add($"{Constants.SettingNames.ShowMostUsed};false");

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

                internalMessageBus.Publish(new SettingsLoadingCompleted(filePath));
            }
            catch (Exception exception)
            {
                internalMessageBus.Publish(new SettingsLoadingFailed(exception));
            }           
        }

        /// <summary> 
        /// </summary>
        /// <param name="modifier1">System.Windows.Input.ModifierKeys</param>
        /// <param name="modifier2">System.Windows.Input.ModifierKeys</param>
        /// <param name="key">System.Windows.Forms.Keys</param>
        /// <param name="useStickySearch"></param>
        /// <param name="showMostUsed"></param>
        /// <param name="filePath"></param>
        public void Save(string modifier1, string modifier2, string key, bool useStickySearch, bool showMostUsed, string? filePath = null)
        {
            filePath = filePath ?? $"{Constants.RootPath}{Constants.SettingFileName}{Constants.SettingFileExtension}";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier1};{modifier1}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier2};{modifier2}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Key};{key}");
            stringBuilder.AppendLine($"{Constants.SettingNames.UseStickySearch};{useStickySearch}");
            stringBuilder.AppendLine($"{Constants.SettingNames.ShowMostUsed};{showMostUsed}");

            File.WriteAllText(filePath, stringBuilder.ToString());

            Load(Constants.RootPath);
        }
    }
}
