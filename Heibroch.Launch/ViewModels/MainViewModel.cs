using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using Heibroch.Common;
using Heibroch.Common.Wpf;
using Heibroch.Launch.Events;
using Heibroch.Launch.Views;

namespace Heibroch.Launch.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventBus eventBus;

        private readonly IShortcutCollection shortcutCollection;
        private readonly IShortcutExecutor shortcutExecutor;
        private readonly ISettingCollection settingCollection;

        private static ShortcutWindow currentShortcutWindow = null;
        private static ShortcutViewModel shortcutViewModel;
        private static SettingsWindow currentSettingsWindow = null;
        private static SettingsViewModel settingsViewModel;
        private static ArgumentsWindow currentArgumentsWindow = null;
        private static ArgumentsViewModel argumentsViewModel;


        private DispatcherTimer dispatcherTimer;
        private TrayIcon trayIcon;

        public MainViewModel() : this(Container.Current.Resolve<IEventBus>(),
                                      Container.Current.Resolve<IShortcutCollection>(),
                                      Container.Current.Resolve<IShortcutExecutor>(),
                                      Container.Current.Resolve<ISettingCollection>())
        {

        }

        public MainViewModel(IEventBus eventBus,
                             IShortcutCollection shortcutCollection,
                             IShortcutExecutor shortcutExecutor,
                             ISettingCollection settingCollection)
        {
            this.eventBus = eventBus;
            this.shortcutCollection = shortcutCollection;
            this.shortcutExecutor = shortcutExecutor;
            this.settingCollection = settingCollection;
            Initialize();
        }
        
        private void Initialize()
        {
            shortcutViewModel = new ShortcutViewModel(shortcutCollection, shortcutExecutor);
            settingsViewModel = new SettingsViewModel(settingCollection);
            argumentsViewModel = new ArgumentsViewModel(shortcutExecutor);

            eventBus.Subscribe<GlobalKeyPressed>(OnKeyPressed);

            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += OnDispatcherTimerTick;
            dispatcherTimer.Start();

            trayIcon = new TrayIcon(OnTrayIconContextMenuItemClicked, new List<string> {"Settings", "Exit"});
        }

        private void OnTrayIconContextMenuItemClicked(string obj)
        {
            if (obj == Constants.ContextMenu.Exit)
            {
                trayIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }

            if (obj == Constants.ContextMenu.Settings)
            {
                currentSettingsWindow = new SettingsWindow();
                currentSettingsWindow.DataContext = settingsViewModel;
                currentSettingsWindow.Show();
                currentSettingsWindow.Activate();
            }
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            if (currentShortcutWindow == null) return;
            
            if (!currentShortcutWindow.IsActive)
                currentShortcutWindow.Activate();

            //if (!currentShortcutWindow.IsFocused)
            //    currentShortcutWindow.Focus();
        }

        private void OnKeyPressed(GlobalKeyPressed obj)
        {
            if (currentShortcutWindow != null || currentArgumentsWindow != null)
            {
                switch (obj.Key)
                {
                    case 0x0000001B: //Escape
                        obj.ProcessKey = false;
                        CloseShortcutWindow();
                        CloseArgumentWindow();
                        break;
                    case 0x00000028: //Down
                        shortcutViewModel?.IncrementSelection(1);
                        break;
                    case 0x00000026: //Up
                        shortcutViewModel?.IncrementSelection(-1);
                        break;
                    case 0x0000000D: //Enter
                        obj.ProcessKey = false;

                        //If the current argument window is open, then execute it
                        if (currentArgumentsWindow != null)
                        {
                            argumentsViewModel.Execute();
                            CloseArgumentWindow();
                        }
                        
                        //If the selected shortcut is an argument command, then display the argument window
                        if (currentShortcutWindow != null && (shortcutViewModel.SelectedItem.Value?.Contains("[Arg]") ?? false))
                        {
                            argumentsViewModel.Command = shortcutViewModel.SelectedItem.Value;
                            CloseShortcutWindow();
                            
                            currentArgumentsWindow = new ArgumentsWindow();
                            currentArgumentsWindow.DataContext = argumentsViewModel;
                            currentArgumentsWindow.Show();
                            currentArgumentsWindow.Activate();
                        }

                        //If the current shortcut window is open, then execute it
                        if (currentShortcutWindow != null)
                        {
                            shortcutViewModel.ExecuteSelection();
                            CloseShortcutWindow();
                        }

                        break;
                }
            }
            else if (Keyboard.Modifiers == (settingsViewModel.Modifier1 | settingsViewModel.Modifier2) && obj.Key == (int)settingsViewModel.Key) //Space
            {
                currentShortcutWindow = new ShortcutWindow();
                currentShortcutWindow.DataContext = shortcutViewModel; 
                currentShortcutWindow.Show();
                currentShortcutWindow.Activate();

                obj.ProcessKey = false;

                shortcutViewModel.Reset();
            }
        }

        private void CloseShortcutWindow()
        {
            shortcutViewModel.Reset();
            currentShortcutWindow?.Close();
            currentShortcutWindow = null;
        }

        private void CloseArgumentWindow()
        {
            argumentsViewModel.Command = string.Empty;
            argumentsViewModel.LaunchText = string.Empty;
            currentArgumentsWindow?.Close();
            currentArgumentsWindow = null;
        }
    }
}
