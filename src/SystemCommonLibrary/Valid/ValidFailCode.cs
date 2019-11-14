namespace SystemCommonLibrary.Network.Valid
{
    /// <summary>
    /// 验证结果错误代码
    /// </summary>
    public enum ValidFailCode
    {
        /// <summary>
        /// 空
        /// </summary>
        Empty,
        /// <summary>
        /// 空、或空白字串
        /// </summary>
        IsNullOrWhiteSpace,
        /// <summary>
        /// 格式错误
        /// </summary>
        WrongFormat,
        /// <summary>
        /// 长度错误
        /// </summary>
        WrongLength,
        /// <summary>
        /// 类型错误
        /// </summary>
        WrongType
    }
}
