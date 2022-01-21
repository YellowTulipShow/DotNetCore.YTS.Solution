using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace YTS.Logic.Test
{
    [TestClass]
    public class TestConsolePrintLog
    {
        [TestMethod]
        public void Info()
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>() { };
            ILog log = new ConsolePrintLog();
            log.Info("≤‚ ‘÷¥––ILog.Info", logArgs);
        }

        [TestMethod]
        public void Error()
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>() { };
            ILog log = new ConsolePrintLog();
            log.Error("≤‚ ‘÷¥––ILog.Error", logArgs);
        }

        [TestMethod]
        public void ErrorException()
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>() { };
            ILog log = new ConsolePrintLog();
            Exception exception = new Exception("≤‚ ‘¥•∑¢µƒ“Ï≥£");
            log.Error("≤‚ ‘÷¥––ILog.Error<Exception>", exception, logArgs);
        }
    }
}
