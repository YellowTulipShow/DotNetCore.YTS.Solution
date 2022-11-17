using System;
using System.Collections.Generic;
using System.Linq;

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
        public virtual void Info(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Info] {message ?? null_message}, args: {ToJSON(args)}");
        }
        private string ToJSON(object args)
        {
            try
            {
                string args_json = JsonConvert.SerializeObject(args);
                return args_json;
            }
            catch (Exception ex)
            {
                return $"序列化参数出错: {ex.Message}";
            }
        }

        /// <inheritdoc />
        public virtual void Error(string message, params IDictionary<string, object>[] args)
        {
            PrintLines($"[Error] {message ?? null_message}, args: {ToJSON(args)}");
        }

        /// <inheritdoc />
        public virtual void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            if (ex == null)
            {
                Error(message, args);
                return;
            }

            IList<string> msglist = new List<string>();
            if (ex is ILogParamException logParamEx)
            {
                string msg = string.Join(", ", new string[]
                {
                    $"[ErrorException] {message ?? null_message}",
                    $"Exception: {ex.Message + ex.StackTrace}",
                    $"args: {ToJSON(args)}",
                    $"logParam: {ToJSON(logParamEx.GetParam())}",
                });
                msglist.Add(msg);
            }
            else
            {
                string msg = string.Join(", ", new string[]
                {
                    $"[ErrorException] {message ?? null_message}",
                    $"Exception: {ex.Message + ex.StackTrace}",
                    $"args: {ToJSON(args)}",
                });
                msglist.Add(msg);
            }
            Exception inex = ex.InnerException;
            while (inex != null)
            {
                if (inex is ILogParamException inlogParamEx)
                {
                    string msg = string.Join(", ", new string[]
                    {
                        $"[ErrorException.InnerException] {inex.Message}",
                        $"StackTrace: {inex.StackTrace}",
                        $"logParam: {ToJSON(inlogParamEx.GetParam())}",
                    });
                    msglist.Add(msg);
                }
                else
                {
                    string msg = string.Join(", ", new string[]
                    {
                        $"[ErrorException.InnerException] {inex.Message}",
                        $"StackTrace: {inex.StackTrace}",
                    });
                    msglist.Add(msg);
                }
                inex = inex.InnerException;
            }
            PrintLines(msglist.ToArray());
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
