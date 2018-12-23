using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Heibroch.Launch.Annotations;

namespace Heibroch.Launch
{
    public class ShortcutViewModel : INotifyPropertyChanged
    {
        private readonly IShortcutCollection _shortcutCollection;
        private KeyValuePair<string, string> _selectedItem;

        public ShortcutViewModel(IShortcutCollection shortcutCollection)
        {
            _shortcutCollection = shortcutCollection;
            _shortcutCollection.Load();
        }

        public string LaunchText
        {
            get => _shortcutCollection.CurrentQuery;
            set
            {
                _shortcutCollection.Filter(value);
                OnPropertyChanged(nameof(LaunchText));
                OnPropertyChanged(nameof(QueryResults));
                OnPropertyChanged(nameof(QueryResultsVisibility));
            }
        }

        public SortedList<string, string> QueryResults => _shortcutCollection.QueryResults;

        public KeyValuePair<string, string> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
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
                //else if(command.StartsWith(Defaults.CommandLineCommand))
                //{
                //    var commandLineArg = command.Remove(0, Defaults.CommandLineCommand.Length);
                //    Process.Start(commandLineArg);
                //}

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
            
            //Application.Current?.Shutdown();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Visibility QueryResultsVisibility => QueryResults.Count > 0 ? Visibility.Visible : Visibility.Hidden;
    }
}
