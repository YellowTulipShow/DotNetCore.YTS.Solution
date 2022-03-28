using System;
using System.IO;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    [TestDescription("基础: 路径转为绝对路径")]
    public class Test_PathToAbs : AbsBaseTestItem
    {
        public override void OnTest()
        {
            Console.WriteLine("AppDomain.CurrentDomain.BaseDirectory: " + AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine("Directory.GetCurrentDirectory(): " + Directory.GetCurrentDirectory());
        }
    }
}
