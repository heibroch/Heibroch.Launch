namespace Heibroch.Launch.Interfaces
{
    public interface IShortcutExecutor
    {
        void AddArgument(string key, string value);
        IEnumerable<string> GetArgKeys(string shortcut);
        bool IsArgShortcut(string shortcut);
        Dictionary<string, string> Arguments { get; }
    }
}
