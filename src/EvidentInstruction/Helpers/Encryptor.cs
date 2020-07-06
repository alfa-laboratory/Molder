using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace EvidentInstruction.Helpers
{
    /// <summary>
    /// Тесты проверки credentials.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Encryptor
    {
        private static SecureString _salt = new SecureString();


        public static void SetSalt(string salt)
        {
            _salt = ToSecureString(salt);
        }

        public static string Encrypt(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var cryptoServiceProvider1 = new MD5CryptoServiceProvider();
            var hash =
                cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(GetOriginalString(_salt).ToCharArray()));
            cryptoServiceProvider1.Clear();
            var cryptoServiceProvider2 = new AesCryptoServiceProvider
            {
                Key = hash, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7
            };
            var encryptor = cryptoServiceProvider2.CreateEncryptor();
            var inArray = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            encryptor.Dispose();
            cryptoServiceProvider2.Clear();
            return Convert.ToBase64String(inArray);
        }

        public static string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var inputBuffer = Convert.FromBase64String(input);
            var cryptoServiceProvider1 = new MD5CryptoServiceProvider();
            var hash =
                cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(GetOriginalString(_salt).ToCharArray()));
            cryptoServiceProvider1.Clear();
            var cryptoServiceProvider2 = new AesCryptoServiceProvider
            {
                Key = hash, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7
            };
            var decryptor = cryptoServiceProvider2.CreateDecryptor();
            var bytes = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            decryptor.Dispose();
            cryptoServiceProvider2.Clear();
            return Encoding.UTF8.GetString(bytes);
        }

        private static SecureString ToSecureString(string str)
        {
            var secureString = new SecureString();
            if (str != null)
            {
                Array.ForEach(str.ToCharArray(), secureString.AppendChar);
            }

            secureString.MakeReadOnly();
            return secureString;
        }

        private static string GetOriginalString(SecureString secStr)
        {
            var num = IntPtr.Zero;
            try
            {
                num = Marshal.SecureStringToGlobalAllocUnicode(secStr);
                return Marshal.PtrToStringUni(num);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(num);
            }
        }
    }
}
