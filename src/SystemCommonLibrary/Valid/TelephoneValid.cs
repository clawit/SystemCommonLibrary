using System.Text.RegularExpressions;

namespace SystemCommonLibrary.Network.Valid
{
    public class TelephoneValid
    {
        /// <summary>
        /// 验证是否是正确的固定电话，返回区号和号码
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="areaCode"></param>
        /// <param name="landline"></param>
        /// <returns></returns>
        public static ValidResult IsValidPhone(string tel, out string areaCode, out string landline)
        {
            areaCode = string.Empty;
            landline = string.Empty;
            if (string.IsNullOrEmpty(tel))
            {
                return new ValidResult(false, ValidFailCode.IsNullOrWhiteSpace, "手机号为空");
            }
            var reg = new Regex(@"^(?:(0(?:10|2[0-57-9]|[3-9]\d{2}))[-—]?)(\d{7,8})$");
            var match = reg.Match(tel);
            if (match.Success)
            {
                areaCode = match.Groups[1].Value;
                landline = match.Groups[2].Value;
                return ValidResult.Success;
            }
            else
                return new ValidResult(false, ValidFailCode.WrongFormat, "手机格式错误");
        }
    }
}
