using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using YTS.Logic.Log;

namespace YTS.Logic.Test
{
    public class LogUniversal : ILog
    {
        private const string null_message = "<null_message>";

        /// <summary>
        /// 常规信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            Console.WriteLine($"[Info] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <summary>
        /// 错误信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            Console.WriteLine($"[Error] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <summary>
        /// 错误信息和异常日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="ex">异常</param>
        /// <param name="args">参数队列</param>
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            Console.WriteLine($"[ErrorException] {message ?? null_message}, Exception: {ex.Message + ex.StackTrace} arg: {JsonConvert.SerializeObject(args)}");
        }
    }
}
