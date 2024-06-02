using System;
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
                var process = new Process();
                process.StartInfo = new ProcessStartInfo();
                process.StartInfo.UseShellExecute = true;

                if (formattedDescription.StartsWith(">"))
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.Arguments = $"/c {formattedDescription.Substring(1, formattedDescription.Length - 1)}";
                }

                else if (formattedDescription.StartsWith("\""))
                {
                    process.StartInfo.FileName = formattedDescription.Substring(1, formattedDescription.Length - 2);
                }

                else
                {
                    process.StartInfo.FileName = formattedDescription;
                }

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
