using System;
using System.Collections.Generic;
using YTS.Tools;

namespace YTS.Test
{
    public class BaseTestFramework : AbsTestFramework<ITestItem>
    {
        public BaseTestFramework(ITestOutput output) : base(output)
        {
        }

        /// <summary>
        /// 是否重复执行
        /// </summary>
        public bool IsRepeatExecute()
        {
            Console.WriteLine(string.Empty);
            Dictionary<ConsoleKey, Func<bool>> keyMethods = new Dictionary<ConsoleKey, Func<bool>>() {
                { ConsoleKey.Q, () => false },
                { ConsoleKey.R, () => true },
                { ConsoleKey.C, () => {
                    Console.Clear();
                    return IsRepeatExecute();
                } }
            };
            Dictionary<ConsoleKey, string> keyExplains = new Dictionary<ConsoleKey, string>() {
                { ConsoleKey.Q, "退出" },
                { ConsoleKey.R, "重复执行" },
                { ConsoleKey.C, "清空屏幕" },
            };

            List<string> commandExplains = new List<string>();
            foreach (ConsoleKey key in keyMethods.Keys)
            {
                string name = key.ToString();
                string explain = keyExplains.GetValue(key, "错误");
                commandExplains.Add(string.Format("{0}({1})", name, explain));
            }
            Console.WriteLine(@"请输入命令: {0}", string.Join(" ", commandExplains.ToArray()));
            ConsoleKeyInfo keyinfo = Console.ReadKey(false);
            Console.WriteLine(string.Empty);

            Func<bool> method = keyMethods.GetValue(keyinfo.Key, () => IsRepeatExecute());
            return method();
        }
    }
}
