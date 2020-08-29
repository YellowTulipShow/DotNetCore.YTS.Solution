using System;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    public class HelloWorld : CaseModel
    {
        public HelloWorld()
        {
            this.NameSign = "Hello World!";
            this.ExeEvent = () =>
            {
                Console.WriteLine("Hello World!");
                return true;
            };
        }
    }
}
