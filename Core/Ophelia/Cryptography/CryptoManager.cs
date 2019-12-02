﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Cryptography
{
    public static class CryptoManager
    {
        public static string Encrypt(string chipperText, string encryptionKey = "")
        {
            try
            {
                if (string.IsNullOrEmpty(chipperText))
                    return chipperText;
                if (string.IsNullOrEmpty(encryptionKey))
                    encryptionKey = ConfigurationManager.EncryptKey;
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                DES.Key = hashMD5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(encryptionKey));
                DES.Mode = CipherMode.ECB;
                ICryptoTransform Encryptor = DES.CreateEncryptor();
                byte[] Buffer = System.Text.Encoding.ASCII.GetBytes(chipperText);
                return Convert.ToBase64String(Encryptor.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch
            {
                return chipperText;
            }
        }
        public static string Decrypt(string richText, string decryptionKey = "")
        {
            try
            {
                if (string.IsNullOrEmpty(richText))
                    return richText;
                if (string.IsNullOrEmpty(decryptionKey))
                    decryptionKey = ConfigurationManager.EncryptKey;
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                DES.Key = hashMD5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(decryptionKey));
                DES.Mode = CipherMode.ECB;
                ICryptoTransform Decryptor = DES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(richText);
                return System.Text.Encoding.ASCII.GetString(Decryptor.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch
            {
                return richText;
            }
        }
        public static string ComputeHash(string plainText, string saltText = "", HashAlgorithms algorithm = HashAlgorithms.SHA1, Encoding encoding = null)
        {
            try
            {
                if (string.IsNullOrEmpty(plainText)) return plainText;
                if (encoding == null) encoding = Encoding.UTF8;
                HashAlgorithm hashProvider = null;
                switch (algorithm)
                {
                    default:
                    case HashAlgorithms.SHA1:
                        hashProvider = new SHA1CryptoServiceProvider(); break;
                    case HashAlgorithms.SHA256:
                        hashProvider = new SHA256CryptoServiceProvider(); break;
                    case HashAlgorithms.SHA384:
                        hashProvider = new SHA384CryptoServiceProvider(); break;
                    case HashAlgorithms.SHA512:
                        hashProvider = new SHA512CryptoServiceProvider(); break;
                    case HashAlgorithms.MD5:
                        hashProvider = new MD5CryptoServiceProvider(); break;
                    case HashAlgorithms.RIPEMD160:
                        hashProvider = new RIPEMD160Managed(); break;
                }

                string hashedText = plainText + saltText;
                byte[] hashbytes = encoding.GetBytes(hashedText);
                byte[] inputbytes = hashProvider.ComputeHash(hashbytes);
                hashProvider.Clear();
                return CryptoHelper.GetHexaDecimal(inputbytes);
            }
            catch { return plainText; }
        }
        public static string GetMd5Hash(string plainText, string saltText = "")
        {
            return ComputeHash(plainText, saltText, HashAlgorithms.MD5);
        }
        public static string GetSHA512Hash(string plainText, string saltText = "")
        {
            return ComputeHash(plainText, saltText, HashAlgorithms.SHA512);
        }
    }
}
