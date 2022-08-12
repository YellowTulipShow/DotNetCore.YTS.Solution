using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace YTS.Log
{
    /// <summary>
    /// 实现类: 控制台打印日志输出实现
    /// </summary>
    public class ConsolePrintLog : BasicJSONConsolePrintLog, ILog
    {
        private readonly HashSet<string> modelUsingHash;

        /// <summary>
        /// 实例化: 控制台打印日志输出实现
        /// </summary>
        public ConsolePrintLog()
        {
            modelUsingHash = new HashSet<string>();
        }

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
                if (operlist == null || operlist.Count == 0)
                {
                    msglist[0] += " ->>> <空参数内容>";
                }
                else if (operlist.Count == 1)
                {
                    PrintItem oper = operlist[0];
                    IList<string> son_msgs = ToMsgList(oper.Content.Dict);
                    msglist.AddRange(son_msgs);
                }
                else
                {
                    IList<string> opermsgs = ToMsgList(operlist);
                    msglist.AddRange(opermsgs);
                }
                PrintLines(msglist.ToArray());
            }
            catch (Exception re_ex)
            {
                var ex_logArgs = this.CreateArgDictionary();
                ex_logArgs["sign"] = sign;
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

        private IList<string> ToMsgList(IList<PrintItem> operlist, IList<int> parentPrefixSigns = null)
        {
            parentPrefixSigns = parentPrefixSigns ?? new List<int>() { 0 };
            List<string> msglist = new List<string>();
            int len = operlist.Count;
            for (int i = 0; i < len; i++)
            {
                bool isLast = i == len - 1;
                IList<int> prefixSigns = new List<int>(parentPrefixSigns)
                    { isLast ? 2 : 1 };
                string prefix = CalcPrefix(prefixSigns);
                prefixSigns[prefixSigns.Count - 1] = isLast ? 0 : 3;
                PrintItem oper = operlist[i];
                msglist.Add($"{prefix}{oper.Name}: {oper.Content.StrValue ?? string.Empty}".TrimEnd());
                if (oper.Content.Dict != null && oper.Content.Dict.Count > 0)
                {
                    IList<string> son_msgs = ToMsgList(oper.Content.Dict, prefixSigns);
                    msglist.AddRange(son_msgs);
                }
            }
            return msglist;
        }

        private string CalcPrefix(IList<int> prefixSigns)
        {
            int len = prefixSigns.Count;
            string[] prefixs = new string[len];
            for (int i = 0; i < len; i++)
            {
                switch (prefixSigns[i])
                {
                    case 0: prefixs[i] = "    "; break;
                    case 1: prefixs[i] = "├── "; break;
                    case 2: prefixs[i] = "└── "; break;
                    case 3: prefixs[i] = "|   "; break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(prefixSigns),
                            $"解析前缀标识出错: {prefixSigns[i]}");
                }
            }
            return string.Join("", prefixs);
        }

        /// <summary>
        /// 打印项
        /// </summary>
        private struct PrintItem
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
        private struct DataContent
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

        /// <summary>
        /// 拆箱解析数据类型值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果值</returns>
        private DataContent ToDataContent(object data)
        {
            IList<PrintItem> args = new List<PrintItem>();
            DataContent content = new DataContent()
            {
                StrValue = null,
                Dict = null,
            };
            if (data is null)
            {
                content.StrValue = "null";
                return content;
            }

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

            // 异常
            if (data is Exception value_ex)
            {
                content.Dict = ToPrintItems(value_ex);
                return content;
            }

            // 时间
            if (data is DateTime value_datetime)
            {
                content.StrValue = value_datetime.ToString("yyyy-MM-dd HH:mm:ss");
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

            // struct : 结构体
            if (!type.IsPrimitive && !type.IsEnum && type.IsValueType)
            {
                return ModelToDataContent(data, content, type);
            }

            // 值类型
            if (type.IsValueType)
            {
                content.StrValue = data.ToString();
                return content;
            }

            // 对象类型
            if (type.IsClass)
            {
                return ModelToDataContent(data, content, type);
            }
            return content;
        }

        private IList<PrintItem> ToPrintItems(IDictionary<string, object> dict)
        {
            if (dict == null)
                return new PrintItem[] { };
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

        private IList<string> GetSortStrKeys(IDictionary<string, object> dict)
        {
            if (dict == null)
                return new string[] { };
            ICollection<string> keys = dict.Keys;
            IList<string> rlist = keys.OrderBy(b => b).ToArray();
            return rlist;
        }

        private IList<PrintItem> ToPrintItems(Exception ex)
        {
            if (ex == null)
                return new PrintItem[] { };

            if (ex is ILogParamException logParamEx)
                return ToPrintItems(logParamEx);

            return ExceptionToPrintItems(ex);
        }
        private IList<PrintItem> ExceptionToPrintItems(Exception ex)
        {
            if (ex == null)
                return new PrintItem[] { };
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
                    Content = ToDataContent((ex.StackTrace ?? string.Empty)
                        .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)),
                },
                new PrintItem()
                {
                    Name = "InnerException",
                    Content = new DataContent() { Dict = ToPrintItems(ex.InnerException) } ,
                },
            };
        }
        private IList<PrintItem> ToPrintItems(ILogParamException ex)
        {
            if (ex == null)
                return new PrintItem[] { };

            IList<PrintItem> arr = ExceptionToPrintItems(ex);
            IDictionary<string, object> param = ex.GetParam();
            arr.Add(new PrintItem()
            {
                Name = "Param",
                Content = new DataContent()
                {
                    Dict = ToPrintItems(param),
                },
            });
            return arr;
        }

        private DataContent ModelToDataContent(object data, DataContent content, Type type)
        {
            IList<PrintItem> args = new List<PrintItem>();
            if (data == null || type == null)
                return content;

            // 判断是否已经输出过的模型数据
            string class_sign = $"{type.FullName}_{data.GetHashCode()}";
            if (modelUsingHash.Contains(class_sign))
            {
                content.StrValue = "重复赋值参数输出";
                return content;
            }
            modelUsingHash.Add(class_sign);

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
    }
}