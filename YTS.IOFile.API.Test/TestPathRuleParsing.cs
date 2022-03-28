using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataSupportIO;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Test
{
    [TestClass]
    public class TestPathRuleParsing
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void TestWriteAndRead()
        {
            ILog log = new FilePrintLog($"./logs/TestPathRuleParsing/{DateTime.Now:yyyy_MM_dd}.log", Encoding.UTF8);
            var dataSupport = DataSupportIOFactory.Default();
            PathRuleParsing parsing = new PathRuleParsing(dataSupport, log);

            var root = "DbSaveRegion";
            var key = $"notes:programming:dotnet:IOFile:write";
            var path = parsing.ToWriteIOPath(root, key);
            Assert.IsTrue(!string.IsNullOrEmpty(path));
            //var answer = $"";
            //Assert.AreEqual(answer, path);
        }
    }
}
