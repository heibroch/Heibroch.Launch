using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Heibroch.Common.Wpf;
using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.ViewModels
{
    public class ShortcutViewModel : ViewModelBase
    {
        private readonly IShortcutCollection<string, ILaunchShortcut> shortcutCollection;
        private readonly ISettingsRepository settingsRepository;
        private readonly IInternalMessageBus internalMessageBus;
        private readonly IMostUsedRepository mostUsedRepository;
        private readonly SelectionCycler selectionCycler;
        private KeyValuePair<string, ILaunchShortcut> selectedQueryResult;
        
        public ShortcutViewModel(IShortcutCollection<string, ILaunchShortcut> shortcutCollection,
                                 ISettingsRepository settingsRepository,
                                 IInternalMessageBus internalMessageBus,
                                 IMostUsedRepository mostUsedRepository)
        {
            this.shortcutCollection = shortcutCollection;
            this.settingsRepository = settingsRepository;
            this.internalMessageBus = internalMessageBus;
            this.mostUsedRepository = mostUsedRepository;

            selectionCycler = new SelectionCycler();

            internalMessageBus.Subscribe<UserShortcutSelectionIncremented>(OnUserShortcutSelectionIncremented);
            internalMessageBus.Subscribe<ShortcutsFilteredCompleted>(OnShortcutsFilteredCompleted);
        }

        private void OnShortcutsFilteredCompleted(ShortcutsFilteredCompleted obj)
        {
            var oldFilter = obj.OldFilter;
            var newFilter = obj.NewFilter;

            if (newFilter.Length < oldFilter.Length && oldFilter.StartsWith(newFilter))
                selectionCycler.Reset();

            RaisePropertyChanged(nameof(LaunchText));
            RaisePropertyChanged(nameof(DisplayedQueryResults));
            RaisePropertyChanged(nameof(QueryResultsVisibility));
            RaisePropertyChanged(nameof(WaterMarkVisibility));

            if (!shortcutCollection.QueryResults.Any(x => x.Key == (selectedQueryResult.Key ?? string.Empty)))
                SelectedQueryResult = shortcutCollection.QueryResults.FirstOrDefault();

            RaisePropertyChanged(nameof(SelectedQueryResult));
        }

        private void OnUserShortcutSelectionIncremented(UserShortcutSelectionIncremented obj)
        {
            var increment = obj.Increment;
            if (DisplayedQueryResults.Count <= 0) return;

            //Move/cycle visibility/shortcuts
            selectionCycler.Increment(increment, IsShowMostUsedEnabled && string.IsNullOrEmpty(LaunchText)
                ? mostUsedRepository.ShortcutUseCounts.Count
                : shortcutCollection.QueryResults.Count);

            if (string.IsNullOrWhiteSpace(SelectedQueryResult.Key))
            {
                SelectedQueryResult = DisplayedQueryResults.First();
                return;
            }

            if (!DisplayedQueryResults.Any(x => x.Key == SelectedQueryResult.Key) && DisplayedQueryResults.Any())
            {
                SelectedQueryResult = DisplayedQueryResults.First();
                return;
            }

            if (DisplayedQueryResults.Count == 1)
            {
                SelectedQueryResult = DisplayedQueryResults.First();
                return;
            }

            var index = DisplayedQueryResults.FindIndex(x => x.Key == SelectedQueryResult.Key) + increment;
            if (index >= DisplayedQueryResults.Count || index < 0) return;

            RaisePropertyChanged(nameof(DisplayedQueryResults));

            SelectedQueryResult = DisplayedQueryResults.ElementAt(selectionCycler.CycleIndex);

            Debug.WriteLine($"SelectedItem now {SelectedQueryResult.Key} \r\n" +
                            $"StartIndex {selectionCycler.StartIndex}\r\n" +
                            $"StopIndex {selectionCycler.StopIndex}\r\n" +
                            $"TotalIndex {selectionCycler.CurrentIndex}\r\n" +
                            $"CycleIndex {selectionCycler.CycleIndex}\r\n" +
                            $"Delta {selectionCycler.Delta}\r\n" +
                            $"CollectionCount {shortcutCollection.QueryResults.Count}" +
                            $"-----------------------------------------------");
        }

        public string LaunchText
        {
            get => shortcutCollection.CurrentQuery ?? string.Empty;
            set
            {
                var oldValue = shortcutCollection?.CurrentQuery ?? string.Empty;

                internalMessageBus.Publish(new ShortcutsFilteredStarted(oldValue, value));
            }
        }

        public List<KeyValuePair<string, ILaunchShortcut>> DisplayedQueryResults
        {
            get
            {
                if (string.IsNullOrEmpty(LaunchText) && IsShowMostUsedEnabled)
                {
                    return mostUsedRepository.ShortcutUseCounts.OrderByDescending(x => x.Item2)
                        .Select(x => shortcutCollection.Shortcuts.FirstOrDefault(y => y.Key == x.Item1))
                        .Where(x => x.Key != default)
                        .ToList();
                }
                return new List<KeyValuePair<string, ILaunchShortcut>>(selectionCycler.SubSelect(shortcutCollection.QueryResults));
            }           
        }

        public KeyValuePair<string, ILaunchShortcut> SelectedQueryResult
        {
            get => selectedQueryResult;
            set
            {
                selectedQueryResult = value;
                RaisePropertyChanged(nameof(SelectedQueryResult));
            }
        }

        public void Reset()
        {
            LaunchText = string.Empty;
            selectionCycler.Reset();
        }

        public void ExecuteSelection()
        {
            internalMessageBus.Publish(new ShortcutExecutingStarted() 
            { 
                ShortcutKey = shortcutCollection.QueryResults.Count == 1 
                    ? shortcutCollection.QueryResults.First().Key 
                    : (SelectedQueryResult.Key ?? LaunchText) 
            });
        }

        public Visibility QueryResultsVisibility => IsShowMostUsedEnabled && mostUsedRepository.ShortcutUseCounts.Count > 0 || 
            shortcutCollection.QueryResults.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility WaterMarkVisibility => LaunchText?.Length <= 0 ? Visibility.Visible : Visibility.Hidden;

        private bool IsShowMostUsedEnabled
        {
            get
            {
                if (!bool.TryParse(settingsRepository.Settings[Constants.SettingNames.ShowMostUsed], out var showMostUsed))
                    return false;

                return showMostUsed;
            }
        }
    }
}
