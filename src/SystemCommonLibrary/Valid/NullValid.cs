namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 空对象检查
    /// </summary>
    public static class NullValid
    {
        /// <summary>
        /// 空对象校验
        /// </summary>
        /// <param name="value">需要校验的对象</param>
        /// <returns>返回校验结果</returns>
        public static ValidResult CheckNull(object value)
        {
            //校验对象
            if (value == null)
                return new ValidResult(false, ValidFailCode.IsNullOrWhiteSpace);
            else
                return ValidResult.Success;
        }
    }
}
