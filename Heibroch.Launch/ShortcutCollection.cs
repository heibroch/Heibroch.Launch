using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Heibroch.Launch
{
    public interface IShortcutCollection
    {
        void Load(string directoryPath = null, bool clear = true);

        void Save(string filePath = null);

        void Add(string shortcutName, string shortcutPath);

        void Remove(string shortcutName);

        void Filter(string searchString);

        //void Execute(string shortcutKey);

        //void ExecuteDirect(string command);

        SortedList<string, string> QueryResults { get; }

        string CurrentQuery { get; }
    }

    public class ShortcutCollection : IShortcutCollection
    {
        private IStringSearchEngine stringSearchEngine;

        public ShortcutCollection()
        {
            stringSearchEngine = new StringSearchEngine();
        }
        
        private readonly Dictionary<string, string> _shortcuts = new Dictionary<string, string>();

        public SortedList<string, string> QueryResults { get; private set; } = new SortedList<string, string>();

        public string CurrentQuery { get; private set; }
        
        public void Load(string directoryPath = null, bool clear = true)
        {
            if (clear)
                _shortcuts.Clear();

            directoryPath = directoryPath ?? $"{Constants.RootPath}";

            if (!Directory.Exists(directoryPath))
            {
                EventLog.WriteEntry(Constants.ApplicationName, $"Could not locate directory\r\n{Environment.StackTrace}");

                if (directoryPath == Constants.RootPath)
                    Directory.CreateDirectory(Constants.RootPath);

                return;
            }

            //Add special shortcuts
            if (!_shortcuts.ContainsKey(Constants.ReloadCommand))
                _shortcuts.Add(Constants.ReloadCommand, "This application command will reload the collection of shortcuts");
            
            //Add files
            var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(Constants.ShortcutFileExtension)).ToList();
            if (!files.Any())
            {
                EventLog.WriteEntry(Constants.ApplicationName, $"No files found in directory\r\n{Environment.StackTrace}", EventLogEntryType.Error);
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
                _shortcuts.Add(fileInfo.Name, file);

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

                    //Update or add
                    if (_shortcuts.ContainsKey(values[0]))
                        _shortcuts[values[0]] = values[1];
                    else
                        _shortcuts.Add(values[0], values[1]);
                }
            }
        }

        public void Save(string filePath = null)
        {
            filePath = filePath ?? $"{Constants.RootPath}\\{Constants.ShortcutFileName}";
            
            var stringBuilder = new StringBuilder();
            foreach (var shortcut in _shortcuts)
            {
                stringBuilder.AppendLine($"{shortcut.Key};{shortcut.Value}");
            }

            File.WriteAllText(filePath, stringBuilder.ToString());
        }

        public void Add(string shortcutName, string shortcutPath)
        {
            if (_shortcuts.ContainsKey(shortcutName))
                _shortcuts[shortcutName] = shortcutPath;
            else
                _shortcuts.Add(shortcutName, shortcutPath);
        }

        public void Remove(string shortcutName)
        {
            if (!_shortcuts.ContainsKey(shortcutName)) return;
            _shortcuts.Remove(shortcutName);
        }
        
        public void Filter(string searchString)
        {
            QueryResults = stringSearchEngine.Search(searchString, _shortcuts);
            CurrentQuery = searchString;
        }
        
        //public void Execute(string shortcutKey)
        //{
        //    if (string.IsNullOrWhiteSpace(shortcutKey)) return;

        //    //Just google it
        //    if (!QueryResults.ContainsKey(shortcutKey))
        //    {
        //        ProcessStart("https://www.google.com/search?q=" + shortcutKey.Replace(' ', '+'));
        //        return;
        //    }

        //    var command = QueryResults[shortcutKey];

        //    if (shortcutKey.Equals(ReloadCommand))
        //    {
        //        Load();
        //        return;
        //    }

        //    ProcessStart(command);
        //}

        //public void ExecuteDirect(string command)
        //{
        //    if (string.IsNullOrWhiteSpace(command)) return;
        //    ProcessStart(command);
        //}

        //private void ProcessStart(string command)
        //{
        //    //// opens the folder in explorer
        //    //Process.Start(@"c:\temp");
        //    //// opens the folder in explorer
        //    //Process.Start("explorer.exe", @"c:\temp");
        //    //// throws exception
        //    //Process.Start(@"c:\does_not_exist");
        //    //// opens explorer, showing some other folder)
        //    //Process.Start("explorer.exe", @"c:\does_not_exist");

        //    try
        //    {
        //        var formattedCommand = command.StartsWith("\"") ? command.Substring(1, command.Length - 2) : command;
        //        if (formattedCommand.StartsWith(Constants.CommandLineCommand))
        //        {
        //            var commandLineArg = command.Remove(1, Constants.CommandLineCommand.Length);
        //            Process.Start("CMD.exe", commandLineArg);
        //            return;
        //        }
        //        if (formattedCommand.StartsWith(Constants.RemoteCommand))
        //        {
        //            var commandLineArg = formattedCommand.Remove(0, Constants.RemoteCommand.Length);
        //            Process.Start("mstsc.exe", "/v:" + commandLineArg);
        //            return;
        //        }

        //        Process.Start(formattedCommand);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        MessageBox.Show($"Failed to execute: {command}");
        //    }
        //}
    }
}
