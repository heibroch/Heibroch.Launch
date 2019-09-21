using System;

namespace Heibroch.Launch
{
    public class Constants
    {
        public static string RootPath = Environment.GetEnvironmentVariable("APPDATA"/*"LocalAppData"*/) + "\\Heibroch\\";

        public static string ShortcutFileName = "Shortcuts";

        public static string ShortcutFileExtension = ".hscut";

        public static string SettingFileName = "Settings";

        public static string SettingFileExtension = ".hset";

        public const string CommandLineCommand = "[CMD]";

        public const string RemoteCommand = "[Remote]";

        public const string ApplicationName = "Heibroch.Launch";

        public const string SearchLocation = "[SearchPath]";

        public const string ReloadCommand = "Reload";

        public const int MaxResultCount = 10;

        public class ContextMenu
        {
            public const string Exit = "Exit";

            public const string Settings = "Settings";
        }
    }
}
