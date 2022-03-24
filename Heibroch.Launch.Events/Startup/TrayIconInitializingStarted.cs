using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class TrayIconInitializingStarted : IInternalMessage
    {
        public TrayIconInitializingStarted(string path) => Path = path;

        public string Path { get; set; }

        public bool LogPublish { get; set; } = true;
    }
}
