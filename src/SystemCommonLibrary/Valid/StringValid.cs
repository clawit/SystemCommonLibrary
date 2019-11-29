namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 字符串验证类
    /// </summary>
    public static class StringValid
    {
        /// <summary>
        /// 校验源字符串是否为空
        /// </summary>
        /// <param name="str">需要校验的字符串</param>
        /// <returns>返回验证结果</returns>
        public static ValidResult IsEmpty(string str)
        {
            //如果是null、空白字符、空串则返回校验结果失败
            if (string.IsNullOrWhiteSpace(str))
                return new ValidResult(false, ValidFailCode.IsNullOrWhiteSpace);
            else
                return ValidResult.Success;
        }
    }
}
