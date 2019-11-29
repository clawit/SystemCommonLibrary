using System.Text.RegularExpressions;

namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 手机号校验
    /// </summary>
    public static class MobileValid
    {
        /// <summary>
        /// 校验是否是正确的手机号码
        /// </summary>
        /// <param name="mobile">需要校验的手机号码</param>
        /// <returns>返回校验结果</returns>
        public static ValidResult IsMobile(string mobile)
        {
            //检查是否为空
            var strCheck = StringValid.IsEmpty(mobile);
            if (strCheck)
            {
                //检查是否符合正则格式
                var regex = new Regex(@"^1[3-9]\d{9}$");
                if (regex.IsMatch(mobile))
                    return ValidResult.Success;
                else
                    return new ValidResult(false, ValidFailCode.WrongFormat, "手机格式错误");
            }
            else
                return strCheck.WithMessage("手机号为空");

        }
    }
}
