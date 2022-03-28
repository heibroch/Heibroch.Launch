using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class UserShortcutSelectionIncremented : IInternalMessage
    {
        public bool LogPublish { get; set; } = true;

        public int Increment { get; set; }

        public override string ToString() => $"Value {Increment}";
    }
}
