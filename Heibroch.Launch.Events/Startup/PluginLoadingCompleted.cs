using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class PluginLoadingCompleted : IInternalMessage
    {
        public PluginLoadingCompleted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;

        public override string ToString() => $"Plugin directory: {Path}";
    }
}
