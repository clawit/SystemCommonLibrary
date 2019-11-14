using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using SystemCommonLibrary.Encrypt;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    /// <summary>
    /// token验证读取类
    /// </summary>
    public static class AuthReader
    {
        /// <summary>
        /// 从request请求中读取Token
        /// </summary>
        /// <param name="authorization">request中Authorization</param>
        /// <returns></returns>
        public static TokenAuthIdentity Read(string authorization)
        {
            string authParameter = null;

            //获取Request请求表头信息
            if (!string.IsNullOrEmpty(authorization)
                && authorization.StartsWith("Token")
                && authorization.IndexOf(" ") != -1)
                authParameter = authorization.Substring(authorization.IndexOf(" "));

            if (string.IsNullOrEmpty(authParameter))
                return null;

            //将获取的标头值进行base64解码并转化为UTF-8
            authParameter = Base64Encrypt.FromBase64(authParameter);
            if (string.IsNullOrEmpty(authParameter))
                return null;

            var authToken = authParameter.Split(':');
            if (authToken.Length < 2)
                return null;

            int id = 0;
            if (!Int32.TryParse(authToken[0], out id)
                && id != 0)
                return null;
            else
                return new TokenAuthIdentity(id, authToken[1]);
        }
    }
}
