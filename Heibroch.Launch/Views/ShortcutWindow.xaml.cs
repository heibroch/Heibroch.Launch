using System.Windows;
using System.Windows.Input;

namespace Heibroch.Launch.Views
{
    public partial class ShortcutWindow : Window
    {
        public ShortcutWindow()
        {
            InitializeComponent();
            QueryTextBox.Loaded += QueryTextBox_Loaded;
        }

        private void QueryTextBox_Loaded(object sender, RoutedEventArgs e) => Keyboard.Focus(QueryTextBox);
    }
}
