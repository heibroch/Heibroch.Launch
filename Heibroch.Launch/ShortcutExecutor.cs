using Heibroch.Launch.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace Heibroch.Launch
{
    public interface IShortcutExecutor
    {
        void AddArgument(string key, string value);
        void Execute(string shortcutKey, ILaunchShortcut launchShortcut = null);
        //void ExecuteDirect(string shortcutKey);
        IEnumerable<string> GetArgKeys(string shortcut);
        bool IsArgShortcut(string shortcut);
        Dictionary<string, string> Arguments { get; }
    }

    public class ShortcutExecutor : IShortcutExecutor
    {
        private readonly IShortcutCollection<string, ILaunchShortcut> shortcutCollection;
        private readonly IPluginLoader pluginLoader;
        
        public ShortcutExecutor(IShortcutCollection<string, ILaunchShortcut> shortcutCollection,
                                IPluginLoader pluginLoader)
        {
            this.shortcutCollection = shortcutCollection;
            this.pluginLoader = pluginLoader;
            Arguments = new Dictionary<string, string>();
        }

        public void Execute(string shortcutKey, ILaunchShortcut launchShortcut = null)
        {
            if (string.IsNullOrWhiteSpace(shortcutKey)) return;

            //Just google it
            if (!shortcutCollection.Shortcuts.ContainsKey(shortcutKey))
            {
                ProcessStart("https://www.google.com/search?q=" + shortcutKey.Replace(' ', '+'));
                return;
            }

            launchShortcut = launchShortcut ?? shortcutCollection.QueryResults[shortcutKey];
            launchShortcut.Execute(new Dictionary<string, string>(), GetFormattedString(Arguments, launchShortcut.Description));
            Arguments.Clear();
        }
        
        private string GetFormattedString(Dictionary<string, string> arguments, string description)
        {
            foreach(var argument in arguments)
            {
                var stringToReplace = $"[Arg={argument.Key}]";
                description = description.Replace(stringToReplace, argument.Value);
            }

            return description;
        }

        //public void ExecuteDirect(string command)
        //{
        //    if (string.IsNullOrWhiteSpace(command)) return;
        //    ProcessStart(command);

        //    args.Clear();
        //}

        private void ProcessStart(string command)
        {
            //// opens the folder in explorer
            //Process.Start(@"c:\temp");
            //// opens the folder in explorer
            //Process.Start("explorer.exe", @"c:\temp");
            //// throws exception
            //Process.Start(@"c:\does_not_exist");
            //// opens explorer, showing some other folder)
            //Process.Start("explorer.exe", @"c:\does_not_exist");

            try
            {
                var formattedCommand = command.StartsWith("\"") ? command.Substring(1, command.Length - 2) : command;
                //if (formattedCommand.StartsWith(Constants.CommandLineCommand))
                //{
                //    var commandLineArg = command.Remove(1, Constants.CommandLineCommand.Length);
                //    Process.Start("CMD.exe", commandLineArg);
                //    return;
                //}
                //if (formattedCommand.StartsWith(Constants.RemoteCommand))
                //{
                //    var commandLineArg = formattedCommand.Remove(0, Constants.RemoteCommand.Length);
                //    Process.Start("mstsc.exe", "/v:" + commandLineArg);
                //    return;
                //}

                Process.Start(formattedCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Failed to execute: {command}");
            }
        }

        public IEnumerable<string> GetArgKeys(string shortcut)
        {
            var argumentKey = "[Arg=";
            var shortcutLength = shortcut.Length;
            var argumentValues = new List<string>();
            for (int i = 0; i < shortcutLength; i++)
            {
                if (shortcut.Length <= i + argumentKey.Length) break;
                if (!shortcut.StartsWith(argumentKey)) continue;

                var currentFullString = shortcut.Substring(i, shortcutLength - i);
                if (!currentFullString.Contains('[')) continue;

                var nextIndexOfArgumentKey = currentFullString.IndexOf(argumentKey[0]);
                currentFullString = currentFullString.Substring(nextIndexOfArgumentKey, currentFullString.Length - nextIndexOfArgumentKey);
                if (!currentFullString.Contains(']')) continue;

                var fullArgumentKeyAndValue = currentFullString.Substring(0, currentFullString.IndexOf(']') + 1);                                
                if (!fullArgumentKeyAndValue.StartsWith("[") || !fullArgumentKeyAndValue.EndsWith("]")) continue;

                var argumentKeyAndValue = fullArgumentKeyAndValue.Substring(1, fullArgumentKeyAndValue.Length - 2);
                if (!argumentKeyAndValue.Contains('=')) continue;

                var argumentValue = argumentKeyAndValue.Split('=')[1];
                argumentValues.Add(argumentValue);

                i += fullArgumentKeyAndValue.Length - 1;
            }
            return argumentValues;
        }

        public void AddArgument(string key, string value) => Arguments[key] = value;

        public bool IsArgShortcut(string shortcut) => GetArgKeys(shortcut).Count() > 0;

        public Dictionary<string, string> Arguments { get; }
    }
}
