using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class PluginsLoadingCompleted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;
    }
}
