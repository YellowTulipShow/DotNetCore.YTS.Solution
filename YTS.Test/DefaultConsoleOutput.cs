using System;

namespace YTS.Test
{
    /// <summary>
    /// 默认的控制台输出
    /// </summary>
    public class DefaultConsoleOutput : ITestOutput
    {
        private const ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;

        /// <summary>
        /// 初始化方法
        /// </summary>
        public void OnInit()
        {
            Console.ForegroundColor = DefaultConsoleColor;
        }

        /// <summary>
        /// 结尾方法
        /// </summary>
        public void OnEnd()
        {
            OnInit();
        }

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="msg">消息</param>
        public void Write(string msg)
        {
            this.OnInit();
            Console.Write(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出错误消息
        /// </summary>
        /// <param name="msg">错误消息</param>
        public void WriteError(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出信息消息
        /// </summary>
        /// <param name="msg">信息消息</param>
        public void WriteInfo(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出警告消息
        /// </summary>
        /// <param name="msg">警告消息</param>
        public void WriteWarning(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出消息 - 换行
        /// </summary>
        /// <param name="msg">消息</param>
        public void WriteLine(string msg)
        {
            this.OnInit();
            Console.WriteLine(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出错误消息 - 换行
        /// </summary>
        /// <param name="msg">错误消息</param>
        public void WriteLineError(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出信息消息 - 换行
        /// </summary>
        /// <param name="msg">信息消息</param>
        public void WriteLineInfo(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            this.OnEnd();
        }

        /// <summary>
        /// 输出警告消息 - 换行
        /// </summary>
        /// <param name="msg">警告消息</param>
        public void WriteLineWarning(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            this.OnEnd();
        }
    }
}
