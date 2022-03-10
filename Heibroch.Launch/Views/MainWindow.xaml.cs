using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Heibroch.Common;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using Heibroch.Launch.Plugin;
using Heibroch.Launch.ViewModels;

namespace Heibroch.Launch.Views
{
    public partial class MainWindow : Window
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc lowLevelKeyboardProc = HookCallback;
        private static IntPtr hookId = IntPtr.Zero;
        private static IEventBus eventBus;
        private IPluginLoader pluginLoader;
                
        public MainWindow()
        {
            try
            {
                eventBus = new EventBus();
                Container.Current.Register<IEventBus>(eventBus);

                //Fixes an issue with current directory being system32 for the plugin loader and not the application path as desired
                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Setting base directory...", EventLogEntryType.Information));
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing components...", EventLogEntryType.Information));
                InitializeComponent();
                
                var logService = new LogService(eventBus);
                Container.Current.Register<ILogService>(logService);

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing plugin loader...", EventLogEntryType.Information));
                pluginLoader = new PluginLoader(eventBus, Container.Current);
                Container.Current.Register<IPluginLoader>(pluginLoader);

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing string search engine...", EventLogEntryType.Information));
                var stringSearchEngine = new StringSearchEngine<ILaunchShortcut>();

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing settings repository...", EventLogEntryType.Information));
                var settingsRepository = new SettingsRepository(eventBus);
                Container.Current.Register<ISettingsRepository>(settingsRepository);

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing shortcut collection...", EventLogEntryType.Information));
                var shortcutCollection = new ShortcutCollection(pluginLoader, eventBus, stringSearchEngine, settingsRepository);
                Container.Current.Register<IShortcutCollection<string, ILaunchShortcut>>(shortcutCollection);

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing shortcut executor...", EventLogEntryType.Information));
                var shortcutExecutor = new ShortcutExecutor(shortcutCollection);
                Container.Current.Register<IShortcutExecutor>(shortcutExecutor);

                

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing plugins...", EventLogEntryType.Information));
                pluginLoader.Load();
                eventBus.Publish(new ProgramLoadedEvent());

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing main view model...", EventLogEntryType.Information));
                DataContext = new MainViewModel(eventBus, shortcutCollection, shortcutExecutor, settingsRepository);

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initializing hooks...", EventLogEntryType.Information));
                hookId = SetHook(lowLevelKeyboardProc);
                Closing += OnMainWindowClosing;

                eventBus.Publish(new LogEntryPublished("Heibroch.Launch - Initializing", "Initialization complete", EventLogEntryType.Information));
            }
            catch (Exception ex)
            {
                eventBus.Publish(new LogEntryPublished("Heibroch.Launch", ex.StackTrace, EventLogEntryType.Error));
                throw ex;
            }            
        }

        private void OnTrayIconDoubleClick(object sender, EventArgs e) => throw new NotImplementedException();

        private void OnTrayIconClick(object sender, EventArgs e) => throw new NotImplementedException();

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { UnhookWindowsHookEx(hookId); }
            catch { }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            {
                using (var curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                var globalKeyPressed = new GlobalKeyPressed() { Key = Marshal.ReadInt32(lParam) };
                eventBus.Publish(globalKeyPressed);
                if (!globalKeyPressed.ProcessKey) return new IntPtr(1);
            }

            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lowLevelKeyboardProc, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
