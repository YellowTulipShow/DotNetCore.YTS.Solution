using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataFileIO;
using YTS.IOFile.API.Tools.PathRuleParsing;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Test
{
    [TestClass]
    public class TestGitHelper
    {
        private ILog log;

        [TestInitialize]
        public void Init()
        {
            log = new FilePrintLog($"./logs/TestGitHelper/{DateTime.Now:yyyy_MM_dd}.log", Encoding.UTF8);
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_Pull()
        {
        }

        [TestMethod]
        public void Test_Status()
        {
            ProcessStartInfo info = new ProcessStartInfo("git", "status")
            {
                UseShellExecute = false,
                WorkingDirectory = @"D:\Work\YTS.ZRQ\PlanNotes.YTSZRQ.StorageArea",
                RedirectStandardInput = true,
                //RedirectStandardError = true,
                RedirectStandardOutput = true,
            };
            Process process = Process.Start(info);
            process.BeginOutputReadLine();
            using (var sr = process.StandardOutput)
            {
                string str = sr.ReadToEnd();
                Assert.IsTrue(str != null);
            }
            process.WaitForExit();
            process.Close();
        }

        [TestMethod]
        public void Test_Commit()
        {
        }

        [TestMethod]
        public void Test_Push()
        {
        }
    }
}
