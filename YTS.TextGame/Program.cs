using System;
using System.Collections.Generic;
using CommandLine;

namespace YTS.TextGame
{
    class Program
    {
        public class Options
        {
            [Option('p', "path", Required = true, HelpText = "文件/夹路径地址")]
            public string Path { get; set; }
        }
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
        }

        private static void Run(Options option)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(string.Format("Path: {0}", new string[] { option.Path }));
        }
    }
}
