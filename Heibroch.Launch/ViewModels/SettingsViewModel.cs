using Heibroch.Common.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Heibroch.Launch.ViewModels
{
    public class SettingsViewModel
    {
        private readonly ISettingCollection settingCollection;

        public SettingsViewModel(ISettingCollection settingCollection)
        {
            this.settingCollection = settingCollection;
            this.settingCollection.Load();

            ModifierKeys = Enum.GetValues(typeof(ModifierKeys)).Cast<ModifierKeys>().ToList();
            Keys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();

            Enum.TryParse(this.settingCollection.Settings.FirstOrDefault(x => x.Key == "Modifier1").Value, out ModifierKeys modifierKey1);
            Modifier1 = modifierKey1;
            Enum.TryParse(this.settingCollection.Settings.FirstOrDefault(x => x.Key == "Modifier2").Value, out ModifierKeys modifierKey2);
            Modifier2 = modifierKey2;
            Enum.TryParse(this.settingCollection.Settings.FirstOrDefault(x => x.Key == "Key").Value, out Keys key);
            Key = key;

            SaveCommand = new ActionCommand(x =>
            {
                this.settingCollection.Save(Modifier1, Modifier2, Key);
                MessageBox.Show("Settings saved!");
            });
        }

        public ModifierKeys Modifier1 { get; set; }

        public ModifierKeys Modifier2 { get; set; }

        public Keys Key { get; set; }

        public List<Keys> Keys { get; set; }

        public List<ModifierKeys> ModifierKeys { get; set; }

        public ActionCommand SaveCommand { get; set; }

        public string SettingsTitle => $"{Application.ProductName} v. {Application.ProductVersion}";
    }
}
