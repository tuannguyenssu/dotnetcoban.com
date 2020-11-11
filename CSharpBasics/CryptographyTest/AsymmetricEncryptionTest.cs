using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyTest
{
    public class AsymmetricEncryptionTest
    {
        public static void Run()
        {
            var message = "dotnetcoban";

            RSAParameters privateAndPublicKeys, publicKeyOnly;
            using (var rsaAlg = RSA.Create())
            {
                privateAndPublicKeys = rsaAlg.ExportParameters(includePrivateParameters: true);
                publicKeyOnly = rsaAlg.ExportParameters(includePrivateParameters: false);
            }

            var encryptedMessage = AsymmetricEncryption.Encrypt(message, publicKeyOnly);
            var decryptedMessage = AsymmetricEncryption.Decrypt(encryptedMessage, privateAndPublicKeys);
            Console.WriteLine("Encrypt - Decrypt");
            Console.WriteLine($"Original Message : {message}");
            Console.WriteLine($"Encrypted Message : {BitConverter.ToString(encryptedMessage).Replace("-", "")}");
            Console.WriteLine($"Decrypted Message : {decryptedMessage}");

            Console.WriteLine();

            var messageSignature = AsymmetricEncryption.Sign(message, privateAndPublicKeys);
            var isVerified = AsymmetricEncryption.Verify(message, messageSignature, publicKeyOnly);
            

            Console.WriteLine("Sign - Verify");
            Console.WriteLine($"Original Message : {message}");
            Console.WriteLine($"Signature Message : {BitConverter.ToString(messageSignature).Replace("-", "")}");
            Console.WriteLine($"Verified Message : {isVerified}");
        }
    }

    public static class AsymmetricEncryption
    {
        public static byte[] Encrypt(string message, RSAParameters rsaParameters)
        {
            using var rsa = RSA.Create(rsaParameters);
            return rsa.Encrypt(Encoding.UTF8.GetBytes(message), RSAEncryptionPadding.Pkcs1);
        }

        public static string Decrypt(byte[] cipherText, RSAParameters rsaParameters)
        {
            using var rsa = RSA.Create(rsaParameters);
            var decryptedMessage = rsa.Decrypt(cipherText, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedMessage);
        }

        public static byte[] Sign(string message, RSAParameters rsaParameters)
        {
            using var rsa = RSA.Create(rsaParameters);
            return rsa.SignData(Encoding.UTF8.GetBytes(message), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        public static bool Verify(string message, byte[] signature, RSAParameters rsaParameters)
        {
            using var rsa = RSA.Create(rsaParameters);
            return rsa.VerifyData(Encoding.UTF8.GetBytes(message), signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
