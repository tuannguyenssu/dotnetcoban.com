using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyTest
{
    public class HashingTest
    {
        public static void Run()
        {
            var message = "dotnetcoban";
            Console.WriteLine(HashUtil.ComputeHash(message));
        }
    }

    public static class HashUtil
    {
        public static string ComputeHash(string message)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }
}
