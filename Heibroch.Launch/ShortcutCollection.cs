using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heibroch.Launch
{
    public class ShortcutCollection : IShortcutCollection<string, ILaunchShortcut>
    {
        private IStringSearchEngine<ILaunchShortcut> stringSearchEngine;
        private readonly ISettingsRepository settingsRepository;
        private IPluginLoader pluginLoader;
        private readonly IInternalMessageBus internalMessageBus;

        public ShortcutCollection(IPluginLoader pluginLoader,
                                  IInternalMessageBus internalMessageBus,
                                  IStringSearchEngine<ILaunchShortcut> stringSearchEngine,
                                  ISettingsRepository settingsRepository)
        {
            this.pluginLoader = pluginLoader;
            this.internalMessageBus = internalMessageBus;
            this.stringSearchEngine = stringSearchEngine;
            this.settingsRepository = settingsRepository;

            internalMessageBus.Subscribe<ShortcutAddingStarted>(OnShortcutAddingStarted);
            internalMessageBus.Subscribe<ShortcutsLoadingStarted>(OnShortcutsLoadingStarted);
            internalMessageBus.Subscribe<ShortcutsFilteredStarted>(OnShortcutsFilteredStarted);
        }

        private void OnShortcutsFilteredStarted(ShortcutsFilteredStarted obj)
        {
            Filter(obj.NewFilter);
            internalMessageBus.Publish(new ShortcutsFilteredCompleted(obj.OldFilter, obj.NewFilter));
        }

        private void OnShortcutsLoadingStarted(ShortcutsLoadingStarted obj) => Load(Constants.RootPath, true);

        private void OnShortcutAddingStarted(ShortcutAddingStarted obj)
        {
            Shortcuts.Add(obj.LaunchShortcut.Title, obj.LaunchShortcut);
            internalMessageBus.Publish(new ShortcutAddingCompleted(obj.LaunchShortcut));
        }

        public Dictionary<string, ILaunchShortcut> Shortcuts { get; set; } = new Dictionary<string, ILaunchShortcut>();

        public List<KeyValuePair<string, ILaunchShortcut>> QueryResults { get; private set; } = new List<KeyValuePair<string, ILaunchShortcut>>();

        public string CurrentQuery { get; private set; }

        private void Load(string directoryPath, bool clear)
        {
            try
            {
                if (clear)
                    Shortcuts.Clear();

                //If the directory doesn't exist, then create it
                if (!Directory.Exists(directoryPath) && clear)
                {
                    if (directoryPath == Constants.RootPath)
                        Directory.CreateDirectory(Constants.RootPath);
                }

                //Create default file if it doesn't exist
                var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(Constants.FileExtensions.ShortcutFileExtension)).ToList();
                if (!files.Any())
                {
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

                //If it's a full reload, publish it has completed
                if (clear)
                    internalMessageBus.Publish(new ShortcutsLoadingCompleted() { Shortcuts = this.Shortcuts });
            }
            catch (Exception exception)
            {
                internalMessageBus.Publish(new ShortcutsLoadingFailed(exception));
            }           
        }

        public void Remove(string shortcutName)
        {
            if (!Shortcuts.ContainsKey(shortcutName)) return;
            Shortcuts.Remove(shortcutName);
        }

        private void Filter(string searchString)
        {
            settingsRepository.Settings.TryGetValue(Constants.SettingNames.UseStickySearch, out var useStickySearch);
            QueryResults = stringSearchEngine.Search(searchString, Shortcuts, bool.Parse(useStickySearch));
            CurrentQuery = searchString;
        }
    }
}
