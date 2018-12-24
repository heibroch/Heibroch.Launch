using System.Windows;
using System.Windows.Input;
using Heibroch.Common;
using Heibroch.Common.Wpf;
using Heibroch.Launch.Events;

namespace Heibroch.Launch
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventBus eventBus;
        private static Window currentShortcutWindow = null;
        private static ShortcutViewModel shortcutViewModel;

        public MainViewModel(IEventBus eventBus)
        {
            this.eventBus = eventBus;
            Initialize();
        }

        private void Initialize()
        {
            shortcutViewModel = new ShortcutViewModel(new ShortcutCollection(), eventBus);
            eventBus.Subscribe<GlobalKeyPressed>(OnKeyPressed);
        }

        private void OnKeyPressed(GlobalKeyPressed obj)
        {
            if (currentShortcutWindow != null)
            {
                switch (obj.Key)
                {
                    case 0x0000001B: //Escape
                        currentShortcutWindow.Close();
                        break;
                    case 0x00000028: //Down
                        shortcutViewModel.IncrementSelection(1);
                        break;
                    case 0x00000026: //Up
                        shortcutViewModel.IncrementSelection(-1);
                        break;
                    case 0x0000000D: //Enter
                        shortcutViewModel.ExecuteSelection();
                        currentShortcutWindow.Close();
                        break;
                }
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && obj.Key == 32) //Space
            {
                currentShortcutWindow = new ShortcutWindow(eventBus);
                currentShortcutWindow.Closing += OnShortcutWindowClosing;
                currentShortcutWindow.DataContext = shortcutViewModel;
                
                currentShortcutWindow.Show();
                currentShortcutWindow.Activate();

                shortcutViewModel.LaunchText = "";
            }
        }

        private void OnShortcutWindowClosing(object sender, System.ComponentModel.CancelEventArgs e) => currentShortcutWindow = null;
    }
}
