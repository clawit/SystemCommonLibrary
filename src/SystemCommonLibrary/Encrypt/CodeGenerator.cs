using System;

namespace SystemCommonLibrary.Encrypt
{
    /// <summary>
    /// 生成加密Hash
    /// </summary>
    public static class CodeGenerator
    {
        /// <summary>
        /// 生成随机Hash
        /// </summary>
        /// <returns></returns>
        public static string RndHash()
        {
            var c = new char[8];
            //生成Hash的源字符串
            const string randomchars = "ABCDEF23WXY45GHK67MNPQRS89TV";
            var maxValue = randomchars.Length - 1;
            //循环字符串
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = randomchars[new Random(Guid.NewGuid().GetHashCode()).Next(0, maxValue)];
            }
            //返回格式化后的hash
            return string.Format(new string(c));
        }
    }
}
