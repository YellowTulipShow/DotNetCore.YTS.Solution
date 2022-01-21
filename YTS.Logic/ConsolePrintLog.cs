using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YTS.Logic
{
    /// <summary>
    /// 控制台打印日志输出实现
    /// </summary>
    public class ConsolePrintLog : ILog
    {
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            var msglist = new List<string>();
            string sign = "[Error]";
            msglist.Add($"{sign} {message}");
        }

        private IList<string> ToTree(string extend, string name, object value)
        {
            var msglist = new List<string>();
            const string null_value = "<null>";
            if (value == null)
            {
                msglist.Add($"{name} {null_value}");
                return msglist;
            }
            if (value is string str)
            {
                if (str == null)
                    str = null_value;
                else if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                    str = "<string.Empty>";
                msglist.Add($"{extend} {name}");
                msglist.Add($"{extend} └── {str}");
                return msglist;
            }
            if (value is IDictionary dic)
            {
                ICollection keys = dic.Keys;
                foreach (var key in keys)
                {
                    msglist.Add($"{extend} └── { str}");
                }
            }
            Type type = value.GetType();

            if (type.IsArray)
            {
                IEnumerable arr = value as IEnumerable;
            }
            msglist.Add($"{name} {JsonConvert.SerializeObject(value)}");
            return msglist;
        }

        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            Console.WriteLine($"[ErrorException] {message}, Exception: {ex.Message + ex.StackTrace} arg: {JsonConvert.SerializeObject(args)}");
        }

        public void Info(string message, params IDictionary<string, object>[] args)
        {
            Console.WriteLine($"[Info] {message}, arg: {JsonConvert.SerializeObject(args)}");
        }

        private void PrintConsoleLines(IList<string> msglist)
        {
            foreach (string str in msglist)
            {
                Console.WriteLine(str);
            }
        }
    }
}