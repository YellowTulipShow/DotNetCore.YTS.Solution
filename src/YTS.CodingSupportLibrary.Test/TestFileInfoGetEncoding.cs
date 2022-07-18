using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Text;

using YTS.Log;

namespace YTS.CodingSupportLibrary.Test
{
    [TestClass]
    public class TestFileInfoGetEncoding
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logEncoding = new UTF8Encoding(false);
            var logFile = ILogExtend.GetLogFilePath("TestISupport");
            log = new FilePrintLog(logFile, logEncoding);

            // 测试开始前引用官方代码页引用, 增加支持中文GBK
            UseExtend.SupportCodePages();
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_FindFileEncoding()
        {
            // 英文
            string[] text_en = new string[] {
                @"<>33,32,2.52.",
                @"/// sLiellekg <summary>",
                @"/// lsdijfliajlsdjlfajg",
                @"* d -f--23+f+ddfjiljsig* OUO8+",
                @"u3li23k(&@L@)*!??SOF_W@_L@|>:OL~~!``;fdi12+d-3+lsieig"
            };
            Encoding[] encodings_en = new Encoding[] {
                Encoding.UTF32,
                Encoding.Unicode,
                Encoding.BigEndianUnicode,
                Encoding.UTF8,
                new UTF8Encoding(true),
                new UTF8Encoding(false),
                Encoding.GetEncoding("GBK"),

                Encoding.ASCII,
            };
            Test_FindFileEncoding(text_en, encodings_en);

            // 中文
            string[] text_zh = new string[] {
                @"///撒旦<summary>",
                @"JSON在线 | JSON解析格式化—SO JSON在线工具",
                @"打款给几^@个姐姐*#@(给几个i姐姐给个姐姐格局",
                @"而立+*-+即给",
                @"善良的看看/*-+",

                @"<>33,32,2.52.",
                @"/// sLiellekg <summary>",
                @"/// lsdijfliajlsdjlfajg",
                @"* d -f--23+f+ddfjiljsig* OUO8+",
                @"u3li23k(&@L@)*!??SOF_W@_L@|>:OL~~!``;fdi12+d-3+lsieig"
            };
            Encoding[] encodings_zh = new Encoding[] {
                Encoding.UTF32,
                Encoding.Unicode,
                Encoding.BigEndianUnicode,
                Encoding.UTF8,
                new UTF8Encoding(true),
                new UTF8Encoding(false),
                Encoding.GetEncoding("GBK"),
            };
            Test_FindFileEncoding(text_zh, encodings_zh);
        }
        private void Test_FindFileEncoding(string[] texts, Encoding[] encodings)
        {
            foreach (var encoding in encodings)
            {
                Test_FindFileEncoding(string.Join('\n', texts), encoding);
                foreach (string text in texts)
                {
                    Test_FindFileEncoding(text, encoding);
                }
            }
        }
        private void Test_FindFileEncoding(string text, Encoding encoding)
        {
            string name = encoding.EncodingName;
            FileInfo codeFile = new FileInfo(Path.Combine(Environment.CurrentDirectory,
                $"./codefiles/{name}.txt"));
            if (!codeFile.Directory.Exists)
            {
                codeFile.Directory.Create();
            }
            File.WriteAllText(codeFile.FullName, text, encoding);
            
            log.Info(codeFile.GetWriteLogFileContentBytesContent());

            //return;
            // 开始判断内容编码逻辑
            Encoding fileEncoding = codeFile.GetEncoding();
            Assert.IsNotNull(fileEncoding);
            // 任何编码都支持最基本的ASCII编码, 如果判断文本内容都是单字节ASCII编码, 使用任何编码读取结果都是一样的
            if (fileEncoding.Equals(Encoding.ASCII))
                fileEncoding = encoding;
            Assert.AreEqual(encoding.BodyName, fileEncoding.BodyName);
            Assert.AreEqual(encoding.HeaderName, fileEncoding.HeaderName);
            Assert.AreEqual(encoding.WebName, fileEncoding.WebName);
            Assert.AreEqual(encoding.EncodingName, fileEncoding.EncodingName);
            string fileContent = File.ReadAllText(codeFile.FullName, fileEncoding);
            Assert.AreEqual(text, fileContent);
        }

        [TestMethod]
        public void Test_PrintBIN()
        {
            uint[] values = new uint[] {
                0b_1000_0000,
                0b_1100_0000,
                0b_1110_0000,
                0b_1111_0000,
                0b_1111_1000,
                0b_1111_1100,
                0b_1111_1110,
                0b_1111_1111,
            };
            StringBuilder str = new StringBuilder();
            str.AppendLine();
            for (int i = 0; i < values.Length; i++)
            {
                uint v = values[i];
                string bin = Convert.ToString(v, 2).PadLeft(8, '0');
                str.AppendLine($"  v: {v}  bin: {bin}");
            }

            log.Info(str.ToString());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_UTF8_UTF8NoBOM_IsEqual()
        {
            Encoding utf8 = new UTF8Encoding(true);
            Encoding utf8NoBOM = new UTF8Encoding(false);

            Assert.AreNotEqual(utf8, utf8NoBOM);
        }
    }
}
