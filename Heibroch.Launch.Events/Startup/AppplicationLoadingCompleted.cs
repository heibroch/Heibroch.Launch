using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ApplicationLoadingCompleted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;
    }
}
