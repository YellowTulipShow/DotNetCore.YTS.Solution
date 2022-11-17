using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YTS.Log.Test
{
    [TestClass]
    public class TestBasicJSONConsolePrintLog
    {
        public sealed class TestCreateLog : BasicJSONConsolePrintLog
        {
            private readonly Action<string[]> action;
            public TestCreateLog(Action<string[]> action)
            {
                this.action = action;
            }
            protected override void PrintLines(params string[] msglist)
            {
                this.action.Invoke(msglist);
            }
        }
        [TestMethod]
        public void TestConnectExecute()
        {
            ILog log = null;
            IDictionary<string, object> logArgs = log.CreateArgDictionary();
            logArgs["name"] = "张三";
            logArgs["age"] = 35;
            logArgs["Time"] = new DateTime(2020, 1, 3, 12, 11, 56);
            IDictionary<string, object>[] logArgsList = new IDictionary<string, object>[]
            {
                logArgs,
            };

            log = new TestCreateLog(strs =>
            {
                Assert.AreEqual(1, strs.Length);
                Assert.AreEqual($"[Info] 测试输出.信息, args: {ToJSON(logArgsList)}", strs[0]);
            });
            log.Info("测试输出.信息", logArgs);

            log = new TestCreateLog(strs =>
            {
                Assert.AreEqual(1, strs.Length);
                Assert.AreEqual($"[Error] 测试输出.错误, args: {ToJSON(logArgsList)}", strs[0]);
            });
            log.Error("测试输出.错误", logArgs);

            Exception ex = new ILogParamException(logArgs, "这是一个测试异常", new ILogParamException(logArgs, "内部测试异常"));
            log = new TestCreateLog(strs =>
            {
                Assert.AreEqual(2, strs.Length);
                Assert.AreEqual($"[ErrorException] 测试输出.异常, Exception: {ex.Message + ex.StackTrace}, args: {ToJSON(logArgsList)}, logParam: {ToJSON(logArgs)}", strs[0]);
                Assert.AreEqual($"[ErrorException.InnerException] 内部测试异常, StackTrace: {ex.InnerException.StackTrace}, logParam: {ToJSON(logArgs)}", strs[1]);
            });
            log.Error("测试输出.异常", ex, logArgs);

            Assert.IsTrue(true);
        }
        private string ToJSON(object args)
        {
            try
            {
                string args_json = JsonConvert.SerializeObject(args);
                return args_json;
            }
            catch (Exception ex)
            {
                return $"序列化参数出错: {ex.Message}";
            }
        }
    }
}
