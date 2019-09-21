using System;
using System.Diagnostics;
using System.Windows;

namespace Heibroch.Launch
{
    public interface IShortcutExecutor
    {
        void Execute(string shortcutKey);
        void ExecuteDirect(string shortcutKey);
    }

    public class ShortcutExecutor : IShortcutExecutor
    {
        private readonly IShortcutCollection shortcutCollection;

        public ShortcutExecutor(IShortcutCollection shortcutCollection)
        {
            this.shortcutCollection = shortcutCollection;
        }

        public void Execute(string shortcutKey)
        {
            if (string.IsNullOrWhiteSpace(shortcutKey)) return;

            //Just google it
            if (!shortcutCollection.QueryResults.ContainsKey(shortcutKey))
            {
                ProcessStart("https://www.google.com/search?q=" + shortcutKey.Replace(' ', '+'));
                return;
            }
            
            if (shortcutKey.Equals(Constants.ReloadCommand))
            {
                shortcutCollection.Load();
                return;
            }

            var command = shortcutCollection.QueryResults[shortcutKey];
            ProcessStart(command);
        }
        
        public void ExecuteDirect(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return;
            ProcessStart(command);
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
                if (formattedCommand.StartsWith(Constants.CommandLineCommand))
                {
                    var commandLineArg = command.Remove(1, Constants.CommandLineCommand.Length);
                    Process.Start("CMD.exe", commandLineArg);
                    return;
                }
                if (formattedCommand.StartsWith(Constants.RemoteCommand))
                {
                    var commandLineArg = formattedCommand.Remove(0, Constants.RemoteCommand.Length);
                    Process.Start("mstsc.exe", "/v:" + commandLineArg);
                    return;
                }

                Process.Start(formattedCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Failed to execute: {command}");
            }
        }
    }
}
