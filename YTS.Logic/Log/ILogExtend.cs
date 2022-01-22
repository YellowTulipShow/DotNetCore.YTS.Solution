using System;
using System.Collections.Generic;

namespace YTS.Logic.Log
{
    /// <summary>
    /// 接口: 日志
    /// </summary>
    public static class ILogExtend
    {
        internal static ConnectLog connectLog;

        /// <summary>
        /// 连接多个不同的日志接口实现, 返回新的可同时执行的日志执行
        /// </summary>
        /// <param name="log">需要连接的实现</param>
        /// <param name="logs">多个连接对象</param>
        /// <returns>连接结果</returns>
        public static ILog Connect(this ILog log, params ILog[] logs)
        {
            if (connectLog == null)
                connectLog = new ConnectLog();
            if (!(log is ConnectLog))
            {
                connectLog.Add(log);
            }
            foreach (var item in logs)
            {
                if (!(item is ConnectLog))
                {
                    connectLog.Add(item);
                }
            }
            return connectLog;
        }
    }
}