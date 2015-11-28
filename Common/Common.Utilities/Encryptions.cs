using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class Encryptions
    {
        internal const int KeyLength = 32;
        internal const int IVLength = 16;

        public static byte[] Encrypt(string key, string base64IV, string value)
        {
            return Encrypt(key, Convert.FromBase64String(base64IV), value);
        }
        public static byte[] Encrypt(string key, byte[] iv, string value)
        {
            byte[] useKey;
            GetByteKeys(key, out useKey);

            return Encrypt(useKey, iv, value);
        }
        public static byte[] Decrypt(string key, string base64IV, string value)
        {
            return Decrypt(key, Convert.FromBase64String(base64IV), value);
        }
        public static byte[] Decrypt(string key, byte[] iv, string value)
        {
            byte[] useKey;
            GetByteKeys(key, out useKey);

            return Decrypt(useKey, iv, value);
        }

        private static void GetByteKeys(string key, out byte[] useKey)
        {
            var bKey = Encoding.UTF8.GetBytes(key);

            useKey = new byte[KeyLength];

            for (byte i = 0; i < useKey.Length; i++)
            {
                useKey[i] = (byte)(i + 1);
            }

            Array.Copy(bKey, useKey, Math.Min(useKey.Length, bKey.Length));
        }

        public static byte[] Encrypt(byte[] key, byte[] iv, string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);

            return Encrypt(key, iv, bytes);
        }
        public static byte[] Encrypt(byte[] key, byte[] iv, byte[] bytes)
        {
            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var enc = aes.CreateEncryptor())
                {
                    using (var strm = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(strm, enc, CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);

                        }
                        return strm.ToArray();
                    }
                }
            }
        }
        public static byte[] Decrypt(byte[] key, byte[] iv, string base64Value)
        {
            var bytes = Convert.FromBase64String(base64Value);
            return Decrypt(key, iv, bytes);
        }
        public static byte[] Decrypt(byte[] key, byte[] iv, byte[] bytes)
        {
            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var enc = aes.CreateDecryptor())
                {
                    using (var strm = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(strm, enc, CryptoStreamMode.Write))
                        {
                            cs.Write(bytes, 0, bytes.Length);
                        }

                        return strm.ToArray();
                    }
                }
            }
        }

        public static byte[] GenerateIV()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[IVLength];
                rng.GetBytes(bytes);

                return bytes;
            }
        }
    }
}
