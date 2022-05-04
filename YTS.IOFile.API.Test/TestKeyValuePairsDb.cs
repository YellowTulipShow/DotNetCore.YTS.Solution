using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataFileIO;
using YTS.IOFile.API.Tools.PathRuleParsing;

using YTS.Git;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Test
{
    [TestClass]
    public class TestKeyValuePairsDb
    {
        private const string root = "PlanNotes.YTSZRQ";

        private IDictionary<string, StoreConfiguration> storeConfigs;
        private ILog log;

        [TestInitialize]
        public void Init()
        {
            log = new FilePrintLog($"./logs/TestKeyValuePairsDb/{DateTime.Now:yyyy_MM_dd}.log", Encoding.UTF8);
            storeConfigs = new Dictionary<string, StoreConfiguration>
            {
                [root] = new StoreConfiguration()
                {
                    SystemAbsolutePath = @"D:\Work\YTS.ZRQ\PlanNotes.YTSZRQ.StorageArea",
                    DescriptionRemarks = "�ƻ��ʼ�",
                    Git = new Repository()
                    {
                        IsEnable = false,
                    },
                }
            };
        }

        [TestCleanup]
        public void Clean()
        {
        }

        private class Model
        {
            public string Name { get; set; }
        }
        [TestMethod]
        public void TestWrite()
        {
            IPathRuleParsingRootConfig pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO<Model> fileIO = new DataFileIOJSON<Model>();
            KeyValuePairsDb<Model> db = new KeyValuePairsDb<Model>(storeConfigs, log, pathRuleParsing, fileIO);
            int successCount = db.Write(root, new Dictionary<string, Model>()
            {
                ["person:�й�:����"] = new Model { Name = "����" },
                ["person:����:����"] = new Model { Name = "����" },
                ["person:����:����"] = new Model { Name = "����" },
                ["person:�й�:����"] = new Model { Name = "����" },
                ["person:�¹�:����"] = new Model { Name = "����" },
            });
            Assert.AreEqual(5, successCount);
        }
        [TestMethod]
        public void TestRead()
        {
            IDictionary<string, Model> rdict;
            string name;
            IPathRuleParsingRootConfig pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO<Model> fileIO = new DataFileIOJSON<Model>();
            KeyValuePairsDb<Model> db = new KeyValuePairsDb<Model>(storeConfigs, log, pathRuleParsing, fileIO);

            rdict = db.Read(root, "person:�й�:����");
            Assert.AreEqual(1, rdict.Count);
            name = rdict["person:�й�:����"].Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:����:����");
            Assert.AreEqual(1, rdict.Count);
            name = rdict["person:����:����"].Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:����:/[^\\n]+/i");
            Assert.AreEqual(2, rdict.Count);
            name = rdict["person:����:����"].Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:/[��|��]��/i:/[^\\n]+/i");
            Assert.AreEqual(4, rdict.Count);
            name = rdict["person:����:����"].Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:/[��|��]��/i:/[^\\n]+[����]/i");
            Assert.AreEqual(2, rdict.Count);
            name = rdict["person:����:����"].Name;
            Assert.AreEqual("����", name);
        }

        [TestMethod]
        public void TestNumberTypeWriteRead()
        {
            IPathRuleParsingRootConfig pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO fileIO = new DataFileIOJSON();
            KeyValuePairsDb db = new KeyValuePairsDb(storeConfigs, log, pathRuleParsing, fileIO);

            var writeDict = new Dictionary<string, object>()
            {
                ["notes:number:firstNum1"] = 11,
                ["notes:number:firstNum2"] = 11.23,
                ["notes:number:firstNum3"] = 11.232D,
                ["notes:number:firstNum4"] = 11.232M,
                ["notes:number:firstNum5"] = -252.51,
                ["notes:number:firstNum6"] = -252.51M,
            };
            int successCount = db.Write(root, writeDict);
            Assert.AreEqual(writeDict.Count, successCount);

            var readDict = db.Read(root, "notes:number:/firstNum\\d/i");
            Assert.AreEqual(writeDict.Count, readDict.Count);
            foreach (string key in writeDict.Keys)
            {
                Assert.IsTrue(readDict.ContainsKey(key));
                object writeValue = writeDict[key];
                object readValue = readDict[key];
                Assert.AreEqual(writeValue.ToString(), readValue.ToString());
            }
        }

        [TestMethod]
        public void TestWebUse()
        {
            IPathRuleParsingRootConfig pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO fileIO = new DataFileIOJSON();
            KeyValuePairsDb db = new KeyValuePairsDb(storeConfigs, log, pathRuleParsing, fileIO);

            var plan_list = db.Read(root, "plan:list");
            Assert.IsNotNull(plan_list);
            Assert.IsTrue(plan_list.Count == 0);

            var dict = new Dictionary<string, object>()
            {
                { "plan:list", new string[] { "7a9d5f99-ea9c-4afc-93a2-94f492aac49c" } },
                { "plan:normal:7a9d5f99-ea9c-4afc-93a2-94f492aac49c", new
                {
                    AddTime = "Fri Apr 29 2022 15:56:00 GMT+0800 (�й���׼ʱ��)",
                    DescribeRemark = "11",
                    Id = "plan:normal:7a9d5f99-ea9c-4afc-93a2-94f492aac49c",
                    ImportanceLevel = "10",
                    NeedComplateCount = "1",
                    Title = "111",
                }},
            };
            var write_result = db.Write(root, dict);
            Assert.AreEqual(2, write_result);

            var read_result = db.Read(root, "plan:list");
            Assert.IsNotNull(read_result);
            Assert.IsTrue(read_result.Count > 0);
        }
    }
}
