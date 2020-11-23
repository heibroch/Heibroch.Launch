namespace Heibroch.Launch.Plugin
{
    public interface ILaunchPlugin
    {
        ILaunchShortcut CreateShortcut(string title, string description);

        /// <summary>
        /// At the start of the description. If there's a match, it will create the shortcut via the given plugin
        /// </summary>
        string ShortcutFilter { get; }
    }
}
