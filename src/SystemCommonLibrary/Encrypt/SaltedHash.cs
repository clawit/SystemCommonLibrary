using System;

namespace SystemCommonLibrary.Encrypt
{
    /// <summary>
    /// 加盐Hash处理
    /// </summary>
    public static class SaltedHash
    {
        /// <summary>
        /// 默认盐常量
        /// </summary>
        private const string _defaultSalt = "VFdwQmVFNVVRVFZOYWxVOQ==";

        /// <summary>
        /// 生成盐
        /// </summary>
        /// <returns></returns>
        public static string GenSalt()
        {
            return CodeGenerator.RndHash();
        }

        public static string GenSaltedHash(string text)
        {
            string saltedStr = text + _defaultSalt;
            return Md5Encrypt.Md5(saltedStr);
        }

        public static string GenSaltedHash(string text, string salt)
        {
            string saltedStr = text + salt;
            return Md5Encrypt.Md5(saltedStr);
        }

        /// <summary>
        /// 校验text是否被篡改
        /// </summary>
        /// <param name="text"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool Verify(string text, string code)
        {
            return code == GenSaltedHash(text);
        }

        /// <summary>
        /// 校验text是否被篡改
        /// </summary>
        /// <param name="text"></param>
        /// <param name="code"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool Verify(string text, string code, string salt)
        {
            return code == GenSaltedHash(text, salt);
        }

    }
}
