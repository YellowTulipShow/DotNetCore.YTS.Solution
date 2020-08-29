using System;
using System.Collections.Generic;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    public class BaseProgram : CaseModel
    {
        public BaseProgram()
        {
            this.NameSign = "测试基本程序!";
            this.SonCases = new List<CaseModel>() {
                new Son1(),
                new Son2(),
                new Son3(),
            };
        }

        public class Son1 : CaseModel
        {
            public Son1()
            {
                this.NameSign = "子程序1";
                this.ExeEvent = () =>
                {
                    Console.WriteLine("只有本体执行程序!");
                    return true;
                };
            }
        }

        public class Son2 : CaseModel
        {
            public Son2()
            {
                this.NameSign = "子程序2";
                this.SonCases = new List<CaseModel>() {
                    new Son1(),
                    new Son1(),
                };
            }

            public override void onInit()
            {
                Console.WriteLine("重写了初始化步骤! 没有自身执行程序, 有两个子程序1!");
            }
        }

        public class Son3 : CaseModel
        {
            public Son3()
            {
                this.NameSign = "子程序3";
                this.SonCases = new List<CaseModel>() {
                    new Son1(),
                    new Son2(),
                    new Son1(),
                    new CaseModel() {
                        NameSign = @"结束子程序",
                        ExeEvent = () =>
                        {
                            Console.WriteLine("返回失败的标识");
                            return false;
                        },
                    },
                };
                this.ExeEvent = () =>
                {
                    Console.WriteLine("有本体执行程序 + 俩个子程序1 + 一个子程序2!");
                    Console.WriteLine("返回失败的标识");
                    return true;
                };
            }
        }
    }
}
