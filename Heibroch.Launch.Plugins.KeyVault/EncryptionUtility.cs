using System.Security.Cryptography;

namespace Heibroch.Launch.Plugins.KeyVault
{
    public class EncryptionUtility
    {
        public byte[] Encrypt(byte[] decryptedBytes, byte[] encryptionKeyBytes, byte[] nonce, out byte[] authTag)
        {
            var aesGcm = new AesGcm(encryptionKeyBytes);
            var encryptedBytes = new byte[decryptedBytes.Length];            
            authTag = new byte[AesGcm.TagByteSizes.MinSize];
            aesGcm.Encrypt(nonce, decryptedBytes, encryptedBytes, authTag, null);
            return encryptedBytes;
        }

        public byte[] Decrypt(byte[] encryptedBytes, byte[] encryptionKey, byte[] nonce, byte[] authTag)
        {
            var aesGcm = new AesGcm(encryptionKey);
            var decryptedBytes = new byte[encryptedBytes.Length];            
            aesGcm.Decrypt(nonce, encryptedBytes, authTag, decryptedBytes, null);
            return decryptedBytes;
        }
    }
}
