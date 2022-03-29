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
                    DescriptionRemarks = "�ƻ��ʼ�",
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
            string root = "PlanNotes.YTSZRQ";
            IDictionary<string, object> rdict;

            rdict = db.Read(root, "person:�й�:����");
            Assert.AreEqual(1, rdict.Count);
            string name = (rdict["person:�й�:����"] as Model).Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:����:����");
            Assert.AreEqual(1, rdict.Count);
            name = (rdict["person:����:����"] as Model).Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:����:/[^\\n]+/i");
            Assert.AreEqual(2, rdict.Count);
            name = (rdict["person:����:����"] as Model).Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:/[��|��]��/i:/[^\\n]+/i");
            Assert.AreEqual(4, rdict.Count);
            name = (rdict["person:����:����"] as Model).Name;
            Assert.AreEqual("����", name);

            rdict = db.Read(root, "person:/[��|��]��/i:/[^\\n]+[����]/i");
            Assert.AreEqual(2, rdict.Count);
            name = (rdict["person:����:����"] as Model).Name;
            Assert.AreEqual("����", name);
        }
    }
}
