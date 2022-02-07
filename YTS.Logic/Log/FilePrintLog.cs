using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace YTS.Logic.Log
{
    /// <summary>
    /// 实现类: 文件打印日志输出实现
    /// </summary>
    public class FilePrintLog : ConsolePrintLog
    {
        private readonly string abs_file_path;
        private readonly Encoding encoding;

        /// <summary>
        /// 初始化实现类: 文件打印日志输出实现
        /// </summary>
        /// <param name="abs_file_path">绝对路径文件地址</param>
        /// <param name="encoding">文件编码内容</param>
        public FilePrintLog(string abs_file_path, Encoding encoding)
        {
            this.abs_file_path = abs_file_path;
            this.encoding = encoding;
        }

        /// <summary>
        /// 打印输出多行内容
        /// </summary>
        /// <param name="msglist">需要打印的多行消息列表</param>
        protected override void PrintLines(params string[] msglist)
        {
            string time_format = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string[] log_head = new string[] { $"[{time_format}] Insert Log:" };
            if (!File.Exists(abs_file_path))
            {
                using var stream = File.Create(abs_file_path);
            }
            File.AppendAllLines(abs_file_path, log_head, encoding);
            File.AppendAllLines(abs_file_path, msglist, encoding);
        }
    }
}