using System;
using System.Security.Cryptography;
using System.Text;

public class KeyGenerationService
{
    public (string encryptionKey, string iv) GenerateEncryptionKey()
    {
        // Generate a 128-bit (16 bytes) AES encryption key.
        using (var aes = Aes.Create())
        {
            aes.KeySize = 128;
            aes.GenerateKey();
            aes.GenerateIV();

            // Convert the key and IV to a hex string for easy use in HLS encryption.
            string encryptionKey = BitConverter.ToString(aes.Key).Replace("-", "").ToLower(); // Hex string of the encryption key
            string iv = BitConverter.ToString(aes.IV).Replace("-", "").ToLower(); // Hex string of the IV

            return (encryptionKey, iv);
        }
    }
}
