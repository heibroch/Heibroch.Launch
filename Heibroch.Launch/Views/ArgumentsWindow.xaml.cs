using System.Windows;

namespace Heibroch.Launch.Views
{
    public partial class ArgumentsWindow : Window
    {
        public ArgumentsWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e) => QueryTextBox.Focus();
    }
}
