using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataSupportIO;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Test
{
    [TestClass]
    public class TestKeyValuePairsDb
    {
        private ILog log;
        private KeyValuePairsDb db;

        [TestInitialize]
        public void Init()
        {
            log = new FilePrintLog($"./logs/TestKeyValuePairsDb/{DateTime.Now:yyyy_MM_dd}.log", Encoding.UTF8);
            var storeConfigs = new Dictionary<string, StoreConfiguration>
            {
                ["PlanNotes.YTSZRQ"] = new StoreConfiguration()
                {
                    SystemAbsolutePath = "D:\\Work\\YTS.IMG\\Bing",
                    DescriptionRemarks = "计划笔记",
                    Git = new StoreConfigurationGit()
                    {
                        IsEnable = false,
                        RemoteWarehouseAddress = "",
                    },
                }
            };
            db = new KeyValuePairsDb(storeConfigs, log);
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
            string root = "PlanNotes.YTSZRQ";
            int successCount = db.Write(root, new Dictionary<string, object>()
            {
                ["person:中国:张三"] = new Model { Name = "张三" },
                ["person:美国:李四"] = new Model { Name = "李四" },
                ["person:美国:田七"] = new Model { Name = "田七" },
                ["person:中国:王五"] = new Model { Name = "王五" },
                ["person:德国:赵六"] = new Model { Name = "赵六" },
            });
            Assert.AreEqual(5, successCount);
        }
        [TestMethod]
        public void TestRead()
        {
            string root = "PlanNotes.YTSZRQ";
            IDictionary<string, object> rdict;

            rdict = db.Read(root, "person:中国:张三");
            Assert.AreEqual(1, rdict.Count);
            string name = (rdict["person:中国:张三"] as Model).Name;
            Assert.AreEqual("张三", name);

            rdict = db.Read(root, "person:美国:田七");
            Assert.AreEqual(1, rdict.Count);
            name = (rdict["person:美国:田七"] as Model).Name;
            Assert.AreEqual("田七", name);

            rdict = db.Read(root, "person:美国:/[^\\n]+/i");
            Assert.AreEqual(2, rdict.Count);
            name = (rdict["person:美国:田七"] as Model).Name;
            Assert.AreEqual("田七", name);

            rdict = db.Read(root, "person:/[中|美]国/i:/[^\\n]+/i");
            Assert.AreEqual(4, rdict.Count);
            name = (rdict["person:美国:田七"] as Model).Name;
            Assert.AreEqual("田七", name);

            rdict = db.Read(root, "person:/[德|美]国/i:/[^\\n]+[六七]/i");
            Assert.AreEqual(2, rdict.Count);
            name = (rdict["person:美国:田七"] as Model).Name;
            Assert.AreEqual("田七", name);
        }
    }
}
