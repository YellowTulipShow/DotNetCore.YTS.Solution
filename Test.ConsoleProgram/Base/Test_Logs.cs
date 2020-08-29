using System;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    public class Test_Logs : CaseModel
    {
        public Test_Logs()
        {
            this.NameSign = "测试日志";
            this.ExeEvent = () =>
            {
                Console.WriteLine("输出普通日志: Hello Logs Recore!");
                return true;
            };
        }
    }
}
