using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.Logic.Internet;
using YTS.Logic.Log;

using static YTS.Logic.Internet.ImageDownloadHelper;

namespace YTS.Logic.Test.Internet
{
    [TestClass]
    public class TestImageDownloadHelper
    {
        [TestMethod]
        public void Download()
        {
            ILog log = new LogUniversal();
            var helper = new ImageDownloadHelper(log);
            string imgurl = @"https://ytsimg.gitee.io/blog/favicon/TopHead.jpg";
            string filepath = @"C:\test_download_image.jpg";

            ExecuteResultStatue statue = helper.Download(imgurl, filepath);
            Assert.AreEqual(ExecuteResultStatue.Complate, statue);
            Assert.IsTrue(new FileInfo(filepath).Length > 0);
            File.Delete(filepath);

            imgurl += '1';
            statue = helper.Download(imgurl, filepath);
            Assert.AreEqual(ExecuteResultStatue.NotFound404, statue);
            Assert.AreEqual(false, File.Exists(filepath));
        }
    }
}
