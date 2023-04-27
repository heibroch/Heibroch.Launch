using Heibroch.Launch.Plugins.KeyVault;
using System.Text;
using Xunit;

namespace Heibroch.Launch.Plugins.KeyVaultTests
{
    public class EncryptionUtilityTests
    {
        [Fact]
        public void TestEncryptionAndDecryption()
        {
            var target = new EncryptionUtility();

            var originalDecryptedString = "bleeeequrgh12!";
            var encryptionKey = "SomeEncryptionKey111111222233333";

            var originalDecryptedBytes = Encoding.UTF8.GetBytes(originalDecryptedString);

            var nonce = new byte[8];

            //Act - Encrypt
            var encryptedBytes = target.Encrypt(originalDecryptedBytes, Convert.FromBase64String(encryptionKey), nonce, out var authTag);
            var encryptedString = Encoding.UTF8.GetString(encryptedBytes);

            //Act - Decrypt
            var decryptedBytes = target.Decrypt(encryptedBytes, Convert.FromBase64String(encryptionKey), nonce, authTag);
            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            //Assert
            Assert.Equal(originalDecryptedString, decryptedString);
            Assert.NotEqual(originalDecryptedString, encryptedString);
        }
    }
}
