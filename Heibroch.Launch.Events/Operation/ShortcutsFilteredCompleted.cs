using Heibroch.Infrastructure.Interfaces.MessageBus;

namespace Heibroch.Launch.Events
{
    public class ShortcutsFilteredCompleted : IInternalMessage
    {
        public ShortcutsFilteredCompleted(string oldFilter, string newFilter)
        {
            OldFilter = oldFilter;
            NewFilter = newFilter;
        }

        public string OldFilter { get; set; }

        public string NewFilter { get; set; }

        public bool LogPublish { get; set; } = true;
    }
}
