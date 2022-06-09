using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Collections.Generic;

namespace YTS.Log.Test
{
    [TestClass]
    public class TestConnectLog
    {
        public sealed class TestCreateLog : ILog
        {
            private readonly Action<string> action;
            public TestCreateLog(Action<string> action)
            {
                this.action = action;
            }
            public void Info(string message, params IDictionary<string, object>[] args)
            {
                action("Info");
            }
            public void Error(string message, params IDictionary<string, object>[] args)
            {
                action("Error");
            }
            public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
            {
                action("ErrorException");
            }
        }
        [TestMethod]
        public void TestConnectExecute()
        {
            IDictionary<string, int> dict = new Dictionary<string, int>();
            void action(string str)
            {
                if (!dict.ContainsKey(str))
                    dict[str] = 0;
                dict[str]++;
            }

            ILog log = new TestCreateLog(action);
            log.Info("≤‚ ‘Info");
            log.Error("≤‚ ‘Error");
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(1, dict["Info"]);
            Assert.AreEqual(1, dict["Error"]);
            Assert.IsFalse(dict.ContainsKey("ErrorException"));

            log = log.Connect(new TestCreateLog(action), new TestCreateLog(action));
            log.Info("≤‚ ‘Info");
            log.Error("≤‚ ‘Error");
            log.Error("≤‚ ‘ErrorException", (Exception)null, null);
            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(4, dict["Info"]);
            Assert.AreEqual(4, dict["Error"]);
            Assert.AreEqual(3, dict["ErrorException"]);

            dict.Clear();

            ILog log1 = new TestCreateLog(action);
            log1.Info("≤‚ ‘Info");
            log1.Error("≤‚ ‘Error");
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual(1, dict["Info"]);
            Assert.AreEqual(1, dict["Error"]);
            Assert.IsFalse(dict.ContainsKey("ErrorException"));

            log1 = log1.Connect(new TestCreateLog(action), new TestCreateLog(action));
            log1.Info("≤‚ ‘Info");
            log1.Error("≤‚ ‘Error");
            log1.Error("≤‚ ‘ErrorException", (Exception)null, null);
            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(4, dict["Info"]);
            Assert.AreEqual(4, dict["Error"]);
            Assert.AreEqual(3, dict["ErrorException"]);

            ILog log2 = log1.Connect(log);
            log2.Info("≤‚ ‘Info");
            log2.Error("≤‚ ‘Error");
            log2.Error("≤‚ ‘ErrorException", (Exception)null, null);
            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(10, dict["Info"]);
            Assert.AreEqual(10, dict["Error"]);
            Assert.AreEqual(9, dict["ErrorException"]);

            Assert.AreEqual(log1.GetHashCode(), log2.GetHashCode());

            dict.Clear();
            Assert.AreEqual(0, dict.Count);

            log2.Info("≤‚ ‘Info");
            log2.Error("≤‚ ‘Error");
            log2.Error("≤‚ ‘ErrorException", (Exception)null, null);
            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(6, dict["Info"]);
            Assert.AreEqual(6, dict["Error"]);
            Assert.AreEqual(6, dict["ErrorException"]);

            dict.Clear();
            Assert.AreEqual(0, dict.Count);

            log.Info("≤‚ ‘Info");
            log.Error("≤‚ ‘Error");
            log.Error("≤‚ ‘ErrorException", (Exception)null, null);
            Assert.AreEqual(3, dict.Count);
            Assert.AreEqual(3, dict["Info"]);
            Assert.AreEqual(3, dict["Error"]);
            Assert.AreEqual(3, dict["ErrorException"]);
        }
    }
}
