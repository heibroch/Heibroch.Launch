using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ApplicationLoadingCompleted : IInternalMessage
    {
        public string? RootPath { get; set; }

        public bool LogPublish { get; set; } = true;
    }
}
