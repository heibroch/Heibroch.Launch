using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Interfaces;
using Heibroch.Launch.Events;

namespace Reload
{
    public class ReloadShortcut : ILaunchShortcut
    {
        private IInternalMessageBus internalMessageBus;

        public ReloadShortcut(IInternalMessageBus internalMessageBus) => this.internalMessageBus = internalMessageBus;

        public Action<IEnumerable<KeyValuePair<string, string>>, string> Execute => (IEnumerable<KeyValuePair<string, string>> args, string formattedDescription) =>
        {
            internalMessageBus.Publish(new ShortcutsLoadingStarted());
        };
        
        public string Title => "Reload";

        public string Description => "This application command will reload the collection of shortcuts";
    }
}
