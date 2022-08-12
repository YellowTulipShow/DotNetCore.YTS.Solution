using System;
using System.IO;
using System.Text;

namespace YTS.Log
{
    /// <summary>
    /// 实现类: 文件打印日志输出实现
    /// </summary>
    public class FilePrintLog : ConsolePrintLog
    {
        private readonly FileInfo logFile;
        private readonly Encoding encoding;

        /// <summary>
        /// 初始化实现类: 文件打印日志输出实现
        /// </summary>
        /// <param name="logFile">文件信息</param>
        /// <param name="encoding">文件编码内容</param>
        public FilePrintLog(FileInfo logFile, Encoding encoding)
        {
            var dire = logFile.Directory;
            if (!dire.Exists)
            {
                dire.Create();
            }
            if (!logFile.Exists)
            {
                logFile.Create().Close();
            }
            this.logFile = logFile;
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
            File.AppendAllLines(logFile.FullName, log_head, encoding);
            File.AppendAllLines(logFile.FullName, msglist, encoding);
        }
    }
}