﻿using Heibroch.Common.Wpf;
using Heibroch.Infrastructure.Interfaces.Logging;
using Heibroch.Launch.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Heibroch.Launch.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsRepository settingsRepository;
        private readonly IInternalLogger internalLogger;

        public SettingsViewModel(ISettingsRepository settingsRepository, IInternalLogger internalLogger)
        {
            this.internalLogger = internalLogger;
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
            bool.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.LogInfo).Value, out bool logInfo);
            LogInfo = internalLogger.IsFailingLogging ? false : logInfo;
            bool.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.LogWarnings).Value, out bool logWarnings);
            LogWarnings = internalLogger.IsFailingLogging ? false : logWarnings;
            bool.TryParse(this.settingsRepository.Settings.FirstOrDefault(x => x.Key == Constants.SettingNames.LogErrors).Value, out bool logErrors);
            LogErrors = internalLogger.IsFailingLogging ? false : logErrors;

            SaveCommand = new ActionCommand(x =>
            {
                if (internalLogger.IsFailingLogging)
                    MessageBox.Show("You do not have permissions on this machine to log to the application log. Therefore logging will be disabled.");
                
                this.internalLogger.LogInfoAction = LogInfo && !internalLogger.IsFailingLogging ? x => EventLog.WriteEntry("Heibroch.Launch", x, EventLogEntryType.Information) : x => { };
                this.internalLogger.LogWarningAction = LogWarnings && !internalLogger.IsFailingLogging ? x => EventLog.WriteEntry("Heibroch.Launch", x, EventLogEntryType.Warning) : x => { };
                this.internalLogger.LogErrorAction = LogErrors && internalLogger.IsFailingLogging ? x => EventLog.WriteEntry("Heibroch.Launch", x, EventLogEntryType.Error) : x => { };

                this.settingsRepository.Save(Modifier1.ToString(), Modifier2.ToString(), Key.ToString(), UseStickySearch, ShowMostUsed, LogInfo, LogWarnings, LogErrors);
                MessageBox.Show("Settings saved!");

                RaisePropertyChanged(nameof(LogInfo));
                RaisePropertyChanged(nameof(LogWarnings));
                RaisePropertyChanged(nameof(LogErrors));
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

        public bool LogInfo { get; set; }

        public bool LogWarnings { get; set; }
        
        public bool LogErrors { get; set; }

        public string SettingsTitle => $"{Application.ProductName} v. {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
    }
}
