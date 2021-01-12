using System;
using CommandLine;
using YTS.Tools;

namespace YTS.TextGame
{
    class Program
    {
        public class Options
        {
            [Option('p', "path", Required = false, HelpText = "文件/夹路径地址")]
            public string Path { get; set; }
        }
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
        }

        private static string[] GetWorlds() => new string[]
            {
                "illegal",
                "self",
                "arouse",
                "container",
                "shift",
                "probably",
                "recommend",
                "invest",
                "green",
                "adventure",
                "president",
                "weed",
                "rank",
                "fade",
                "no",
                "displease",
                "fill",
                "stain",
                "thirteen",
                "their",
            };

        private static void Run(Options option)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(string.Format("Path: {0}", new string[] { option.Path }));
            StartGame();
        }

        private static void StartGame()
        {
            string[] worlds = GetWorlds();
            Console.WriteLine("开始游戏了:");
            int size = 3;
            var ccm = new ConsoleCleanManage();
            ccm.RecordPoint();
            bool IsRepeatExecute()
            {
                Console.WriteLine(@"请输入命令: Q(退出) R(重复执行) C(清空屏幕)");
                ConsoleKeyInfo keyinfo = Console.ReadKey(false);
                Console.WriteLine(string.Empty);
                ccm.CleanALL();
                switch (keyinfo.Key)
                {
                    case ConsoleKey.Q:
                        return false;
                    case ConsoleKey.R:
                        return true;
                    case ConsoleKey.C:
                        return IsRepeatExecute();
                    default:
                        return IsRepeatExecute();
                }
            }
            do
            {
                string world = RandomData.GetItem(worlds);
                int unfinishedCount = size;
                while (unfinishedCount > 0)
                {
                    Console.WriteLine($"单词: {world} 请重复写入, 剩余次数: {unfinishedCount - 1}");
                    string inputContent = Console.ReadLine();
                    ccm.CleanALL();
                    if (world == inputContent)
                    {
                        unfinishedCount--;
                    }
                    else
                    {
                        Console.WriteLine($"输入 {inputContent} 错误, 请重新再来一遍!");
                    }
                }
            } while (IsRepeatExecute());
            Console.WriteLine("游戏已结束!");
        }
    }
}
