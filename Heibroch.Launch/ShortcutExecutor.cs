using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows;

namespace Heibroch.Launch
{
    public class ShortcutExecutor : IShortcutExecutor
    {
        private readonly IShortcutCollection<string, ILaunchShortcut> shortcutCollection;
        private readonly IInternalMessageBus internalMessageBus;

        public ShortcutExecutor(IShortcutCollection<string, ILaunchShortcut> shortcutCollection,
                                IInternalMessageBus internalMessageBus)
        {
            this.shortcutCollection = shortcutCollection;
            this.internalMessageBus = internalMessageBus;

            Arguments = new Dictionary<string, string>();

            internalMessageBus.Subscribe<ShortcutExecutingStarted>(OnShortcutExecutingStarted);
            internalMessageBus.Subscribe<ShortcutArgumentFilled>(OnShortcutArgumentFilled);
        }

        private void OnShortcutArgumentFilled(ShortcutArgumentFilled obj) => AddArgument(obj.Key, obj.Value);

        private void OnShortcutExecutingStarted(ShortcutExecutingStarted obj)
        {
            Execute(obj.ShortcutKey, obj.LaunchShortcut);
            internalMessageBus.Publish(new ShortcutExecutingCompleted() { ShortcutKey = obj.ShortcutKey, LaunchShortcut = obj.LaunchShortcut });
        }

        private void Execute(string shortcutKey, ILaunchShortcut? launchShortcut = null)
        {
            if (string.IsNullOrWhiteSpace(shortcutKey)) return;

            //Just google it
            if (!shortcutCollection.Shortcuts.ContainsKey(shortcutKey))
            {
                ProcessStart("https://www.google.com/search?q=" + HttpUtility.UrlEncode(shortcutKey));
                return;
            }

            launchShortcut = launchShortcut ?? shortcutCollection.QueryResults.First(x => x.Key == shortcutKey).Value;
            launchShortcut.Execute(new Dictionary<string, string>(), GetFormattedString(Arguments, launchShortcut.Description));
            Arguments.Clear();
        }

        private string GetFormattedString(Dictionary<string, string> arguments, string description)
        {
            foreach (var argument in arguments)
            {
                var stringToReplace = $"[Arg={argument.Key}]";
                description = description.Replace(stringToReplace, argument.Value);
            }

            return description;
        }

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

                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = formattedCommand,
                    UseShellExecute = true
                };
                
                Process.Start(processStartInfo);
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

            if (!shortcut.Contains(argumentKey)) return argumentValues;

            for (int i = 0; i < shortcutLength; i++)
            {
                if (shortcut.Length <= i + argumentKey.Length) break;

                var currentFullString = shortcut.Substring(i, shortcutLength - i);
                if (!currentFullString.StartsWith(argumentKey) || !currentFullString.Contains(']')) continue;

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
