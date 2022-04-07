﻿using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                                       $"{Constants.SettingNames.UseStickySearch};true " +
                                       $"{Constants.SettingNames.ShowMostUsed};false");

                    streamWriter.Flush();
                    streamWriter.Dispose();
                }

                var lines = File.ReadAllLines(filePath).ToList();

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
        public void Save(string modifier1, string modifier2, string key, bool useStickySearch, bool showMostUsed)
        {
            var filePath = $"{pathRepository.AppSettingsDirectory}{Constants.FileNames.Settings}{Constants.FileExtensions.SettingFileExtension}";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier1};{modifier1}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Modifier2};{modifier2}");
            stringBuilder.AppendLine($"{Constants.SettingNames.Key};{key}");
            stringBuilder.AppendLine($"{Constants.SettingNames.UseStickySearch};{useStickySearch}");
            stringBuilder.AppendLine($"{Constants.SettingNames.ShowMostUsed};{showMostUsed}");

            File.WriteAllText(filePath, stringBuilder.ToString());

            Load();
        }
    }
}