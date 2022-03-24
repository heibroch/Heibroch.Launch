using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class SettingsLoadingCompleted : IInternalMessage
    {
        public SettingsLoadingCompleted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;

        public override string ToString() => $"Settings path: {Path}";
    }
}
