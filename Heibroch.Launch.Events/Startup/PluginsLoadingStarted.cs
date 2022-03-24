using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class PluginsLoadingStarted : IInternalMessage
    {
        public PluginsLoadingStarted(string directory) => Directory = directory;

        public string Directory { get; set; }

        public bool LogPublish { get; set; } = true;

        public override string ToString() => $"Plugins root directory: {Directory}";
    }
}
