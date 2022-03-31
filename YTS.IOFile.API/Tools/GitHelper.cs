using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// Git 操作工具类
    /// </summary>
    public class GitHelper : IGit
    {
        private readonly Encoding encoding;
        private readonly StoreConfiguration storeConfiguration;

        /// <summary>
        /// 实例化 - Git 操作工具类
        /// </summary>
        /// <param name="storeConfiguration">工作目录</param>
        public GitHelper(StoreConfiguration storeConfiguration)
        {
            this.storeConfiguration = storeConfiguration;
            encoding = Encoding.UTF8;
        }
        /// <summary>
        /// 实例化 - Git 操作工具类
        /// </summary>
        /// <param name="storeConfiguration"></param>
        /// <param name="encoding"></param>
        public GitHelper(StoreConfiguration storeConfiguration, Encoding encoding)
        {
            this.storeConfiguration = storeConfiguration;
            this.encoding = encoding;
        }

        private void OnExecuteOnlyCommand(string command, Action<string> readLineCallBack)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"git", command)
            {
                UseShellExecute = false,
                WorkingDirectory = storeConfiguration.SystemAbsolutePath,
                RedirectStandardOutput = true,
                StandardOutputEncoding = encoding,
            };
            Process process = Process.Start(info);
            if (readLineCallBack != null)
            {
                using var sr = process.StandardOutput;
                while (!sr.EndOfStream)
                {
                    readLineCallBack(sr.ReadLine());
                }
                sr.Close();
            }
            process.WaitForExit();
            process.Close();
        }

        /// <inheritdoc />
        public void Add(string filePath)
        {
            string command = "add .";
            filePath = filePath?.Trim();
            if (!string.IsNullOrEmpty(filePath))
            {
                command = $"add {filePath}";
            }
            OnExecuteOnlyCommand(command, null);
        }

        /// <inheritdoc />
        public StringBuilder Status()
        {
            StringBuilder builder = new StringBuilder();
            OnExecuteOnlyCommand("status", line => {
                builder.AppendLine(line);
            });
            return builder;
        }

        /// <inheritdoc />
        public StringBuilder Commit(string message)
        {
            message = message?.Trim() ?? "save data";
            StringBuilder builder = new StringBuilder();
            OnExecuteOnlyCommand($"commit -m \"{message}\"", line => {
                builder.AppendLine(line);
            });
            return builder;
        }

        /// <inheritdoc />
        public StringBuilder Pull(string message)
        {
            message = message?.Trim() ?? "save data";
            StringBuilder builder = new StringBuilder();
            OnExecuteOnlyCommand("fetch origin", line => {
                builder.AppendLine(line);
            });
            OnExecuteOnlyCommand($"merge origin/master -m \"{message}\"", line => {
                builder.AppendLine(line);
            });
            return builder;
        }

        /// <inheritdoc />
        public StringBuilder Push()
        {
            StringBuilder builder = new StringBuilder();
            OnExecuteOnlyCommand("push origin master", line => {
                builder.AppendLine(line);
            });
            return builder;
        }
    }
}
