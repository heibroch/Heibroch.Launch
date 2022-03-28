using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using System;

namespace Heibroch.Launch
{
    public class MostUsedShortcutsRepository
    {
        private readonly IInternalMessageBus internalMessageBus;

        public MostUsedShortcutsRepository(IInternalMessageBus internalMessageBus)
        {
            this.internalMessageBus = internalMessageBus;

            internalMessageBus.Subscribe<ShortcutExecutingCompleted>(OnShortcutExecutingCompleted);
            internalMessageBus.Subscribe<ShortcutsLoadingCompleted>(OnShortcutsLoadingCompleted);
        }

        private void OnShortcutsLoadingCompleted(ShortcutsLoadingCompleted obj)
        {
            //Read out most-used lines and compare them to the current settings collection
            //Remove all entries from most-used that do not exist in the settings collection from persisted storage
            //Publish event for most used shortcuts so that the viewmodels can fetch them
            throw new NotImplementedException();
        }

        private void OnShortcutExecutingCompleted(ShortcutExecutingCompleted obj)
        {
            //Persist usage count on given shortcut
            //Update most used list
            //Publish most used list updated
            throw new NotImplementedException();
        }
    }
}
