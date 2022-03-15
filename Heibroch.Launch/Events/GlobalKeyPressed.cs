using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class GlobalKeyPressed : IInternalEvent
    {
        public int Key { get; set; }

        public bool ProcessKey { get; set; } = true;

        public bool LogEvent { get; set; } = true;
    }
}
