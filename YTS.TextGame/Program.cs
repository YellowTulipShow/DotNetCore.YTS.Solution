using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            var ccm = new ConsoleCleanManage();
            ccm.RecordPoint();
            Console.WriteLine(string.Format("Path: asdfasdfsdfsdfsdfsdfsdffsdfsdf", new string[] { option.Path }));
            ccm.CleanALL();
            Console.WriteLine(string.Format("Path: asdfasdfsdfsdfsdfsdfsdffsdfsdf", new string[] { option.Path }));
            Console.WriteLine(string.Format("Path: asdfasdfsdfsdfsdfsdfsdffsdfsdf", new string[] { option.Path }));
            Console.WriteLine(string.Format("Path: asdfasdfsdfsdfsdfsdfsdffsdfsdf", new string[] { option.Path }));
            ccm.CleanALL();
            // ConsoleExpand.ClearLineContents(1);
            // Consoler.OverWrite("dfsdfsdfsdf");
            // Consoler.ClearConsoleLine();
            // StartGame();
        }

        public class ConsoleCleanManage
        {
            private int SelfCursorTop { get; set; }
            public ConsoleCleanManage()
            {
                RecordPoint();
            }

            public void RecordPoint()
            {
                SelfCursorTop = Console.CursorTop;
            }

            public void CleanALL()
            {
                while (Console.CursorTop >= SelfCursorTop)
                {
                    CleanSingleLine();
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
                Console.WriteLine(string.Empty);
            }

            public void CleanSingleLine()
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string('#', Console.BufferWidth - 1));
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.BufferWidth - 1));
                Console.SetCursorPosition(0, Console.CursorTop);
            }
        }















        private static void RepeatWrite()
        {
            //System.Console.SetCursorPosition(0,4);//定位光标位置，第四行第一位
            //System.Console.CursorTop;//获取已输出文本的行数
            //System.Console.BufferWidth;//获取控制台的宽度
            Console.WriteLine("downloading , wait a moment please ...");
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine("这是第一行一一一一一一一一一");
                Console.WriteLine("这是第二行二二二二二二二二二");
                ConsoleExpand.ClearLineContents(3);
                Console.WriteLine(i + "% downloaded ...");
            }
            Console.WriteLine("\ndownload completely !");
            Console.ReadKey();
        }

        private static void StartGame()
        {
            string[] worlds = GetWorlds();
            Console.WriteLine("开始游戏了:");
            int size = 10;
            do
            {
                string world = RandomData.GetItem(worlds);
                int unfinishedCount = size;
                bool haveError = false;
                while (unfinishedCount > 0)
                {
                    Console.WriteLine($"单词: {world} 请重复写入, 剩余次数: {unfinishedCount - 1}");
                    string inputContent = Console.ReadLine();
                    Console.WriteLine(string.Empty);
                    int lineCount = 2;
                    if (haveError) lineCount++;
                    ConsoleExpand.ClearLineContents(lineCount);
                    // Consoler.ClearConsoleLine(lineCount);
                    if (world == inputContent)
                    {
                        unfinishedCount--;
                        haveError = false;
                    }
                    else
                    {
                        Console.WriteLine($"输入 {inputContent} 错误, 请重新再来一遍!");
                        haveError = true;
                    }
                }
            } while (IsRepeatExecute());
            Console.WriteLine("游戏已结束!");
        }

        private static bool IsRepeatExecute()
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine(@"请输入命令: Q(退出) R(重复执行) C(清空屏幕)");
            ConsoleKeyInfo keyinfo = Console.ReadKey(false);
            Console.WriteLine(string.Empty);
            switch (keyinfo.Key)
            {
                case ConsoleKey.Q:
                    return false;
                case ConsoleKey.R:
                    return true;
                case ConsoleKey.C:
                    Console.Clear();
                    return IsRepeatExecute();
                default:
                    return IsRepeatExecute();
            }
        }
    }
}
