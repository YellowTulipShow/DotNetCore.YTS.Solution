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

        public void Info(string message, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, args);
            }
        }
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, args);
            }
        }
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            foreach (var item in logDict.Values)
            {
                item.Error(message, ex, args);
            }
        }
    }
}
