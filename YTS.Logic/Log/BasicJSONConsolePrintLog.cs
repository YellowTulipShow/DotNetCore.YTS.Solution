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

        /// <summary>
        /// 常规信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Info] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <summary>
        /// 错误信息日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="args">参数队列</param>
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Error] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <summary>
        /// 错误信息和异常日志写入
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="ex">异常</param>
        /// <param name="args">参数队列</param>
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            PrintLines($"[ErrorException] {message ?? null_message}, Exception: {ex.Message + ex.StackTrace} arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <summary>
        /// 打印输出多行内容
        /// </summary>
        /// <param name="msglist">需要打印的多行消息列表</param>
        protected virtual void PrintLines(params string[] msglist)
        {
            foreach (string str in msglist)
            {
                Console.WriteLine(str);
            }
        }
    }
}
