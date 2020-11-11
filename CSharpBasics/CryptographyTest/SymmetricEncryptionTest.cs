using System;
using System.IO;
using System.Security.Cryptography;

namespace CryptographyTest
{
    public class SymmetricEncryptionTest
    {
        public static void Run()
        {
            var message = "dotnetcoban";

            byte[] key, iv;
            using (Aes aesAlg = Aes.Create())
            {
                key = aesAlg.Key;
                iv = aesAlg.IV;
            }

            var encryptedMessage = SymmetricEncryption.Encrypt(message, key, iv);
            var decryptedMessage = SymmetricEncryption.Decrypt(encryptedMessage, key, iv);

            Console.WriteLine($"Original Message : {message}");
            Console.WriteLine($"Encrypted Message : {BitConverter.ToString(encryptedMessage).Replace("-", "")}");
            Console.WriteLine($"Decrypted Message : {decryptedMessage}");

        }
    }

    public class SymmetricEncryption
    {
        public static byte[] Encrypt(string message, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(message);
            }
            return ms.ToArray();
        }

        public static string Decrypt(byte[] cipher, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
