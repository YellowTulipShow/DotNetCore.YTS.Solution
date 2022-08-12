using System;
using System.IO;
using System.Collections.Generic;

namespace YTS.Log
{
    /// <summary>
    /// 日志接口扩展方法库
    /// </summary>
    public static class ILogExtend
    {
        /// <summary>
        /// 连接多个不同的日志接口实现, 返回新的可同时执行的日志执行
        /// </summary>
        /// <param name="log">需要连接的实现</param>
        /// <param name="logs">多个连接对象</param>
        /// <returns>连接结果</returns>
        public static ILog Connect(this ILog log, params ILog[] logs)
        {
            ConnectLog clog = null;
            IList<ILog> loglist = new List<ILog>(logs);
            loglist.Insert(0, log);
            for (int i = 0; i < loglist.Count; i++)
            {
                if (loglist[i] is ConnectLog item_clog)
                {
                    clog = item_clog;
                    break;
                }
            }
            clog = clog ?? new ConnectLog();
            for (int i = 0; i < loglist.Count; i++)
            {
                if (loglist[i] is ConnectLog item_clog)
                {
                    IList<ILog> son_logs = item_clog.GetInsideLogs();
                    for (int son_i = 0; son_i < son_logs.Count; son_i++)
                    {
                        clog.Add(son_logs[son_i]);
                    }
                }
                else
                {
                    clog.Add(loglist[i]);
                }
            }
            return clog;
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

        /// <summary>
        /// 获取应用程序所在目录地址 (注意: 不是执行程序所在的地址)
        /// </summary>
        public static string GetApplicationDirectoryAddress() => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 生成默认日志文件信息
        /// </summary>
        /// <param name="region">日志内容标识</param>
        /// <returns>文件信息</returns>
        public static FileInfo GetLogFilePath(string region)
        {
            string path = Path.Combine(
                GetApplicationDirectoryAddress(),
                $"logs/{region}/{DateTime.Now:yyyy_MM_dd}.log");
            return new FileInfo(path);
        }
    }
}