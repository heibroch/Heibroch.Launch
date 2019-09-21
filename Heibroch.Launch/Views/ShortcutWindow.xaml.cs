using System.Windows;
using System.Windows.Input;

namespace Heibroch.Launch.Views
{
    public partial class ShortcutWindow : Window
    {
        public ShortcutWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
            QueryTextBox.Loaded += QueryTextBox_Loaded;
        }

        private void QueryTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            //QueryTextBox.Focus();
            //QueryTextBox.Select(0, 0);
            //FocusManager.SetFocusedElement(this, QueryTextBox);
            Keyboard.Focus(QueryTextBox);
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            

        }
    }
}
