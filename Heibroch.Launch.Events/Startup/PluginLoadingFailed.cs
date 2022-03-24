using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class PluginLoadingFailed : IInternalMessage
    {
        public PluginLoadingFailed(Exception exception) => Exception = exception;

        public Exception Exception { get; set; }

        public bool LogPublish { get; set; } = true;

        public override string ToString() => $"{Exception}\r\n{Exception.StackTrace}";
    }
}
