using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ShortcutArgumentFilled : IInternalMessage
    {
        public ShortcutArgumentFilled(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public bool LogPublish { get; set; } = true;
    }
}
