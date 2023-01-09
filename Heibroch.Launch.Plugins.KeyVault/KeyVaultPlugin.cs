using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using Heibroch.Launch.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Heibroch.Launch.Plugins.KeyVault
{
    public class KeyVaultPlugin : ILaunchPlugin
    {
        IInternalMessageBus internalMessageBus;
        private SortedList<string, (string encryptedValue, string authTag)> keyVaultKeysAndSecrets;
        private EncryptionUtility encryptionUtility;
        private string fullFileEncryptionKey = "this is my encryption key 12345!";
        private string rootPath;

        public KeyVaultPlugin(IInternalMessageBus internalMessageBus)
        {
            this.internalMessageBus = internalMessageBus;
            this.internalMessageBus.Subscribe<ApplicationLoadingCompleted>(OnApplicationLoadingCompleted);
            this.internalMessageBus.Subscribe<ShortcutsLoadingStarted>(OnShortcutsLoadingStarted);

            encryptionUtility = new EncryptionUtility();
            keyVaultKeysAndSecrets = new SortedList<string, (string encryptedValue, string authTag)>();            
        }

        private void OnShortcutsLoadingStarted(ShortcutsLoadingStarted obj) => PopulateShortcuts();

        private void OnApplicationLoadingCompleted(ApplicationLoadingCompleted obj)
        {
            //Read file
            rootPath = obj.RootPath;
            var keyVaultAuthTagPath = obj.RootPath + "Keys.hsvaulta";
            var keyVaultNoncePath = obj.RootPath + "Keys.hsvaultn";
            var keyVaultDataPath = obj.RootPath + "Keys.hsvaultd";

            keyVaultKeysAndSecrets = ReadFile(keyVaultDataPath, keyVaultAuthTagPath, keyVaultNoncePath);

            PopulateShortcuts();
        }

        private void PopulateShortcuts()
        {
            if (rootPath == null) return; //If application has not started yet.

            //Add secrets
            foreach (var keyVaultKeyAndSecret in keyVaultKeysAndSecrets)
            {
                internalMessageBus.Publish(new ShortcutAddingStarted(new KeyVaultShortcut
                {
                    Title = keyVaultKeyAndSecret.Key,
                    Description = "[Secret]",
                    Execute = (x, y) =>
                    {
                        var shortcutKeyBytes = GetShortcutValueKeyBytes(GetShortcutValueKeyString(keyVaultKeyAndSecret.Key));
                        var nonce = new byte[AesGcm.NonceByteSizes.MinSize];
                        var shortcutSecretBytes = Convert.FromBase64String(keyVaultKeyAndSecret.Value.encryptedValue);
                        var decryptedShortcutValue = encryptionUtility.Decrypt(shortcutSecretBytes, shortcutKeyBytes, nonce, Convert.FromBase64String(keyVaultKeyAndSecret.Value.authTag));
                        var decryptedShortcutString = Encoding.UTF8.GetString(decryptedShortcutValue);
                        TextCopy.ClipboardService.SetText(decryptedShortcutString);
                    }                    
                }));
            }

            //Add shortcuts for managing the secrets
            var keyVaultAuthTagPath = rootPath + "Keys.hsvaulta";
            var keyVaultNoncePath = rootPath + "Keys.hsvaultn";
            var keyVaultDataPath = rootPath + "Keys.hsvaultd";
            AddKeyVaultCommands(keyVaultDataPath, keyVaultAuthTagPath, keyVaultNoncePath);
        }

        private static string GetShortcutValueKeyString(string key) => $"hei{key}b0ch secret keys for lyhfe boiii!".Substring(0, 32);
        private static byte[] GetShortcutValueKeyBytes(string shortcutValueKey) => Encoding.UTF8.GetBytes(shortcutValueKey);

        private void AddKeyVaultCommands(string keyVaultDataPath, string keyVaultAuthTagPath, string keyVaultNoncePath)
        {
            //Add the shortcut to add keys from the vault
            internalMessageBus.Publish(new ShortcutAddingStarted(new KeyVaultShortcut
            {
                Title = "Add to key vault",
                Description = "[Arg=Key] and the value to be the [Arg=Secret]",
                Execute = (x, y) =>
                {
                    var arguments = x.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);                    
                    var decryptedValueBytes = Encoding.UTF8.GetBytes(arguments["Secret"]);
                    var secretKeyString = GetShortcutValueKeyString(arguments["Key"]);
                    var secretKeyBytes = GetShortcutValueKeyBytes(secretKeyString);
                    var nonce = new byte[AesGcm.NonceByteSizes.MinSize];

                    var encryptedValueBytes = encryptionUtility.Encrypt(decryptedValueBytes, secretKeyBytes, nonce, out var authTag);

                    var authTagString = Convert.ToBase64String(authTag);
                    var encryptedValueString = Convert.ToBase64String(encryptedValueBytes);

                    //Add to collection
                    keyVaultKeysAndSecrets.Add(arguments["Key"], (encryptedValueString, authTagString));

                    //Replace file
                    SaveFile(keyVaultDataPath, keyVaultAuthTagPath, keyVaultNoncePath, keyVaultKeysAndSecrets);

                    //Re-populate shortcuts
                    internalMessageBus.Publish(new ShortcutsLoadingStarted());
                },
            }));

            //Add the shortcut to remove keys from the vault
            internalMessageBus.Publish(new ShortcutAddingStarted(new KeyVaultShortcut
            {
                Title = "Remove from key vault",
                Description = "[Arg=Key]",
                Execute = (x, y) =>
                {
                    if (!keyVaultKeysAndSecrets.ContainsKey(y))
                        return;
                                        
                    keyVaultKeysAndSecrets.Remove(y);

                    //Replace file
                    SaveFile(keyVaultDataPath, keyVaultAuthTagPath, keyVaultNoncePath, keyVaultKeysAndSecrets);

                    //Re-populate shortcuts
                    internalMessageBus.Publish(new ShortcutsLoadingStarted());
                },
            }));
        }

        private void SaveFile(string keyVaultDataPath, string keyVaultAuthTagPath, string keyVaultNoncePath, SortedList<string, (string encryptedValue, string authTag)> keyVaultKeysAndSecrets)
        {  
            var fullString = new StringBuilder();

            foreach (var keyVaultKeyAndSecret in keyVaultKeysAndSecrets)
            {
                fullString.AppendLine(keyVaultKeyAndSecret.Key);
                fullString.AppendLine(keyVaultKeyAndSecret.Value.encryptedValue);
                fullString.AppendLine(keyVaultKeyAndSecret.Value.authTag);
            }

            var nonce = new byte[AesGcm.NonceByteSizes.MinSize];

            var encryptedBytes = encryptionUtility.Encrypt(
                Encoding.UTF8.GetBytes(fullString.ToString()),
                Encoding.UTF8.GetBytes(fullFileEncryptionKey),
                nonce,
                out var createdAuthTag);

            File.WriteAllBytes(keyVaultDataPath, encryptedBytes);
            File.WriteAllBytes(keyVaultAuthTagPath, createdAuthTag);
            File.WriteAllBytes(keyVaultNoncePath, nonce);
        }

        private SortedList<string, (string encryptedValue, string authTag)> ReadFile(string keyVaultDataPath, string keyVaultAuthTagPath, string keyVaultNoncePath)
        {
            var keysAndValues = new SortedList<string, (string encryptedValue, string authTag)>();
            if (!File.Exists(keyVaultDataPath) || !File.Exists(keyVaultAuthTagPath) || !File.Exists(keyVaultNoncePath))
                return keysAndValues;

            var keyVaultDataBytes = File.ReadAllBytes(keyVaultDataPath);
            var keyVaultAuthTagBytes = File.ReadAllBytes(keyVaultAuthTagPath);
            var keyVaultNonceBytes = File.ReadAllBytes(keyVaultNoncePath);

            var encryptionKeyBytes = Encoding.UTF8.GetBytes(fullFileEncryptionKey);
            var decryptedBytes = encryptionUtility.Decrypt(keyVaultDataBytes, encryptionKeyBytes, keyVaultNonceBytes, keyVaultAuthTagBytes);
            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);
                        
            var decryptedLines = decryptedString.Split(Environment.NewLine);
            for (int i = 0; i < decryptedLines.Length; i+=3)
            {
                if (i + 2 >= decryptedLines.Length) break;
                keysAndValues.Add(decryptedLines[i], (decryptedLines[i + 1], decryptedLines[i + 2]));
            }

            return keysAndValues;
        }

        public string? ShortcutFilter => null;

        public ILaunchShortcut? CreateShortcut(string title, string description) => null;
    }
}
