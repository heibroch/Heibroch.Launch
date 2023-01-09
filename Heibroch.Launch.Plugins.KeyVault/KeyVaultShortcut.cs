using Heibroch.Launch.Interfaces;

namespace Heibroch.Launch.Plugins.KeyVault
{
    public class KeyVaultShortcut : ILaunchShortcut
    {
        public Action<IEnumerable<KeyValuePair<string, string>>, string>? Execute { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
