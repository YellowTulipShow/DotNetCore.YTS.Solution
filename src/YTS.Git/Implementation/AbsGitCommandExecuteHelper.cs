using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

using YTS.Git.Models;

namespace YTS.Git.Implementation
{
    /// <summary>
    /// 抽象类: Git 命令执行通用帮助方法
    /// </summary>
    public abstract class AbsGitCommandExecuteHelper
    {
        /// <summary>
        /// 注入的存储库信息
        /// </summary>
        protected readonly Repository repository;

        /// <summary>
        /// 实例化 - 抽象类: Git 命令执行通用帮助方法
        /// </summary>
        /// <param name="repository">需操作的存储库配置</param>
        public AbsGitCommandExecuteHelper(Repository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 单独执行命令内容, 读取返回控制台消息内容
        /// </summary>
        /// <param name="command">命令内容</param>
        /// <param name="readLineCallBack">读取行内容回调方法, 可为空不读取内容</param>
        public virtual void OnExecuteOnlyCommand(string command, Action<string> readLineCallBack)
        {
            command = (command ?? string.Empty).Trim();
            command = Regex.Replace(command, @"^git\s*", "");

            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException("command", "命令内容为空");
            if (repository.RootPath == null)
                throw new ArgumentNullException("repository.RootPath", "存储库根目录配置为空");
            if (!repository.RootPath.Exists)
                throw new ArgumentOutOfRangeException("repository.RootPath", "存储库根目录不存在");

            ProcessStartInfo info = new ProcessStartInfo(@"git", command)
            {
                UseShellExecute = false,
                WorkingDirectory = repository.RootPath.FullName,
                RedirectStandardOutput = true,
                StandardOutputEncoding = repository.OutputTextEncoding ?? Encoding.UTF8,
            };
            using (Process process = Process.Start(info))
            {
                if (readLineCallBack != null)
                {
                    using StreamReader sr = process.StandardOutput;
                    while (!sr.EndOfStream)
                    {
                        readLineCallBack(sr.ReadLine());
                    }
                    sr.Close();
                }
                process.WaitForExit();
                process.Close();
            }
        }
    }
}
