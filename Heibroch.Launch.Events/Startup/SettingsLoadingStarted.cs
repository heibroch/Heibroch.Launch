using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class SettingsLoadingStarted : IInternalMessage
    {
        public SettingsLoadingStarted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;

        public override string ToString() => $"Settings path: {Path}";
    }
}
