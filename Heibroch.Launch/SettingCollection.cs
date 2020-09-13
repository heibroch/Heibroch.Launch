using Heibroch.Common;
using Heibroch.Launch.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace Heibroch.Launch
{
    public interface ISettingCollection
    {
        void Load(string directoryPath = null, bool clear = true);

        void Save(ModifierKeys modifier1, ModifierKeys modifier2, Keys key, string filePath = null);

        SortedList<string, string> Settings { get; }
    }

    public class SettingCollection : ISettingCollection
    {
        private readonly IEventBus eventBus;

        public SettingCollection(IEventBus eventBus) => this.eventBus = eventBus;

        public SortedList<string, string> Settings { get; private set; } = new SortedList<string, string>();

        public void Load(string directoryPath = null, bool clear = true)
        {
            if (clear)
                Settings.Clear();

            directoryPath = directoryPath ?? $"{Constants.RootPath}";

            if (!Directory.Exists(directoryPath))
            {
                eventBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"Could not locate directory\r\n{Environment.StackTrace}", EventLogEntryType.Information));

                if (directoryPath == Constants.RootPath)
                    Directory.CreateDirectory(Constants.RootPath);

                return;
            }

            //Add files
            var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(Constants.SettingFileExtension)).ToList();
            if (!files.Any())
            {
                eventBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"No files found in directory\r\n{Environment.StackTrace}", EventLogEntryType.Error));
                var defaultFilePath = directoryPath + "Settings" + Constants.SettingFileExtension;

                var fileStream = File.Create(defaultFilePath);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.Write("Modifier1;Control\r\n" +
                                   "Modifier2;Shift\r\n" +
                                   "Key;Space");

                streamWriter.Flush();
                streamWriter.Dispose();

                files.Add(defaultFilePath);
            }

            var file = files.First();
            var lines = File.ReadAllLines(file);
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

        public void Save(ModifierKeys modifier1, ModifierKeys modifier2, Keys key, string filePath = null)
        {
            filePath = filePath ?? $"{Constants.RootPath}{Constants.SettingFileName}{Constants.SettingFileExtension}";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Modifier1;{modifier1}");
            stringBuilder.AppendLine($"Modifier2;{modifier2}");
            stringBuilder.AppendLine($"Key;{key}");

            File.WriteAllText(filePath, stringBuilder.ToString());
        }
    }
}
