using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ShortcutsLoadingCompleted : IInternalMessage
    {
        public ShortcutsLoadingCompleted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;

        public override string ToString() => $"Path: {Path}";
    }
}
