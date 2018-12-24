using Heibroch.Common;
using System.Windows;
using System.Windows.Input;

namespace Heibroch.Launch
{
    public partial class ShortcutWindow : Window
    {
        private IEventBus eventBus;

        public ShortcutWindow(IEventBus eventBus)
        {
            this.eventBus = eventBus;

            InitializeComponent();

            Loaded += OnMainWindowLoaded;
            PreviewKeyDown += OnMainWindowPreviewKeyDown;
        }
        
        //Necessary in order for a space not to be in the textbox from the beginning
        private void OnMainWindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control || e.Key != Key.Space) return;
            e.Handled = true;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e) => QueryTextBox.Focus();
    }
}
