using System;
using System.Collections.Generic;

namespace YTS.Log
{
    /// <summary>
    /// 接口: 日志
    /// </summary>
    public interface ILog : ILog<IDictionary<string, object>>
    {
    }

    /// <summary>
    /// 接口: 泛型日志
    /// </summary>
    /// <typeparam name="T">参数数据类型</typeparam>
    public interface ILog<T>
    {
        /// <summary>
        /// 常规信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        void Info(string message, params T[] args);

        /// <summary>
        /// 错误信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        void Error(string message, params T[] args);

        /// <summary>
        /// 错误信息和异常日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="ex">异常</param>
        /// <param name="args">参数队列</param>
        void Error(string message, Exception ex, params T[] args);
    }
}