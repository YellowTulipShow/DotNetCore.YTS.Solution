using System;
using System.Collections.Generic;
using System.Text;

namespace YTS.Logic.Log
{
    /// <summary>
    /// 连接日志
    /// </summary>
    internal sealed class ConnectLog : ILog
    {
        private readonly IDictionary<int, ILog> logDict;
        public ConnectLog()
        {
            logDict = new Dictionary<int, ILog>();
        }

        public void Add(ILog log)
        {
            int code = log.GetHashCode();
            if (!logDict.ContainsKey(code))
            {
                logDict.Add(code, log);
            }
        }

        /// <summary>
        /// 常规信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, args);
            }
        }

        /// <summary>
        /// 错误信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, args);
            }
        }

        /// <summary>
        /// 错误信息和异常日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="ex">异常</param>
        /// <param name="args">参数队列</param>
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, ex, args);
            }
        }
    }
}
