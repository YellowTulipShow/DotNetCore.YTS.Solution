using System;
using CommandLine;
using YTS.Tools;

namespace YTS.TextGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ApplicationOptions>(args).WithParsed(Run);
        }

        private static void Run(ApplicationOptions option)
        {
            Console.WriteLine("开始游戏了:");
            var fileContentManage = new FileContentManage(option);
            var ccm = new ConsoleCleanManage();
            var game = new ConsoleGame(option, ccm);
            do
            {
                var list = fileContentManage.GetRandomMatchesContents();
                foreach (var item in list)
                {
                    Console.WriteLine($"item.Input: {item.Input}");
                    Console.WriteLine($"item.Print: {item.Print}");
                    Console.WriteLine($"item.Answer: {item.Answer}");
                    Console.WriteLine();
                }
                // game.StartRoundGame(list);
                // break;
            } while (game.IsRepeatExecute());
            Console.WriteLine("游戏已结束!");
        }
    }
}
