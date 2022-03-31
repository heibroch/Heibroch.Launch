using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Events
{
    public class ShortcutsLoadingCompleted : IInternalMessage
    {
        public ShortcutsLoadingCompleted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;

        public Dictionary<string, ILaunchShortcut> Shortcuts { get; set; }

        public override string ToString() => $"Path: {Path}";
    }
}
