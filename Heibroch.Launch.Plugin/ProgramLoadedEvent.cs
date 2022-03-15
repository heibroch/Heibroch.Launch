using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Plugin
{
    public class ProgramLoadedEvent : IInternalEvent
    {
        public bool LogEvent { get; set; } = true;
    }
}
