using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools.PathRuleParsing;

using Newtonsoft.Json;
using YTS.Log;

namespace YTS.IOFile.API.Test
{
    [TestClass]
    public class TestPathRuleParsingJSON
    {
        public const string root_dire = @"D:\_code_test";

        private ILog log;
        private IPathRuleParsingRootConfig ruleParsing;

        [TestInitialize]
        public void Init()
        {
            log = new FilePrintLog($"./logs/TestPathRuleParsingJSON/{DateTime.Now:yyyy_MM_dd}.log", Encoding.UTF8);
            ruleParsing = new PathRuleParsingJSON(log);

            // 清理测试环境
            if (Directory.Exists(root_dire))
            {
                Directory.Delete(root_dire, true);
            }
        }

        [TestCleanup]
        public void Clean()
        {
            Directory.Delete(root_dire, true);
        }

        [TestMethod]
        public void Test_ToWrite()
        {
            ruleParsing.SetRoot(root_dire);

            PathResolutionResult result;
            result = ruleParsing.ToWrite(@"plan:list");
            Assert.AreEqual($"{root_dire}\\_data\\plan\\list.json", result.AbsolutePathAddress);

            ruleParsing.SetRoot($"{root_dire}\\");
            result = ruleParsing.ToWrite(@"plan:list");
            Assert.AreEqual($"{root_dire}\\_data\\plan\\list.json", result.AbsolutePathAddress);
        }

        [TestMethod]
        public void Test_ToRead()
        {
            IList<PathResolutionResult> list;
            PathResolutionResult result;

            ruleParsing.SetRoot(root_dire);

            // 开始获取
            list = ruleParsing.ToRead(@"plan:list");
            Assert.AreEqual(null, list);

            // 写入内容
            string json = JsonConvert.SerializeObject(new { });
            FileExtend.WriteAllText($"{root_dire}\\_data\\plan\\list.json", json);

            // 读取地址
            list = ruleParsing.ToRead(@"plan:list");
            Assert.AreEqual(1, list.Count);
            result = list[0];
            Assert.AreEqual(@"plan:list", result.Key);
            Assert.AreEqual($"{root_dire}\\_data\\plan\\list.json", result.AbsolutePathAddress);

            // 写入第二个文件
            FileExtend.WriteAllText($"{root_dire}\\_data\\zwang\\list.json", json);

            // 再次读取
            list = ruleParsing.ToRead(@"/\w+/i:list");
            Assert.AreEqual(2, list.Count);
            result = list[0];
            Assert.AreEqual(@"plan:list", result.Key);
            Assert.AreEqual($"{root_dire}\\_data\\plan\\list.json", result.AbsolutePathAddress);

            result = list[1];
            Assert.AreEqual(@"zwang:list", result.Key);
            Assert.AreEqual($"{root_dire}\\_data\\zwang\\list.json", result.AbsolutePathAddress);
        }
    }
}
