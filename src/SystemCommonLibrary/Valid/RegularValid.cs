using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 正则表达式验证
    /// </summary>
    public static class RegularValid
    {

        /// <summary>
        /// 检验是否匹配
        /// </summary>
        /// <param name="regularTxt">正则表达式</param>
        /// <param name="validText">需验证内容</param>
        /// <returns></returns>
        public static ValidResult IsMatch(string regularTxt, string validText)
        {
            //检查是否符合正则格式
            var regex = new Regex(regularTxt);
            if (regex.IsMatch(validText))
                return ValidResult.Success;
            else
                return new ValidResult(false, ValidFailCode.WrongFormat, "格式错误");

        }

    }
}
