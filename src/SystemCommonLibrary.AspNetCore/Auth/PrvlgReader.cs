using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using SystemCommonLibrary.Encrypt;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    /// <summary>
    /// Prvlg验证读取类
    /// </summary>
    public static class PrvlgReader
    {
        /// <summary>
        /// 从request请求中读取Token
        /// </summary>
        /// <returns></returns>
        public static PrivilegeIdentity Read(string privilege)
        {
            try
            {
                //将获取的标头值进行base64解码并转化为UTF-8
                privilege = Base64Encrypt.FromBase64(privilege);
                if (string.IsNullOrEmpty(privilege))
                    return null;

                string[] prvlgs = null;

                //获取Request请求表头信息
                if (!string.IsNullOrEmpty(privilege))
                    prvlgs = privilege.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                if (prvlgs == null || prvlgs.Length != 2)
                    return null;

                return new PrivilegeIdentity(prvlgs[0].Trim(), prvlgs[1].Trim());
            }
            catch
            {
                return null;
            }
        }
    }
}
