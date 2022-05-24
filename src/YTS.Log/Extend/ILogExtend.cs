using System.Collections.Generic;

namespace YTS.Log
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

        /// <summary>
        /// 创建默认的参数字典
        /// </summary>
        /// <typeparam name="TKey">字典键类型</typeparam>
        /// <typeparam name="TValue">字典值类型</typeparam>
        /// <param name="log">键值类型日志对象</param>
        /// <returns>空字典</returns>
        public static IDictionary<TKey, TValue> CreateArgDictionary<TKey, TValue>(this ILog<IDictionary<TKey, TValue>> log)
        {
            return new Dictionary<TKey, TValue>();
        }
    }
}