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
using Newtonsoft.Json;

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
        public void Test_ToWrite()
        {
            const string root_dire = @"C:\_code_test\dire";
            // ������Ի���
            Directory.Delete(root_dire, true);
            ruleParsing.SetRoot(root_dire);

            PathResolutionResult result;

            result = ruleParsing.ToWrite(@"plan:list");
            Assert.AreEqual(@"C:\_code_test\dire\plan\list.json", result.AbsolutePathAddress);

            ruleParsing.SetRoot(@"C:\_code_test\dire\");
            result = ruleParsing.ToWrite(@"plan:list");
            Assert.AreEqual(@"C:\_code_test\dire\plan\list.json", result.AbsolutePathAddress);

            Directory.Delete(root_dire, true);
        }

        [TestMethod]
        public void Test_ToRead()
        {
            IList<PathResolutionResult> list;
            PathResolutionResult result;


            const string root_dire = @"C:\_code_test\dire";
            // ������Ի���
            Directory.Delete(root_dire, true);
            ruleParsing.SetRoot(root_dire);

            // ��ʼ��ȡ
            list = ruleParsing.ToRead(@"plan:list");
            Assert.AreEqual(null, list);

            // д������
            string json = JsonConvert.SerializeObject(new { });
            File.WriteAllText(@"C:\_code_test\dire\plan\list.json", json);

            // ��ȡ��ַ
            list = ruleParsing.ToRead(@"plan:list");
            Assert.AreEqual(1, list.Count);
            result = list[0];
            Assert.AreEqual(@"plan:list", result.Key);
            Assert.AreEqual(@"C:\_code_test\dire\plan\list.json", result.AbsolutePathAddress);

            // д��ڶ����ļ�
            File.WriteAllText(@"C:\_code_test\dire\zwang\list.json", json);

            // �ٴζ�ȡ
            list = ruleParsing.ToRead(@"/\w+/i:list");
            Assert.AreEqual(2, list.Count);
            result = list[0];
            Assert.AreEqual(@"plan:list", result.Key);
            Assert.AreEqual(@"C:\_code_test\dire\plan\list.json", result.AbsolutePathAddress);

            result = list[1];
            Assert.AreEqual(@"zwang:list", result.Key);
            Assert.AreEqual(@"C:\_code_test\dire\zwang\list.json", result.AbsolutePathAddress);

            Directory.Delete(root_dire, true);
        }
    }
}
