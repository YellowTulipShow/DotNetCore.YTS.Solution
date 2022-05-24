using System;
using System.Collections.Generic;

namespace YTS.Log
{
    /// <summary>
    /// 连接日志
    /// </summary>
    internal sealed class ConnectLog : ILog
    {
        private readonly IDictionary<int, ILog> logDict;

        /// <summary>
        /// 初始化实现
        /// </summary>
        public ConnectLog()
        {
            logDict = new Dictionary<int, ILog>();
        }

        /// <summary>
        /// 增加日志需要连接的日志目标
        /// </summary>
        /// <param name="log">日志目标</param>
        public void Add(ILog log)
        {
            int code = log.GetHashCode();
            if (!logDict.ContainsKey(code))
            {
                logDict.Add(code, log);
            }
        }

        /// <inheritdoc />
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, args);
            }
        }

        /// <inheritdoc />
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, args);
            }
        }

        /// <inheritdoc />
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, ex, args);
            }
        }
    }
}
