using System;
using YTS.Test;

namespace Test.ConsoleProgram
{
    public class ConsoleOutput : ITestOutput
    {
        private const ConsoleColor DefaultConsoleColor = ConsoleColor.Gray;

        public void OnInit()
        {
            Console.ForegroundColor = DefaultConsoleColor;
        }

        public void OnEnd()
        {
            OnInit();
        }

        public void Write(string msg)
        {
            this.OnInit();
            Console.Write(msg);
            this.OnEnd();
        }

        public void WriteError(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(msg);
            this.OnEnd();
        }

        public void WriteInfo(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(msg);
            this.OnEnd();
        }

        public void WriteWarning(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(msg);
            this.OnEnd();
        }

        public void WriteLine(string msg)
        {
            this.OnInit();
            Console.WriteLine(msg);
            this.OnEnd();
        }

        public void WriteLineError(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            this.OnEnd();
        }

        public void WriteLineInfo(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            this.OnEnd();
        }

        public void WriteLineWarning(string msg)
        {
            this.OnInit();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            this.OnEnd();
        }
    }
}
