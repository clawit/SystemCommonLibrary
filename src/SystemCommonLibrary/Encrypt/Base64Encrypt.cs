using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Encrypt
{
    public static class Base64Encrypt
    {
        /// <summary>
        /// base64加密
        /// </summary>
        /// <param name="text">需要进行base64加密的字符串</param>
        /// <returns>返回经过base64加密后的字符串</returns>
        public static string ToBase64(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// 将字符串从base64格式解码
        /// </summary>
        /// <param name="code">需要解码的base64字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string FromBase64(string code)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(code));
            }
            catch
            {
                return null;
            }
        }
    }
}
