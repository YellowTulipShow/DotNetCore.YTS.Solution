using System;
using System.Text;
using YTS.Test;

namespace Test.ConsoleProgram
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {
            ITestOutput output = new DefaultConsoleOutput();
            output.OnInit();
            ExeTestMain(output);
            output.OnEnd();
        }

        public static void ConsoleConfig()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.Title = @"YTS.Test 控制台程序";
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void ExeTestMain(ITestOutput output)
        {
            ExeBaseTestFramework(output);
        }

        public static void ExeBaseTestFramework(ITestOutput output)
        {
            var baseFramework = new YTS.Test.BaseTestFramework(output);
            baseFramework.OnInit();

            var algorithm = new BaseTestTemplate();
            baseFramework.OnExecute(algorithm, new Libray(output));

            baseFramework.OutputEndResult();
            // 重复调用测试
            // if (baseFramework.IsRepeatExecute()) ExeBaseTestFramework(output);
        }
    }
}
