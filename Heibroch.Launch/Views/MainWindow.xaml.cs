using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Heibroch.Common;
using Heibroch.Launch.Events;
using Heibroch.Launch.Plugin;
using Heibroch.Launch.ViewModels;

namespace Heibroch.Launch.Views
{
    public partial class MainWindow : Window
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc proc = HookCallback;
        private static IntPtr hookId = IntPtr.Zero;
        private static IEventBus eventBus;
        private IPluginLoader pluginLoader;
                
        public MainWindow()
        {
            InitializeComponent();
            
            eventBus = new EventBus();
            Container.Current.Register<IEventBus>(eventBus);

            pluginLoader = new PluginLoader();
            Container.Current.Register<IPluginLoader>(pluginLoader);
            
            var shortcutCollection = new ShortcutCollection(pluginLoader);
            Container.Current.Register<IShortcutCollection<string, ILaunchShortcut>>(shortcutCollection);

            var shortcutExecutor = new ShortcutExecutor(shortcutCollection, pluginLoader);
            Container.Current.Register<IShortcutExecutor>(shortcutExecutor);

            var settingCollection = new SettingCollection();
            Container.Current.Register<ISettingCollection>(settingCollection);

            pluginLoader.Load();

            foreach (var plugin in pluginLoader.Plugins)
            {
                try
                {
                    plugin.OnProgramLoaded();
                }
                catch (Exception exception)
                {
                    System.Windows.MessageBox.Show($"{exception.ToString()}\r\n{exception.StackTrace}");
                }                
            }

            DataContext = new MainViewModel();
            
            hookId = SetHook(proc);            
            Closing += OnMainWindowClosing;
        }

        private void OnTrayIconDoubleClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnTrayIconClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { UnhookWindowsHookEx(hookId); }
            catch { }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
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
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
