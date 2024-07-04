using System;
using System.IO;
using System.CommandLine;

using YTS.Log;
using YTS.ConsolePrint;
using System.Collections;
using System.Collections.Generic;

namespace YTS.Command.FileRecognition
{
    /// <summary>
    /// 命令参数解析器
    /// </summary>
    public class CommandArgsParser
    {
        private readonly ILog log;
        private readonly IMain main;

        /// <summary>
        /// 实例化 - 命令参数解析器
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="main">程序接口</param>
        public CommandArgsParser(ILog log, IMain main)
        {
            this.log = log;
            this.main = main;
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
                Option<string> option_inventory_file_name = GetOption_InventoryFileName();
                Option<bool?> option_is_Recursive = GetOption_IsRecursive();

                RootCommand rootC = new RootCommand("识别路径中的文件, 输出文件清单");
                rootC.AddGlobalOption(option_root_path);
                rootC.AddGlobalOption(option_inventory_file_name);
                rootC.AddGlobalOption(option_is_Recursive);

                rootC.SetHandler(context =>
                {
                    try
                    {
                        string root_path = context.ParseResult.GetValueForOption(option_root_path);
                        logArgs["root_path"] = root_path;
                        string inventory_file_name = context.ParseResult.GetValueForOption(option_inventory_file_name);
                        logArgs["inventory_file_name"] = inventory_file_name;
                        bool is_recursive = context.ParseResult.GetValueForOption(option_is_Recursive) ?? false;
                        logArgs["is_Recursive"] = is_recursive;

                        main.OnExecute(new M.ExecuteParam()
                        {
                            root_path = root_path,
                            is_recursive = is_recursive,
                            inventory_file_name = inventory_file_name,
                        });
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
        private Option<string> GetOption_InventoryFileName()
        {
            var option = new Option<string>(
                aliases: new string[] { "-i", "--inventory" },
                getDefaultValue: () => "_inventory",
                description: "清单文件名称"); ;
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
