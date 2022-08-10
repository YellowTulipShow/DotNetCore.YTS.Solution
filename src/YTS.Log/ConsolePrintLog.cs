using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;

namespace YTS.Log
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

        /// <summary>
        /// 数据内容
        /// </summary>
        public struct DataContent
        {
            /// <summary>
            /// 字符串值
            /// </summary>
            public string StrValue { get; set; }
            /// <summary>
            /// 字典值
            /// </summary>
            public IDictionary<string, object> Dict { get; set; }
        }

        /// <summary>
        /// 拆箱解析数据类型值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果值</returns>
        public DataContent ToDataContent(object data)
        {
            DataContent content = new DataContent()
            {
                StrValue = null,
                Dict = null,
            };

            if (data is null)
                return content;

            if (data is bool value_bool)
                content.StrValue = value_bool.ToString();

            if (data is sbyte value_sbyte)
                content.StrValue = value_sbyte.ToString();

            if (data is byte value_byte)
                content.StrValue = value_byte.ToString();

            if (data is short value_short)
                content.StrValue = value_short.ToString();

            if (data is ushort value_ushort)
                content.StrValue = value_ushort.ToString();

            if (data is int value_int)
                content.StrValue = value_int.ToString();

            if (data is uint value_uint)
                content.StrValue = value_uint.ToString();

            if (data is long value_long)
                content.StrValue = value_long.ToString();

            if (data is ulong value_ulong)
                content.StrValue = value_ulong.ToString();

            if (data is float value_float)
                content.StrValue = value_float.ToString();

            if (data is double value_double)
                content.StrValue = value_double.ToString();

            if (data is decimal value_decimal)
                content.StrValue = value_decimal.ToString();

            if (data is char value_char)
                content.StrValue = value_char.ToString();

            if (data is Enum value_enum)
                content.StrValue = value_enum.ToString();

            return content;
        }


        private void Print(string sign, string message, Exception ex, params IDictionary<string, object>[] args)
        {
            try
            {
                List<string> msglist = new List<string> { $"{sign} {message}:" };
                // 层级
                int layer = 0;
                List<string> signlist = new List<string> { };
                while (true)
                {
                    args

                }














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
                    var exArgs = ToIDictionary(ex);
                    msglist.AddRange(ToTree(sign.Length, $"Exception", exArgs));
                }
                PrintLines(msglist.ToArray());
            }
            catch (Exception re_ex)
            {
                var ex_logArgs = this.CreateArgDictionary();
                ex_logArgs["message"] = message;
                ex_logArgs["sonArgs"] = args;
                ex_logArgs["exArg"] = ex;
                throw new ILogParamException(ex_logArgs, "打印日志时发生错误! 内部错误!", re_ex);
            }
        }
        private IDictionary<string, object> ToIDictionary(Exception ex)
        {
            var args = this.CreateArgDictionary();
            args["Message"] = ex.Message;
            args["Data"] = ex.Data;
            args["Source"] = ex.Source;
            args["StackTrace"] = (ex.StackTrace ?? string.Empty).Split('\n');
            args["InnerException"] = ToIDictionary(ex.InnerException);
            return args;
        }

        /// <summary>
        /// 排序字符串键名队列
        /// </summary>
        /// <param name="dict">字典对象</param>
        /// <returns>排序结果</returns>
        public IList<string> GetSortStrKeys(IDictionary<string, object> dict)
        {
            ICollection<string> keys = dict.Keys;
            IList<string> rlist = keys.OrderBy(b => b).ToArray();
            return rlist;
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