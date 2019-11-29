using System.Text.RegularExpressions;

namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 密码校验
    /// </summary>
    public static class PasswordValid
    {
        /// <summary>
        /// 校验登录密码
        /// </summary>
        /// <param name="pwd">需要进行校验的密码字符串</param>
        /// <returns>返回校验结果</returns>
        public static ValidResult IsStrongPassword(string pwd)
        {
            //如果密码字符串空校验通过
            if (StringValid.IsEmpty(pwd))
            {
                //密码长度必须在6~20位之间
                if (pwd.Length < 6 || pwd.Length > 20)
                {
                    return new ValidResult(false, ValidFailCode.WrongLength, "密码长度必须在6-20位");
                }

                //密码不能包含中文正则校验
                if (new Regex(@"[\u4e00-\u9fa5]").IsMatch(pwd))
                {
                    return new ValidResult(false, ValidFailCode.WrongFormat, "不能包含中文");
                }

                //密码不能为纯数字或字母校验
                if (new Regex(@"^([0-9]+|[a-zA-Z]+)$").IsMatch(pwd))
                {
                    return new ValidResult(false, ValidFailCode.WrongFormat, "密码不能为纯数字或纯字母");
                }
                return ValidResult.Success;
            }
            else
            {
                return new ValidResult(false, ValidFailCode.IsNullOrWhiteSpace, "密码不能为空");
            }
        }
    }
}
