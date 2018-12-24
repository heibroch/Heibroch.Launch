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
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e) => QueryTextBox.Focus();
    }
}
