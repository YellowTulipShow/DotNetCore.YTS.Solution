using System;
using System.Collections.Generic;
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
            if (msglist == null || msglist.Length <= 0)
                return;
            string time_format = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            List<string> rlist = new List<string>
            {
                $"[{time_format}] Insert Log:"
            };
            rlist.AddRange(msglist);
            rlist.Add(string.Empty);
            WriteFileContents(rlist);
        }

        private void WriteFileContents(IList<string> msglist)
        {
            if (msglist == null || msglist.Count <= 0)
                return;
            string fullPath = logFile.FullName;
            FileMode fileModel = File.Exists(fullPath) ? FileMode.Append : FileMode.Create;
            using (FileStream fs = new FileStream(fullPath, fileModel, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter sr = new StreamWriter(fs, encoding))
                {
                    for (int i = 0; i < msglist.Count; i++)
                    {
                        sr.WriteLine(msglist[i]);
                    }
                    sr.Close();
                    fs.Close();
                }
            }
        }
    }
}