using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace YTS.Tools
{
    /// <summary>
    /// 系统日志记录
    /// </summary>
    public class Logs : AbsBasicDataModel
    {
        public Logs() { }

        /// <summary>
        /// 日志 类型枚举
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 记录
            /// </summary>
            [Explain(@"记录")]
            Record = 0,

            /// <summary>
            /// 错误
            /// </summary>
            [Explain(@"错误")]
            Error = 1,

            /// <summary>
            /// 异常
            /// </summary>
            [Explain(@"异常")]
            Exception = 2,
        }

        /// <summary>
        /// 日志类型 (默认为 LogEnum.Error 错误)
        /// </summary>
        public LogType Type { get; set; } = LogType.Record;

        /// <summary>
        /// 写入日志位置
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 详情消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 添加时间 (默认添加当前时间)
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 写入一个日志
        /// </summary>
        /// <param name="lgModel">日志的数据模型</param>
        /// <returns>写入的日志文件绝对路径</returns>
        public string Write()
        {
            string path = GetLogFilePath();
            string formatcontent = GetFormatContent();
            Encoding encode = Encoding.UTF8;
            File.AppendAllText(path, formatcontent, encode);
            return path;
        }

        /// <summary>
        /// 获取日志文件路径 以指定时间计算
        /// </summary>
        private string GetLogFilePath()
        {
            string directory = string.Format("/Logs/{0}", this.Type.ToString());
            string file = string.Format("{0}.log", this.AddTime.ToString("yyyy-MM-dd-HH-mm-ss"));
            return PathHelp.CreateUseFilePath(directory, file);
        }

        /// <summary>
        /// 获取格式化后内容
        /// </summary>
        protected virtual string GetFormatContent()
        {
            string[] strs = new string[] {
                ConvertTool.ToString(this.AddTime),
                this.Type.ToString(),
                this.Position,
                this.Message,
            };
            return ConvertTool.ToString(strs, @"  >>  ") + "\n";
        }

        /// <summary>
        /// 写入一个异常报错日志
        /// </summary>
        /// <param name="ex">表示在应用程序执行期间发生的错误。</param>
        public static void Write(Exception ex)
        {
            Logs log = new LogsException()
            {
                Type = LogType.Exception,
                AddTime = DateTime.Now,
                Position = new StackTrace(true).ToString(),
                Message = ex.Message,
                IsStackTraceList = true,
            };
            log.Write();
        }

        /// <summary>
        /// 输出异常堆栈记录
        /// </summary>
        /// <param name="msg">自定义消息文本</param>
        public static void WriteStackTrace(string msg)
        {
            Logs log = new LogsException()
            {
                Type = Logs.LogType.Exception,
                AddTime = DateTime.Now,
                Position = new System.Diagnostics.StackTrace(true).ToString(),
                Message = msg,
                IsStackTraceList = true,
            };
            log.Write();
        }
    }

    /// <summary>
    /// 异常版系统日志
    /// </summary>
    public class LogsException : Logs
    {
        public LogsException() { }

        /// <summary>
        /// 是否开启堆栈跟踪列表, 默认不开启
        /// </summary>
        public bool IsStackTraceList { get; set; }

        /// <summary>
        /// 格式化日志信息, 其中判断使用堆栈跟踪
        /// </summary>
        /// <returns>格式化后文本内容</returns>
        protected override string GetFormatContent()
        {
            if (!this.IsStackTraceList)
            {
                return base.GetFormatContent();
            }
            string[] strs = new string[] {
                ConvertTool.ToString(this.AddTime),
                this.Type.ToString(),
                this.Message,
            };
            string[] strs2 = new string[] {
                ConvertTool.ToString(strs, @"  >>  "),
                @"堆栈跟踪:",
                this.Position,
            };
            return ConvertTool.ToString(strs2, "\n") + "\n";
        }
    }
}
