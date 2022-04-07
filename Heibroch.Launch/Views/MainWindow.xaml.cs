using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Heibroch.Common;
using Heibroch.Infrastructure.Interfaces.Logging;
using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Infrastructure.Logging;
using Heibroch.Infrastructure.Messaging;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using Heibroch.Launch.ViewModels;

namespace Heibroch.Launch.Views
{
    public partial class MainWindow : Window
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc lowLevelKeyboardProc = HookCallback;
        private static IntPtr hookId = IntPtr.Zero;
        private static IInternalMessageBus internalMessageBus;
        private IPluginLoader pluginLoader;

        public MainWindow()
        {
            try
            {
                var internalLogger = new InternalLogger();
                internalLogger.LogInfoAction = x => EventLog.WriteEntry("Heibroch.Launch", x, EventLogEntryType.Information);
                internalLogger.LogWarningAction = x => EventLog.WriteEntry("Heibroch.Launch", x, EventLogEntryType.Warning);
                internalLogger.LogErrorAction = x => EventLog.WriteEntry("Heibroch.Launch", x, EventLogEntryType.Error);
                Container.Current.Register<IInternalLogger>(internalLogger);

                internalMessageBus = new InternalMessageBus(internalLogger);
                Container.Current.Register<IInternalMessageBus>(internalMessageBus);

                internalMessageBus.Publish(new ApplicationLoadingStarted());

                //Fixes an issue with current directory being system32 for the plugin loader and not the application path as desired
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                
                InitializeComponent();

                var pathRepository = new PathRepository();
                Container.Current.Register<IPathRepository>(pathRepository);

                pluginLoader = new PluginLoader(internalMessageBus, Container.Current);
                Container.Current.Register<IPluginLoader>(pluginLoader);

                var stringSearchEngine = new StringSearchEngine<ILaunchShortcut>();
                Container.Current.Register<IStringSearchEngine<ILaunchShortcut>>(stringSearchEngine);

                var mostUsedRepository = new MostUsedRepository(internalMessageBus, pathRepository);
                Container.Current.Register<IMostUsedRepository>(mostUsedRepository);

                var settingsRepository = new SettingsRepository(internalMessageBus, pathRepository);
                Container.Current.Register<ISettingsRepository>(settingsRepository);

                var shortcutCollection = new ShortcutCollection(pluginLoader, internalMessageBus, stringSearchEngine, settingsRepository);
                Container.Current.Register<IShortcutCollection<string, ILaunchShortcut>>(shortcutCollection);

                var shortcutExecutor = new ShortcutExecutor(shortcutCollection, internalMessageBus);
                Container.Current.Register<IShortcutExecutor>(shortcutExecutor);

                pluginLoader.Load();

                //Load shortcuts
                internalMessageBus.Publish(new ShortcutsLoadingStarted());

                DataContext = new MainViewModel(internalMessageBus, shortcutCollection, shortcutExecutor, settingsRepository, mostUsedRepository);

                hookId = SetHook(lowLevelKeyboardProc);
                Closing += OnMainWindowClosing;

                internalMessageBus.Publish(new ApplicationLoadingCompleted());
            }
            catch (Exception exception)
            {
                internalMessageBus.Publish(new ApplicationLoadingFailed(exception));
                throw;
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
                internalMessageBus.Publish(globalKeyPressed);
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
