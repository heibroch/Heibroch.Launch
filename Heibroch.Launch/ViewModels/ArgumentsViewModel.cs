using System.Windows;
using Heibroch.Common.Wpf;

namespace Heibroch.Launch.ViewModels
{
    public class ArgumentsViewModel : ViewModelBase
    {
        private readonly IShortcutExecutor shortcutExecutor;
        private string launchText;

        public ArgumentsViewModel(IShortcutExecutor shortcutExecutor)
        {
            this.shortcutExecutor = shortcutExecutor;
        }

        public void Execute() => shortcutExecutor.ExecuteDirect(Command.Replace("[Arg]", LaunchText));

        public string LaunchText
        {
            get => launchText;
            set
            {
                launchText = value;
                RaisePropertyChanged(nameof(WaterMarkVisibility));
                RaisePropertyChanged(nameof(LaunchText));
            }
        }

        public string Command { get; set; }

        public Visibility WaterMarkVisibility => LaunchText?.Length <= 0 || string.IsNullOrWhiteSpace(launchText) ? Visibility.Visible : Visibility.Hidden;
    }
}
