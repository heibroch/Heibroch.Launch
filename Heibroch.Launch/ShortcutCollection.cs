using Heibroch.Common;
using Heibroch.Launch.Events;
using Heibroch.Launch.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Heibroch.Launch
{
    public class ShortcutCollection : IShortcutCollection<string, ILaunchShortcut>
    {
        private IStringSearchEngine<ILaunchShortcut> stringSearchEngine;
        private IPluginLoader pluginLoader;
        private readonly IEventBus eventBus;

        public ShortcutCollection(IPluginLoader pluginLoader,
                                  IEventBus eventBus)
        {
            this.pluginLoader = pluginLoader;
            this.eventBus = eventBus;

            stringSearchEngine = new StringSearchEngine<ILaunchShortcut>();
        }
        
        public Dictionary<string, ILaunchShortcut> Shortcuts { get; } = new Dictionary<string, ILaunchShortcut>();

        public SortedList<string, ILaunchShortcut> QueryResults { get; private set; } = new SortedList<string, ILaunchShortcut>();

        public string CurrentQuery { get; private set; }
        
        public void Load(string directoryPath = null, bool clear = true)
        {
            if (clear)
                Shortcuts.Clear();

            directoryPath = directoryPath ?? $"{Constants.RootPath}";

            if (!Directory.Exists(directoryPath))
            {
                eventBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"Could not locate directory\r\n{Environment.StackTrace}", EventLogEntryType.Information));

                if (directoryPath == Constants.RootPath)
                    Directory.CreateDirectory(Constants.RootPath);

                return;
            }

            //Create default file if it doesn't exist
            var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(Constants.ShortcutFileExtension)).ToList();
            if (!files.Any())
            {
                eventBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"No files found in directory\r\n{Environment.StackTrace}", EventLogEntryType.Error));
                var defaultShortcutFilePath = directoryPath + "\\MyShortcuts.hscut";
                
                var fileStream = File.Create(defaultShortcutFilePath);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.Write("//To add a shortcut to a file or whatever\r\n" +
                                   "MySearchSite;\"https://duckduckgo.com/" + Environment.NewLine + 
                                   $"{Constants.SearchLocation};\\\\sys.dom\\dfs\\v\\users\\DWAP\\HeibrochLaunch\\");

                streamWriter.Flush();
                streamWriter.Dispose();

                files.Add(defaultShortcutFilePath);
            }
            
            foreach (var file in files)
            {
                //Add shortcuts in case of modification needed
                var fileInfo = new FileInfo(file);
                Shortcuts.Add(fileInfo.Name, new LaunchShortcut(fileInfo.Name, file));

                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.StartsWith("//")) continue;
                    var values = line.Split(';');

                    if (values[0] == Constants.SearchLocation)
                    {
                        Load(values[1], false);
                        continue;
                    }

                    var plugin = pluginLoader.Plugins.Where(x => !string.IsNullOrWhiteSpace(x.ShortcutFilter)).FirstOrDefault(x => values[1].ToLower().StartsWith(x.ShortcutFilter.ToLower()));
                                        
                    //Update or add
                    Shortcuts[values[0]] = plugin?.CreateShortcut(values[0], values[1]) ?? new LaunchShortcut(values[0], values[1]);
                }
            }
        }

        public void Remove(string shortcutName)
        {
            if (!Shortcuts.ContainsKey(shortcutName)) return;
            Shortcuts.Remove(shortcutName);
        }
        
        public void Filter(string searchString)
        {
            QueryResults = stringSearchEngine.Search(searchString, Shortcuts);
            CurrentQuery = searchString;
        }

        public void Add(ILaunchShortcut launchShortcut) => Shortcuts.Add(launchShortcut.Title, launchShortcut);
    }
}
