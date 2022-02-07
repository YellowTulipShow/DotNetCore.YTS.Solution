using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.Logic.Log;

namespace YTS.Logic.Test.Log
{
    [TestClass]
    public class TestConsolePrintLog
    {
        [TestMethod]
        public void Info()
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>()
            {
                { "Name", "张三" },
                { "fileUrl", "D:\\Work\\Image\\1.jpg" },
            };
            ILog log = new ConsolePrintLog();
            log.Info("测试执行ILog.Info", logArgs);
        }

        [TestMethod]
        public void Error()
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>()
            {
                { "Name", "张三" },
                { "fileUrl", "D:\\Work\\Image\\1.jpg" },
            };
            ILog log = new ConsolePrintLog();
            log.Error("测试执行ILog.Error", logArgs);
        }

        [TestMethod]
        public void ErrorException()
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>()
            {
                { "Name", "张三" },
                { "fileUrl", "D:\\Work\\Image\\1.jpg" },
            };
            ILog log = new ConsolePrintLog();
            Exception exception = new Exception("测试触发的异常");
            log.Error("测试执行ILog.Error<Exception>", exception, logArgs);
        }
    }
}
