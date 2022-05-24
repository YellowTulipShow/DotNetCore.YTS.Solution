using System;
using System.IO;
using CommandLine;
using YTS.Tools;

namespace YTS.TextGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string config_path = "_config.json";
            if (File.Exists(config_path))
            {
                string text = File.ReadAllText(config_path);
                var option = JsonHelper.ToObject<ApplicationOptions>(text);
                Run(option);
                return;
            }
            Parser.Default.ParseArguments<ApplicationOptions>(args).WithParsed(Run);
        }

        private static void Run(ApplicationOptions option)
        {
            try
            {
                Console.WriteLine("开始游戏了:");
                var fileContentManage = new FileContentManage(option);
                var ccm = new ConsoleCleanManage();
                var game = new ConsoleGame(option, ccm);
                do
                {
                    var list = fileContentManage.GetRandomMatchesContents();
                    game.StartRoundGame(list);
                } while (game.IsRepeatExecute());
                Console.WriteLine("游戏已结束!");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"错误: {ex.Message}");
            }
        }
    }
}
