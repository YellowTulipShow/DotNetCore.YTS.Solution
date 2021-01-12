using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YTS.Tools;

namespace YTS.TextGame
{
    /// <summary>
    /// 控制台游戏主体
    /// </summary>
    public class ConsoleGame
    {
        private readonly ApplicationOptions appArgs;
        private readonly ConsoleCleanManage ccm;

        /// <summary>
        /// 初始化创建控制台游戏主体
        /// </summary>
        /// <param name="appArgs">应用程序配置项</param>
        public ConsoleGame(ApplicationOptions appArgs, ConsoleCleanManage ccm)
        {
            this.appArgs = appArgs;
            this.ccm = ccm;
        }

        /// <summary>
        /// 开始一个新的回合游戏
        /// </summary>
        /// <param name="matchesContents">多个匹配项内容</param>
        public void StartRoundGame(IList<MatchesContent> matchesContents)
        {
            foreach (var item in matchesContents)
            {
                ExecuteSingleMatchesContent(item);
            }
        }

        /// <summary>
        /// 执行一个单独匹配项
        /// </summary>
        /// <param name="matchesContent">匹配项</param>
        private void ExecuteSingleMatchesContent(MatchesContent matchesContent)
        {
            int unfinishedCount = appArgs.CountWorkRepeat;
            while (unfinishedCount > 0)
            {
                Console.WriteLine($"单次匹配项请重复写入, 剩余次数: {unfinishedCount - 1}");
                Console.WriteLine($"内容: {matchesContent.Print}");
                string inputContent = Console.ReadLine();
                ccm.CleanALL();
                if (matchesContent.Answer.Equals(inputContent))
                {
                    unfinishedCount--;
                }
                else
                {
                    Console.WriteLine($"输入 {inputContent} 错误, 请重新再来一遍!");
                }
            }
        }

        /// <summary>
        /// 用户决定是否重新开始游戏
        /// </summary>
        public bool IsRepeatExecute()
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
    }
}
