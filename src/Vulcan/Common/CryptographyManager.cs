using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Vulcan.Common
{
    public class CryptographyManager
    {
        private static byte[] _slat = new byte[]
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
            return FormsAuthentication.HashPasswordForStoringInConfigFile(encryptingString, "md5");
        }
        public static string AESEncrypt(string toEncrypt, string password)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, CryptographyManager._slat, 1024);
            return CryptographyManager.AESEncrypt(toEncrypt, rfc2898DeriveBytes.GetBytes(32), rfc2898DeriveBytes.GetBytes(16));
        }
        public static string AESEncrypt(string toEncrypt, byte[] keyArray, byte[] ivArray)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Key = keyArray,
                IV = ivArray,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            }.CreateEncryptor();
            byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(array, 0, array.Length);
        }
        public static Rfc2898DeriveBytes RFCDB(string password)
        {
            return new Rfc2898DeriveBytes(password, CryptographyManager._slat, 1024);
        }
        public static string AESDecrypt(string toDecrypt, string password)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, CryptographyManager._slat, 1024);
            return CryptographyManager.AESDecrypt(toDecrypt, rfc2898DeriveBytes.GetBytes(32), rfc2898DeriveBytes.GetBytes(16));
        }
        public static string AESDecrypt(string toDecrypt, byte[] keyArray, byte[] ivArray)
        {
            byte[] array = Convert.FromBase64String(toDecrypt);
            ICryptoTransform cryptoTransform = new RijndaelManaged
            {
                Key = keyArray,
                IV = ivArray,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            }.CreateDecryptor();
            byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            string @string = Encoding.UTF8.GetString(bytes);
            return @string.Replace("\0", "");
        }
    }
}
