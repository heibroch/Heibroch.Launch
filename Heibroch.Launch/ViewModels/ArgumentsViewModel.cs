using System.Windows;
using Heibroch.Common.Wpf;
using Heibroch.Launch.Plugin;

namespace Heibroch.Launch.ViewModels
{
    public class ArgumentsViewModel : ViewModelBase
    {
        private readonly IShortcutExecutor shortcutExecutor;
        private string launchText;
        private string argumentKey;

        public ArgumentsViewModel(IShortcutExecutor shortcutExecutor) => this.shortcutExecutor = shortcutExecutor;

        public void Execute() => shortcutExecutor.AddArgument(ArgumentKey, launchText);

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

        public string WaterMarkText => $"{ArgumentKey}...";

        public ILaunchShortcut Command { get; set; }

        public Visibility WaterMarkVisibility => LaunchText?.Length <= 0 || string.IsNullOrWhiteSpace(launchText) ? Visibility.Visible : Visibility.Hidden;

        public string ArgumentKey
        {
            get => argumentKey;
            internal set
            {
                argumentKey = value;
                RaisePropertyChanged(nameof(WaterMarkText));
            }
        }

        public void Reset()
        {
            ArgumentKey = null;
            LaunchText = null;
            Command = null;
        }

        public void ExecuteArgument() => shortcutExecutor.AddArgument(ArgumentKey, LaunchText);
    }
}
