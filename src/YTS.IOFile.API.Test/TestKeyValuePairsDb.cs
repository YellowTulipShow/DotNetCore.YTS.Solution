using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataFileIO;
using YTS.IOFile.API.Tools.PathRuleParsing;

using YTS.Git;
using YTS.Log;

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
                    SystemAbsolutePath = @"D:\_code_test",
                    DescriptionRemarks = "计划笔记",
                    Git = new Repository()
                    {
                        IsEnable = false,
                    },
                }
            };
            foreach (var key in storeConfigs.Keys)
            {
                string root_dire = storeConfigs[key].SystemAbsolutePath;
                if (Directory.Exists(root_dire))
                {
                    Directory.Delete(root_dire, true);
                }
                Directory.CreateDirectory(root_dire);
            }
        }

        [TestCleanup]
        public void Clean()
        {
            foreach (var key in storeConfigs.Keys)
            {
                string root_dire = storeConfigs[key].SystemAbsolutePath;
                Directory.Delete(root_dire, true);
            }
        }

        private class Model
        {
            public string Name { get; set; }
        }
        [TestMethod]
        public void Test_ReadAndWrite()
        {
            IDictionary<string, Model> rdict;
            string name;
            IPathRuleParsingRootConfig pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO<Model> fileIO = new DataFileIOJSON<Model>();
            IKeyValuePairsDb<Model> db = new KeyValuePairsDb<Model>(storeConfigs, log, pathRuleParsing, fileIO);

            rdict = db.Read(root, "person:中国:张三");
            Assert.AreEqual(null, rdict);

            int successCount = db.Write(root, new Dictionary<string, Model>()
            {
                ["person:中国:张三"] = new Model { Name = "张三" },
                ["person:美国:李四"] = new Model { Name = "李四" },
                ["person:美国:田七"] = new Model { Name = "田七" },
                ["person:中国:王五"] = new Model { Name = "王五" },
                ["person:德国:赵六"] = new Model { Name = "赵六" },
            });
            Assert.AreEqual(5, successCount);

            rdict = db.Read(root, "person:中国:张三");
            Assert.AreEqual(1, rdict.Count);
            name = rdict["person:中国:张三"].Name;
            Assert.AreEqual("张三", name);

            rdict = db.Read(root, "person:美国:田七");
            Assert.AreEqual(1, rdict.Count);
            name = rdict["person:美国:田七"].Name;
            Assert.AreEqual("田七", name);

            rdict = db.Read(root, "person:美国:/[^\\n]+/i");
            Assert.AreEqual(2, rdict.Count);
            name = rdict["person:美国:田七"].Name;
            Assert.AreEqual("田七", name);

            rdict = db.Read(root, "person:/[中|美]国/i:/[^\\n]+/i");
            Assert.AreEqual(4, rdict.Count);
            name = rdict["person:美国:田七"].Name;
            Assert.AreEqual("田七", name);

            rdict = db.Read(root, "person:/[德|美]国/i:/[^\\n]+[六七]/i");
            Assert.AreEqual(2, rdict.Count);
            name = rdict["person:美国:田七"].Name;
            Assert.AreEqual("田七", name);
        }

        [TestMethod]
        public void TestNumberTypeWriteRead()
        {
            IPathRuleParsingRootConfig pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO fileIO = new DataFileIOJSON();
            IKeyValuePairsDb db = new KeyValuePairsDb(storeConfigs, log, pathRuleParsing, fileIO);

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
    }
}
