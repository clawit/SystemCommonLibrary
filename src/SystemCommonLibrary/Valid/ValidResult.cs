namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 校验结果类
    /// </summary>
    public class ValidResult
    {
        public ValidResult(bool correct, ValidFailCode code = ValidFailCode.Empty, string message = null)
        {
            Correct = correct;
            Message = message;
            Code = code;
        }

        private static ValidResult _success = new ValidResult(true);
        /// <summary>
        /// 验证通过
        /// </summary>
        public static ValidResult Success { get { return _success; } }
        public bool Correct { get; set; }
        /// <summary>
        /// 返回结果信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 验证结果错误代码
        /// </summary>
        public ValidFailCode Code { get; set; }
    }

    public static class ValidResultExt
    {
        public static ValidResult WithMessage(this ValidResult result, string message)
        {
            result.Message = message;
            return result;
        }
    }
}
