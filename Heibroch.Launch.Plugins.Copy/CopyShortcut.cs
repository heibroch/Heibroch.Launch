using Heibroch.Launch.Plugin;
using System;
using System.Collections.Generic;

namespace Heibroch.Launch.Plugins.Copy
{
    public class CopyShortcut : ILaunchShortcut
    {
        public CopyShortcut(Action<string, string> action, string title, string description)
        {
            Title = title;
            Description = description;
            Execute = (IEnumerable<KeyValuePair<string, string>> arguments, string formattedDescription) => action(title, formattedDescription);
        }

        public Action<IEnumerable<KeyValuePair<string, string>>, string> Execute { get; }

        public string Title { get; }

        public string Description { get; }
    }
}
