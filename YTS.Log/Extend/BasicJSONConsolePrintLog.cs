using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace YTS.Log
{
    /// <summary>
    /// 实现类: 执行最基础最简单的JSON序列化打印日志方法
    /// </summary>
    public class BasicJSONConsolePrintLog : ILog
    {
        private const string null_message = "<null_message>";

        /// <inheritdoc />
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Info] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <inheritdoc />
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Error] {message ?? null_message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        /// <inheritdoc />
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
