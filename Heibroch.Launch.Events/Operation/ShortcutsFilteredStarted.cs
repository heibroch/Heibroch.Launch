using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ShortcutsFilteredStarted : IInternalMessage
    {
        public ShortcutsFilteredStarted(string oldFilter, string newFilter)
        {
            OldFilter = oldFilter;
            NewFilter = newFilter;
        }

        public string OldFilter { get; set; }

        public string NewFilter { get; set; }

        public bool LogPublish { get; set; } = true;
    }
}
