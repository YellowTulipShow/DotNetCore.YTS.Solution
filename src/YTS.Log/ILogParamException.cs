using System;
using System.Collections.Generic;

namespace YTS.Log
{
    /// <summary>
    /// 携带日志接口参数的异常
    /// </summary>
    public class ILogParamException : Exception
    {
        private readonly IDictionary<string, object> param;

        /// <summary>
        /// 实例化日志参数异常
        /// </summary>
        /// <param name="param">日志参数</param>
        /// <param name="message">解释异常原因的错误消息。</param>
        public ILogParamException(IDictionary<string, object> param, string message) : base(message)
        {
            this.param = param;
        }
        /// <summary>
        /// 实例化日志参数异常
        /// </summary>
        /// <param name="param">日志参数</param>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用（在 Visual Basic 中为 Nothing）。</param>
        public ILogParamException(IDictionary<string, object> param, string message, Exception innerException) : base(message, innerException)
        {
            this.param = param;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public IDictionary<string, object> GetParam() => param;
    }
}
