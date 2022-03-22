using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Plugin
{
    public class ProgramLoadedEvent : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;
    }
}
