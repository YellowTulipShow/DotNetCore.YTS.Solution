using System;
using System.IO;
using System.CommandLine;

using YTS.Log;
using YTS.ConsolePrint;

namespace YTS.Command.FileRecognition
{
    /// <summary>
    /// 命令参数解析器
    /// </summary>
    public class CommandArgsParser
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化 - 命令参数解析器
        /// </summary>
        /// <param name="log">日志接口</param>
        public CommandArgsParser(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 解析执行
        /// </summary>
        /// <param name="args">用户传入的命令行参数</param>
        /// <returns>执行返回编码</returns>
        public int OnParser(string[] args)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["CommandInputArgs"] = args;
            int code = 0;
            try
            {
                Option<string> option_root_path = GetOption_RootPath();
                Option<bool?> option_is_Recursive = GetOption_IsRecursive();

                RootCommand rootC = new RootCommand("识别路径中的文件, 输出文件清单");
                rootC.AddGlobalOption(option_root_path);
                rootC.AddGlobalOption(option_is_Recursive);

                rootC.SetHandler(context =>
                {
                    try
                    {
                        string root_path = context.ParseResult.GetValueForOption(option_root_path);
                        logArgs["root_path"] = root_path;
                        bool is_Recursive = context.ParseResult.GetValueForOption(option_is_Recursive) ?? false;
                        logArgs["is_Recursive"] = is_Recursive;

                        Console.WriteLine($"路径: {root_path}");
                        string r_name = is_Recursive ? "递归" : "不递归";
                        Console.WriteLine($"是否递归: {r_name}");
                    }
                    catch (Exception ex)
                    {
                        log.Error("执行程序逻辑出错", ex, logArgs);
                        code = 2;
                    }
                });

                return rootC.Invoke(args);
            }
            catch (Exception ex)
            {
                log.Error("解释命令出错", ex, logArgs);
                code = 1;
            }
            return code;
        }

        private Option<string> GetOption_RootPath()
        {
            var option = new Option<string>(
                aliases: new string[] { "-p", "--path" },
                getDefaultValue: () =>
                {
                    // 当前用户配置地址
                    var dire = System.Environment.CurrentDirectory;
                    return dire;
                },
                description: "需执行识别的根目录"); ;
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
        private Option<bool?> GetOption_IsRecursive()
        {
            var option = new Option<bool?>(
                aliases: new string[] { "-r", "--is-recursive" },
                getDefaultValue: () => false,
                description: "是否递归查询目录"); ;
            option.Arity = ArgumentArity.ZeroOrOne;
            return option;
        }
    }
}
