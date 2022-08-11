﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

        private void Print(string sign, string message, Exception ex, params IDictionary<string, object>[] args)
        {
            try
            {
                List<string> msglist = new List<string> {
                    $"{sign} {message}:"
                };
                IList<PrintItem> operlist = CalcOperableList(ex, args);
                string prefix = " ".PadLeft(4);
                if (operlist == null || operlist.Count == 0)
                {
                    msglist[0] += " ->>> <空参数内容>";
                }
                else if (operlist.Count == 1)
                {
                    PrintItem oper = operlist[0];
                    IList<string> son_msgs = ToMsgList(oper.Content.Dict, prefix);
                    msglist.AddRange(son_msgs);
                }
                else
                {
                    IList<string> opermsgs = ToMsgList(operlist, prefix);
                    msglist.AddRange(opermsgs);
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
        private IList<PrintItem> CalcOperableList(Exception ex, params IDictionary<string, object>[] args)
        {
            IList<PrintItem> operlist = new List<PrintItem>();
            if (ex != null)
            {
                operlist.Add(new PrintItem()
                {
                    Name = "Exception",
                    Content = new DataContent()
                    {
                        Dict = ToPrintItems(ex)
                    },
                });
            }
            if (args == null || args.Length <= 0)
            {
                return operlist;
            }
            for (int i = 0; i < args.Length; i++)
            {
                operlist.Add(new PrintItem()
                {
                    Name = $"arg[{i}]",
                    Content = new DataContent()
                    {
                        Dict = ToPrintItems(args[i])
                    },
                });
            }
            return operlist;
        }
        private IList<string> ToMsgList(IList<PrintItem> operlist, string parent_prefix)
        {
            List<string> msglist = new List<string>();
            for (int i = 0; i < operlist.Count; i++)
            {
                string prefix = CalcPrefix(parent_prefix, i == operlist.Count - 1);
                PrintItem oper = operlist[i];
                msglist.Add($"{prefix}{oper.Name}: {oper.Content.StrValue ?? string.Empty}".TrimEnd());
                if (oper.Content.Dict != null && oper.Content.Dict.Count > 0)
                {
                    IList<string> son_msgs = ToMsgList(oper.Content.Dict, prefix);
                    msglist.AddRange(son_msgs);
                }
            }
            return msglist;
        }
        private string CalcPrefix(string parent, bool isLast)
        {
            parent = parent.Replace('─', ' ');
            parent = Regex.Replace(parent, @"[^\s]$", "│");
            char head = isLast ? '└' : '├';
            return $"{parent}{head}── ";
        }

        /// <summary>
        /// 打印项
        /// </summary>
        public struct PrintItem
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 值内容
            /// </summary>
            public DataContent Content { get; set; }
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
            public IList<PrintItem> Dict { get; set; }
        }

        private IList<PrintItem> ToPrintItems(IDictionary<string, object> dict)
        {
            IList<PrintItem> list = new List<PrintItem>();
            IList<string> keys = GetSortStrKeys(dict) ?? new string[] { };
            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                list.Add(new PrintItem()
                {
                    Name = key,
                    Content = ToDataContent(dict[key])
                });
            }
            return list;
        }

        /// <summary>
        /// 拆箱解析数据类型值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果值</returns>
        public DataContent ToDataContent(object data)
        {
            IList<PrintItem> args = new List<PrintItem>();
            DataContent content = new DataContent()
            {
                StrValue = null,
                Dict = null,
            };
            if (data is null) return content;

            // 字符串
            if (data is string value_str)
            {
                content.StrValue = value_str;
                return content;
            }

            // 枚举
            if (data is Enum value_enum)
            {
                content.StrValue = value_enum.ToString();
                return content;
            }

            // 字典
            if (data is IDictionary value_dict_nulltype)
            {
                foreach (object key in value_dict_nulltype.Keys)
                {
                    string key_str = key?.ToString() ?? "<Null Key>";
                    args.Add(new PrintItem()
                    {
                        Name = key_str,
                        Content = ToDataContent(value_dict_nulltype[key]),
                    });
                }
                content.Dict = args;
                return content;
            }

            // 日志字典
            if (data is IDictionary<string, object> value_dict)
            {
                content.Dict = ToPrintItems(value_dict);
                return content;
            }

            // 异常参数
            if (data is Exception value_ex)
            {
                content.Dict = ToPrintItems(value_ex);
                return content;
            }

            // 迭代器
            if (data is IEnumerator list_Enumerator)
            {
                int index = 0;
                while (list_Enumerator.MoveNext())
                {
                    args.Add(new PrintItem()
                    {
                        Name = $"[{index}]",
                        Content = ToDataContent(list_Enumerator.Current),
                    });
                    index++;
                }
                content.Dict = args;
                return content;
            }

            // 枚举迭代
            if (data is IEnumerable list)
            {
                int index = 0;
                foreach (var item in list)
                {
                    args.Add(new PrintItem()
                    {
                        Name = $"[{index}]",
                        Content = ToDataContent(item),
                    });
                    index++;
                }
                content.Dict = args;
                return content;
            }

            Type type = data.GetType();

            // 值类型
            if (type.IsValueType)
            {
                content.StrValue = data.ToString();
                return content;
            }

            // 对象类型
            if (type.IsClass)
            {
                foreach (var item in type.GetProperties())
                {
                    object item_value = item.GetValue(data, null);
                    args.Add(new PrintItem()
                    {
                        Name = item.Name,
                        Content = ToDataContent(item_value),
                    });
                }
                foreach (var item in type.GetFields())
                {
                    object item_value = item.GetValue(data);
                    args.Add(new PrintItem()
                    {
                        Name = item.Name,
                        Content = ToDataContent(item_value),
                    });
                }
                content.Dict = args;
                return content;
            }
            return content;
        }

        private IList<PrintItem> ToPrintItems(Exception ex)
        {
            return new List<PrintItem>
            {
                new PrintItem()
                {
                    Name = "Message",
                    Content = new DataContent() { StrValue = ex.Message, }
                },
                new PrintItem()
                {
                    Name = "Data",
                    Content = ToDataContent(ex.Data),
                },
                new PrintItem()
                {
                    Name = "Source",
                    Content = new DataContent() { StrValue = ex.Source, }
                },
                new PrintItem()
                {
                    Name = "StackTrace",
                    Content = ToDataContent((ex.StackTrace ?? string.Empty).Split('\n')),
                },
                new PrintItem()
                {
                    Name = "InnerException",
                    Content = new DataContent() { Dict = ToPrintItems(ex.InnerException) } ,
                },
            };
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
    }
}