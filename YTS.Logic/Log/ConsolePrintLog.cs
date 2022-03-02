using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace YTS.Logic.Log
{
    /// <summary>
    /// 实现类: 控制台打印日志输出实现
    /// </summary>
    public class ConsolePrintLog : BasicJSONConsolePrintLog, ILog
    {
        /// <inheritdoc />
        public new void Info(string message, params IDictionary<string, object>[] args)
        {
            Print("[Info]", message, null, args);
        }

        /// <inheritdoc />
        public new void Error(string message, params IDictionary<string, object>[] args)
        {
            Print("[Error]", message, null, args);
        }

        /// <inheritdoc />
        public new void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            Print("[ErrorException]", message, ex, args);
        }

        private void Print(string sign, string message, Exception ex, params IDictionary<string, object>[] args)
        {
            try
            {
                var msglist = new List<string>
                {
                    $"{sign} {message}"
                };
                if (args.Length == 1)
                {
                    msglist.AddRange(ToTree(sign.Length, $"arg", args[0]));
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        msglist.AddRange(ToTree(sign.Length, $"args[{i}]", args[i]));
                    }
                }
                if (ex != null)
                {
                    var exArgs = ToDynamic(ex);
                    msglist.AddRange(ToTree(sign.Length, $"Exception", exArgs));
                }
                PrintLines(msglist.ToArray());
            }
            catch (Exception re_ex)
            {
                base.Error("打印日志时发生错误! 内部错误!", re_ex, new Dictionary<string, object>()
                {
                    { "message", message },
                    { "ex", ex },
                    { "args", args },
                });
            }
        }
        private dynamic ToDynamic(Exception ex)
        {
            return ex == null ? null : new
            {
                ex.Message,
                ex.Data,
                StackTrace = (ex.StackTrace ?? string.Empty).Split('\n'),
                InnerException = ToDynamic(ex.InnerException),
                ex.Source,
            };
        }
        private IList<string> ToTree(int leftSpaceWidth, string name, object value)
        {
            var msglist = new List<string>();
            const string sign = "└──";
            const string null_value = "<null>";
            const string null_string = "<string.Empty>";
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
                    str = null_string;
                msglist.Add($"{leftSpace} {sign} {name}: {str}");
                return msglist;
            }
            Type type = value.GetType();
            if (type.IsValueType)
            {
                msglist.Add($"{leftSpace} {sign} {name}: {value?.ToString() ?? null_value}");
                return msglist;
            }
            msglist.Add($"{leftSpace} {sign} {name}:");
            if (value is IDictionary dic)
            {
                IDictionaryPushMsg(msglist, lowerLevel_leftSpaceWidth, dic);
            }
            else if (value is IEnumerable arr)
            {
                IEnumerablePushMsg(msglist, lowerLevel_leftSpaceWidth, arr);
            }
            else if (type.IsClass)
            {
                ClassPushMsg(value, msglist, lowerLevel_leftSpaceWidth, type);
            }
            else
            {
                JSONValuePushMsg(value, msglist, lowerLevel_leftSpaceWidth);
            }
            return msglist;
        }
        private void JSONValuePushMsg(object value, List<string> msglist, int lowerLevel_leftSpaceWidth)
        {
            var son_msglist = ToTree(lowerLevel_leftSpaceWidth, "JSON-Value", JsonConvert.SerializeObject(value));
            msglist.AddRange(son_msglist);
        }
        private void ClassPushMsg(object value, List<string> msglist, int lowerLevel_leftSpaceWidth, Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties();
            if (propertyInfos != null && propertyInfos.Length > 0)
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    if (!propertyInfo.CanRead)
                        continue;
                    var model_value = propertyInfo.GetValue(value, null);
                    var son_msglist = ToTree(lowerLevel_leftSpaceWidth, propertyInfo.Name, model_value);
                    msglist.AddRange(son_msglist);
                }
            }
        }
        private void IEnumerablePushMsg(List<string> msglist, int lowerLevel_leftSpaceWidth, IEnumerable arr)
        {
            int index = 0;
            foreach (var item in arr)
            {
                index++;
                var son_msglist = ToTree(lowerLevel_leftSpaceWidth, index.ToString(), item);
                msglist.AddRange(son_msglist);
            }
        }
        private void IDictionaryPushMsg(List<string> msglist, int lowerLevel_leftSpaceWidth, IDictionary dic)
        {
            ICollection keys = dic.Keys;
            foreach (var key in keys)
            {
                var dic_value = dic[key];
                var son_msglist = ToTree(lowerLevel_leftSpaceWidth, (key ?? "<key>").ToString(), dic_value);
                msglist.AddRange(son_msglist);
            }
        }
    }
}