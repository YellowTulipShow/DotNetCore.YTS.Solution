using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace YTS.Logic
{
    /// <summary>
    /// 控制台打印日志输出实现
    /// </summary>
    public class ConsolePrintLog : ILog
    {
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            Print("[Info]", message, null, args);
        }
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            Print("[Error]", message, null, args);
        }
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            Print("[ErrorException]", message, ex, args);
        }

        private void Print(string sign, string message, Exception ex, params IDictionary<string, object>[] args)
        {
            var msglist = new List<string>
            {
                $"{sign} {message}"
            };
            for (int i = 0; i < args.Length; i++)
            {
                msglist.AddRange(ToTree(sign.Length, $"args[{i}]", args[i]));
            }
            if (ex != null)
            {
                msglist.AddRange(ToTree(sign.Length, $"Exception", ex));
            }
            PrintConsoleLines(msglist);
        }
        private IList<string> ToTree(int leftSpaceWidth, string name, object value)
        {
            var msglist = new List<string>();
            const string sign = "└──";
            const string null_value = "<null>";
            string leftSpace = string.Empty.PadLeft(leftSpaceWidth, ' ');
            int lowerLevel_leftSpaceWidth = leftSpaceWidth + 1 + sign.Length;

            if (value == null)
            {
                msglist.Add($"{leftSpace} {sign} {name} {null_value}");
                return msglist;
            }
            else if (value is string str)
            {
                if (str == null)
                    str = null_value;
                else if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                    str = "<string.Empty>";
                msglist.Add($"{leftSpace} {sign} {name}: {str}");
                return msglist;
            }

            Type type = value.GetType();
            if (type.IsSealed)
            {
                msglist.Add($"{leftSpace} {sign} {name}: {value?.ToString() ?? null_value}");
                return msglist;
            }
            msglist.Add($"{leftSpace} {sign} {name}:");
            if (value is IDictionary dic)
            {
                ICollection keys = dic.Keys;
                foreach (var key in keys)
                {
                    var dic_value = dic[key];
                    var son_msglist = ToTree(lowerLevel_leftSpaceWidth, (key ?? "<key>").ToString(), dic_value);
                    msglist.AddRange(son_msglist);
                }
            }
            else if(value is IEnumerable arr)
            {
                int index = 0;
                foreach (var item in arr)
                {
                    index++;
                    var son_msglist = ToTree(lowerLevel_leftSpaceWidth, index.ToString(), item);
                    msglist.AddRange(son_msglist);
                }
            }
            else
            {
                PropertyInfo[] propertyInfos = type.GetProperties();
                if (propertyInfos != null && propertyInfos.Length > 0)
                {
                    foreach (var propertyInfo in propertyInfos)
                    {
                        var model_value = propertyInfo.GetValue(value, null);
                        var son_msglist = ToTree(lowerLevel_leftSpaceWidth, propertyInfo.Name, model_value);
                        msglist.AddRange(son_msglist);
                    }
                }
                else
                {
                    var son_msglist = ToTree(lowerLevel_leftSpaceWidth, "JSON-Value", JsonConvert.SerializeObject(value));
                    msglist.AddRange(son_msglist);
                }
            }
            return msglist;
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