using YTS.Tools;

namespace YTS.WEBAPI
{
    /// <summary>
    /// 结果状态代码
    /// </summary>
    [Explain(@"结果状态代码")]
    public enum ResultStatueCode
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        [Explain(@"执行成功")]
        OK = 0,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Explain(@"参数错误")]
        ParameterError = 1,

        /// <summary>
        /// 意外的异常
        /// </summary>
        [Explain(@"意外的异常")]
        UnexpectedException = 2,

        /// <summary>
        /// 执行逻辑错误
        /// </summary>
        [Explain(@"执行逻辑错误")]
        LogicError = 3,
    }
}
