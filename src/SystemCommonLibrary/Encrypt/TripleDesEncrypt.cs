using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SystemCommonLibrary.Encrypt
{
    /// <summary>
    /// TripleDesEncrypt加密处理类
    /// </summary>
    public static class TripleDesEncrypt
    {
        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <param name="key">加密的KEY</param>
        /// <param name="iv"></param>
        /// <returns>返回加密后的字符串</returns>
        public static string Encrypt3Des(string text, byte[] key = null, byte[] iv = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            if (key == null)
                key = _defaultKey;
            if (iv == null)
                iv = _defaultIv;

            return BitConverter.ToString(Encrypt3Des(bytes, key, iv));
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="code">需要解密的字符串</param>
        /// <param name="key">解密的KEY</param>
        /// <param name="iv"></param>
        /// <returns>返回解密后的字符串</returns>
        public static string Decrypt3Des(string code, byte[] key = null, byte[] iv = null)
        {
            byte[] bytes = Array.ConvertAll<string, byte>(code.Split('-'), s => Convert.ToByte(s, 16));
            if (key == null)
                key = _defaultKey;
            if (iv == null)
                iv = _defaultIv;

            return Encoding.UTF8.GetString(Decrypt3Des(bytes, key, iv));
        }



        /// <summary>
        /// string to encrypt string, use default key and iv
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text)
        {
            return Base64Encrypt.ToBase64(Encrypt3Des(text, _defaultKey, _defaultIv));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="code">需要解密的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string code)
        {
            return Decrypt3Des(Base64Encrypt.FromBase64(code), _defaultKey, _defaultIv);
        }

        #region internal
        private static readonly byte[] _defaultIv = new byte[]
                                                       {
                                                           120, 77, 48, 156,
                                                           64, 33 , 76, 28
                                                       };

        private static readonly byte[] _defaultKey = new byte[]
                                                        {
                                                            24, 78, 45, 32, 8, 255,
                                                            31, 85, 67, 5, 1, 67,
                                                            37, 86, 167, 90, 166,
                                                            138, 74, 50, 99, 37, 67
                                                            , 30
                                                        };

        private static readonly TripleDESCryptoServiceProvider Tdes = new TripleDESCryptoServiceProvider();

        /// <summary>
        /// byte[] to byte[], use custom key and iv
        /// </summary>
        /// <param name="bytes">明文</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns>密文</returns>
        private static byte[] Encrypt3Des(byte[] bytes, byte[] key, byte[] iv)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, Tdes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cryptoStream.Close();
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// byte[] to byte[], use custom key and iv
        /// </summary>
        /// <param name="cryptograph">密文</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns>明文</returns>
        private static byte[] Decrypt3Des(byte[] cryptograph, byte[] key, byte[] iv)
        {
            using (var s = new MemoryStream())
            {
                using (var decStream = new CryptoStream(s, Tdes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    decStream.Write(cryptograph, 0, cryptograph.Length);
                    decStream.FlushFinalBlock();
                    decStream.Close();
                }

                return s.ToArray();
            }
        }
        #endregion
    }
}
