using System;

namespace Heibroch.Launch
{
    public class Constants
    {
        public static string RootPath = Environment.GetEnvironmentVariable("APPDATA"/*"LocalAppData"*/) + "\\Heibroch\\";

        public static string ThemesPath = System.IO.Path.Combine(RootPath, "Themes");

        public const string ApplicationName = "Heibroch.Launch";

        public const string SearchLocation = "[SearchPath]";

        public const int MaxResultCount = 10;

        public class FileNames
        {
            public const string Settings = "Settings";
            public const string MostUsed = "MostUsed";
        }

        public class FileExtensions
        {
            public const string ShortcutFileExtension = ".hscut";
            public const string SettingFileExtension = ".hset";
            public const string MostUsedFileExtension = ".hmus";
        }

        public class ContextMenu
        {
            public const string Exit = "Exit";
            public const string Settings = "Settings";
        }

        public static class SettingNames
        {
            public const string Modifier1 = "Modifier1";
            public const string Modifier2 = "Modifier2";
            public const string Key = "Key";
            public const string Theme = "Theme";
            public const string UseStickySearch = "UseStickySearch";
            public const string ShowMostUsed = "ShowMostUsed";
            public const string LogInfo = "LogInfo";
            public const string LogWarnings = "LogWarnings";
            public const string LogErrors = "LogErrors";
        }
    }
}
