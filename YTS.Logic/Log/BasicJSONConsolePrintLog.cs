using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace YTS.Logic.Log
{
    /// <summary>
    /// 实现类: 执行最基础最简单的JSON序列化打印日志方法
    /// </summary>
    public class BasicJSONConsolePrintLog : ILog
    {
        private const string null_message = "<null_message>";
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Info] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Error] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            PrintLines($"[ErrorException] {message ?? null_message}, Exception: {ex.Message + ex.StackTrace} arg: {JsonConvert.SerializeObject(args)}");
        }
        protected virtual void PrintLines(params string[] msglist)
        {
            foreach (string str in msglist)
            {
                Console.WriteLine(str);
            }
        }
    }
}
