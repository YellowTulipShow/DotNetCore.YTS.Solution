using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataFileIO;
using YTS.IOFile.API.Tools.PathRuleParsing;

using YTS.Git;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Test
{
    [TestClass]
    public class TestPathRuleParsingJSON
    {
        private ILog log;
        private IPathRuleParsingRootConfig ruleParsing;

        [TestInitialize]
        public void Init()
        {
            log = new FilePrintLog($"./logs/TestPathRuleParsingJSON/{DateTime.Now:yyyy_MM_dd}.log", Encoding.UTF8);
            ruleParsing = new PathRuleParsingJSON(log);
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void TestToWriteIOPath()
        {
            PathResolutionResult result;
            ruleParsing.SetRoot(@"C:\_code_test\dire");

            result = ruleParsing.ToWrite(@"plan:list");
            Assert.AreEqual(@"C:\_code_test\dire\plan\list.json", result.AbsolutePathAddress);

            ruleParsing.SetRoot(@"C:\_code_test\dire\");
            result = ruleParsing.ToWrite(@"plan:list");
            Assert.AreEqual(@"C:\_code_test\dire\plan\list.json", result.AbsolutePathAddress);
        }

        [TestMethod]
        public void TestToReadIOPath()
        {
            IDictionary<string, string> dict;
            dict = ruleParsing.ToReadIOPath(@"C:\Work\Dir", @"plan:list");
            Assert.AreEqual(new Dictionary<string, string>() {
                { "plan:list", @"C:\Work\Dir\plan\list.json" }
            }, dict);
        }
    }
}
