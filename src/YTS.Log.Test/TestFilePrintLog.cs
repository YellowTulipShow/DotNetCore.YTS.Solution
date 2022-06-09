using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Collections.Generic;

namespace YTS.Log.Test
{
    [TestClass]
    public class TestFilePrintLog
    {
        [TestMethod]
        public void TestCreate()
        {
            try
            {
                FileInfo fileInfo = ILogExtend.GetLogFilePath("TestFilePrintLog");
                ILog log = new FilePrintLog(fileInfo, System.Text.Encoding.UTF8);
                log.Info("测试文件信息写入");
                Assert.IsNotNull(log);
                Assert.IsTrue(fileInfo.Exists);
                string fullPath = fileInfo.FullName;
                Assert.IsTrue(!string.IsNullOrEmpty(fullPath));
                fullPath = fullPath.Replace('\\', '/');
                Assert.IsTrue(fullPath.IndexOf("/bin/Debug/") > -1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(false);
            }
        }
    }
}
