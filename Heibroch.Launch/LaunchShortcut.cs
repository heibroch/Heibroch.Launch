﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch
{
    public class LaunchShortcut : ILaunchShortcut
    {
        public LaunchShortcut(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public Action<IEnumerable<KeyValuePair<string, string>>, string> Execute => (IEnumerable<KeyValuePair<string, string>> args, string formattedDescription) =>
        {
            try
            {
                var formattedCommand = formattedDescription.StartsWith("\"") ? formattedDescription.Substring(1, formattedDescription.Length - 2) : formattedDescription;

                var process = new Process();
                process.StartInfo = new ProcessStartInfo();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = formattedCommand;
                process.Start();
            }
            catch (Exception ex)            
            {
                MessageBox.Show($"Could not launch shortcut \"{Title}\"\r\n{ex}");
            }
        };

        public string Title { get; }

        public string Description { get; }
    }
}
