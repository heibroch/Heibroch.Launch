using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ApplicationLoadingStarted : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;
    }
}
