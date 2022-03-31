using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ShortcutsLoadingStarted : IInternalMessage
    {
        public ShortcutsLoadingStarted(string path, bool clear)
        {
            Path = path;
            Clear = clear;
        }

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;

        public bool Clear { get; set; }

        public override string ToString() => $"Path: {Path}";
    }
}
