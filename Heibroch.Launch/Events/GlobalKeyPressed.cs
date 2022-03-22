using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class GlobalKeyPressed : IInternalMessage
    {
        public int Key { get; set; }

        public bool ProcessKey { get; set; } = true;

        public bool LogPublish { get; set; } = true;
    }
}
