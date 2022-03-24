using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    internal class TrayIconInitializingCompleted : IInternalMessage
    {
        public TrayIconInitializingCompleted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;
    }
}
