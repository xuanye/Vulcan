using System;
using System.Security.Cryptography;
using System.Text;

namespace Vulcan.Core
{
    /// <summary>
    /// </summary>
    public class CryptographyManager
    {
        private static readonly byte[] _slat =
        {
            83,
            110,
            100,
            97,
            32,
            67,
            82,
            77,
            32,
            88,
            117,
            97,
            110,
            121,
            101
        };

        public static string Md5Encrypt(string encryptingString)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(encryptingString));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = Md5Encrypt(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string AESEncrypt(string toEncrypt, string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, _slat, 1024);
            return AESEncrypt(toEncrypt, rfc2898DeriveBytes.GetBytes(32), rfc2898DeriveBytes.GetBytes(16));
        }

        public static string AESEncrypt(string toEncrypt, byte[] keyArray, byte[] ivArray)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            var cryptoTransform = Aes.Create().CreateEncryptor(keyArray, ivArray);

            byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(array, 0, array.Length);
        }

        public static Rfc2898DeriveBytes RFCDB(string password)
        {
            return new Rfc2898DeriveBytes(password, _slat, 1024);
        }

        public static string AESDecrypt(string toDecrypt, string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, _slat, 1024);
            return AESDecrypt(toDecrypt, rfc2898DeriveBytes.GetBytes(32), rfc2898DeriveBytes.GetBytes(16));
        }

        public static string AESDecrypt(string toDecrypt, byte[] keyArray, byte[] ivArray)
        {
            byte[] array = Convert.FromBase64String(toDecrypt);
            var cryptoTransform = Aes.Create().CreateEncryptor(keyArray, ivArray);

            byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            string @string = Encoding.UTF8.GetString(bytes);
            return @string.Replace("\0", "");
        }
    }
}
