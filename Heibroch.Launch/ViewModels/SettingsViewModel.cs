using Heibroch.Common.Wpf;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Heibroch.Launch.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsRepository settingsRepository;

        public SettingsViewModel(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
            this.settingsRepository.Load();

            ModifierKeys = Enum.GetValues(typeof(ModifierKeys)).Cast<ModifierKeys>().ToList();
            Keys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();

            Enum.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.Modifier1).Value, out ModifierKeys modifierKey1);
            Modifier1 = modifierKey1;
            Enum.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.Modifier2).Value, out ModifierKeys modifierKey2);
            Modifier2 = modifierKey2;
            Enum.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.Key).Value, out Keys key);
            Key = key;
            bool.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.UseStickySearch).Value, out bool useStickySearch);
            UseStickySearch = useStickySearch;
            bool.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.ShowMostUsed).Value, out bool showMostUsed);
            ShowMostUsed = showMostUsed;

            SaveCommand = new ActionCommand(x =>
            {
                this.settingsRepository.Save(Modifier1.ToString(), Modifier2.ToString(), Key.ToString(), UseStickySearch, ShowMostUsed);
                MessageBox.Show("Settings saved!");
            });
        }

        public ModifierKeys Modifier1 { get; set; }

        public ModifierKeys Modifier2 { get; set; }

        public Keys Key { get; set; }

        public bool UseStickySearch { get; set; }

        public bool ShowMostUsed { get; set; }

        public List<Keys> Keys { get; set; }

        public List<ModifierKeys> ModifierKeys { get; set; }

        public ActionCommand SaveCommand { get; set; }

        public string SettingsTitle => $"{Application.ProductName} v. {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
    }
}
