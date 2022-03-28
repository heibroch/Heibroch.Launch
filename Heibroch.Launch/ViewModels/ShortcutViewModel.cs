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
        private readonly SelectionCycler selectionCycler;
        private KeyValuePair<string, ILaunchShortcut> selectedQueryResult;
        private KeyValuePair<string, ILaunchShortcut> selectedMostUsedResult;

        public ShortcutViewModel(IShortcutCollection<string, ILaunchShortcut> shortcutCollection,
                                 ISettingsRepository settingsRepository,
                                 IInternalMessageBus internalMessageBus)
        {
            this.shortcutCollection = shortcutCollection;
            this.settingsRepository = settingsRepository;
            this.internalMessageBus = internalMessageBus;

            selectionCycler = new SelectionCycler();

            Initialize();
        }

        private void Initialize()
        {
            shortcutCollection.Load(Constants.RootPath, true);

            internalMessageBus.Subscribe<UserShortcutSelectionIncremented>(OnUserShortcutSelectionIncremented);
        }

        private void OnUserShortcutSelectionIncremented(UserShortcutSelectionIncremented obj)
        {
            var increment = obj.Increment;
            if (DisplayedQueryResults.Count <= 0) return;

            //Move/cycle visibility/shortcuts
            selectionCycler.Increment(increment, shortcutCollection.QueryResults.Count);

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

                shortcutCollection.Filter(value);

                if (value.Length < oldValue.Length && oldValue.StartsWith(value))
                    selectionCycler.Reset();

                RaisePropertyChanged(nameof(LaunchText));
                RaisePropertyChanged(nameof(DisplayedQueryResults));
                RaisePropertyChanged(nameof(DisplayedMostUsedResults));
                RaisePropertyChanged(nameof(QueryResultsVisibility));
                RaisePropertyChanged(nameof(WaterMarkVisibility));

                if (!shortcutCollection.QueryResults.Any(x => x.Key == (selectedQueryResult.Key ?? string.Empty)))
                    SelectedQueryResult = shortcutCollection.QueryResults.FirstOrDefault();

                RaisePropertyChanged(nameof(SelectedQueryResult));
            }
        }


        public List<KeyValuePair<string, ILaunchShortcut>> DisplayedMostUsedResults => new List<KeyValuePair<string, ILaunchShortcut>>()
        {
            new KeyValuePair<string, ILaunchShortcut>("test1", new LaunchShortcut("test1", "bleeeeeurgh")),
            new KeyValuePair<string, ILaunchShortcut>("test2", new LaunchShortcut("test3", "bleeeeweeurgh")),
            new KeyValuePair<string, ILaunchShortcut>("test3", new LaunchShortcut("test3", "bleeewfeeeurgh")),
        };

        public KeyValuePair<string, ILaunchShortcut> SelectedMostUsedResult
        {
            get => selectedMostUsedResult;
            set
            {
                selectedMostUsedResult = value;
                RaisePropertyChanged(nameof(SelectedMostUsedResult));
            }
        }

        public List<KeyValuePair<string, ILaunchShortcut>> DisplayedQueryResults => new List<KeyValuePair<string, ILaunchShortcut>>(selectionCycler.SubSelect(shortcutCollection.QueryResults));

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
            RaisePropertyChanged(nameof(MostUsedResultsVisibility));
        }

        public void ExecuteSelection() => internalMessageBus.Publish(new ShortcutExecutingStarted() { ShortcutKey = shortcutCollection.QueryResults.Count == 1 ? shortcutCollection.QueryResults.First().Key : (SelectedQueryResult.Key ?? LaunchText) });

        public Visibility MostUsedResultsVisibility
        {
            get
            {
                bool.TryParse(settingsRepository.Settings[Constants.SettingNames.ShowMostUsed], out var showMostUsed);
                return showMostUsed ? Visibility.Visible : Visibility.Hidden;
            }
            
        }

        public Visibility QueryResultsVisibility => shortcutCollection.QueryResults.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility WaterMarkVisibility => LaunchText?.Length <= 0 ? Visibility.Visible : Visibility.Hidden;
    }
}
