using System;
using System.Collections;
using System.Collections.Generic;
using YTS.Logic;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    [TestDescription("基础: 测试转换工具 - 转为:数值类型:Decimal")]
    public class Test_ILogicILog : AbsBaseTestItem
    {
        public override void OnTest()
        {
            Console.WriteLine(string.Empty);
            ILog log = new ConsolePrintLog();
            IDictionary<string, object> logArgs = new Dictionary<string, object>()
            {
                { "Name", "张三" },
                { "fileUrl", "D:\\Work\\Image\\1.jpg" },
                { "数值", 154123.5123M },
                { "空值", null },
                { "空字符串", string.Empty },
                { "单个空白字符串", " " },
                { "匿名对象", new {
                    Sum = 22,
                    Language = "EN",
                    Data = new {
                        Email = "1142@qq.com"
                    }
                } },
            };
            log.Info("测试执行ILog.Info", logArgs);
            log.Error("测试执行ILog.Error", logArgs);
            Exception exception = new Exception("测试触发的异常");
            log.Error("测试执行ILog.Error<Exception>", exception, logArgs);
        }
    }
}
