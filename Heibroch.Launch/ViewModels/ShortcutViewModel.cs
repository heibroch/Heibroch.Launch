using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Heibroch.Common;
using Heibroch.Common.Wpf;

namespace Heibroch.Launch
{
    public class ShortcutViewModel : ViewModelBase
    {
        private readonly IShortcutCollection shortcutCollection;
        private readonly IEventBus eventBus;
        private KeyValuePair<string, string> _selectedItem;

        public ShortcutViewModel(IShortcutCollection shortcutCollection,
                                 IEventBus eventBus)
        {
            this.eventBus = eventBus;
            this.shortcutCollection = shortcutCollection;

            Initialize();
        }

        private void Initialize() => shortcutCollection.Load();

        public string LaunchText
        {
            get => shortcutCollection.CurrentQuery;
            set
            {
                shortcutCollection.Filter(value);                
                RaisePropertyChanged(nameof(LaunchText));
                RaisePropertyChanged(nameof(QueryResults));
                RaisePropertyChanged(nameof(QueryResultsVisibility));
            }
        }

        public SortedList<string, string> QueryResults => shortcutCollection.QueryResults;

        public KeyValuePair<string, string> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public void IncrementSelection(int increment)
        {
            if (QueryResults.Count <= 0) return;

            if (string.IsNullOrWhiteSpace(SelectedItem.Key))
            {
                SelectedItem = QueryResults.First();
                return;
            }

            if (!QueryResults.ContainsKey(SelectedItem.Key))
            {
                SelectedItem = QueryResults.First();
                return;
            }
            
            var index = QueryResults.IndexOfKey(SelectedItem.Key) + increment;
            if (index >= QueryResults.Count || index < 0) return;

            SelectedItem = QueryResults.ElementAt(index);
            
        }

        public void ExecuteSelection()
        {
            if (string.IsNullOrWhiteSpace(SelectedItem.Key)) return;
            if (!QueryResults.ContainsKey(SelectedItem.Key)) return;

            var command = QueryResults[SelectedItem.Key];

            //// opens the folder in explorer
            //Process.Start(@"c:\temp");
            //// opens the folder in explorer
            //Process.Start("explorer.exe", @"c:\temp");
            //// throws exception
            //Process.Start(@"c:\does_not_exist");
            //// opens explorer, showing some other folder)
            //Process.Start("explorer.exe", @"c:\does_not_exist");
            try
            {
                if (command.StartsWith(Defaults.CommandLineCommand))
                {
                    var commandLineArg = command.Remove(0, Defaults.CommandLineCommand.Length);
                    Process.Start("CMD.exe", commandLineArg);
                    
                }
                else
                {
                    Process.Start(command);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show($"Failed to execute: {command}");
            }
        }
        
        public Visibility QueryResultsVisibility => QueryResults.Count > 0 ? Visibility.Visible : Visibility.Hidden;
    }
}
