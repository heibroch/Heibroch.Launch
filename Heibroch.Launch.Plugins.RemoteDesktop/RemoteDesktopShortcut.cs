using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Plugins.RemoteDesktop
{
    public class RemoteDesktopShortcut : ILaunchShortcut
    {
        public RemoteDesktopShortcut(Action<string, string> action, string title, string description)
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
