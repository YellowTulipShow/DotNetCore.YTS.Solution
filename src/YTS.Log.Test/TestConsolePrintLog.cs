using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace YTS.Log.Test
{
    [TestClass]
    public class TestConsolePrintLog
    {
        private class TestRunLog : ConsolePrintLog
        {
            private readonly List<string> lines;
            public TestRunLog() : base()
            {
                lines = new List<string>();
            }
            protected override void PrintLines(params string[] msglist)
            {
                lines.AddRange(msglist);
            }
            public IList<string> GetMsgLines()
            {
                return lines;
            }
            public void Clear()
            {
                lines.Clear();
            }
        }


        [TestMethod]
        public void TestBasicDataMsg()
        {
            var log = new TestRunLog();
            var logArgs = log.CreateArgDictionary();

            logArgs["Name"] = "张三";
            logArgs["fileUrl"] = "D:\\Work\\Image\\1.jpg";

            log.Info("测试信息1", logArgs);
            Verif(new string[] {
                "[Info] 测试信息1:" +
                "    ├── Name: 张三",
                "    └── fileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Error("测试信息1", logArgs);
            Verif(new string[] {
                "[Error] 测试信息1:" +
                "    ├── Name: 张三",
                "    └── fileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Info("测试错误2", logArgs, logArgs, logArgs);
            Verif(new string[] {
                "[Info] 测试错误2:" +
                "    ├── arg[0]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[1]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    └── arg[2]:",
                "        ├── Name: 张三",
                "        └── fileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Error("测试错误2", logArgs, logArgs, logArgs);
            Verif(new string[] {
                "[Error] 测试错误2:" +
                "    ├── arg[0]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[1]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    └── arg[2]:",
                "        ├── Name: 张三",
                "        └── fileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            Exception ex = new Exception("异常内容标题描述");
            log.Error("测试错误2", ex, logArgs, logArgs, logArgs);
            Verif(new string[] {
                "[ErrorException] 测试错误2:" +
                "    ├── arg[0]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[1]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[2]:",
                "    |   ├── Name: 张三",
                "    |   └── fileUrl: D:\\Work\\Image\\1.jpg",
                "    └── Exception:",
                "        ├── Message: 异常内容标题描述",
                "        ├── Data:",
                "        └── StackTrace:",
                "            ├── [0]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            ├── [1]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            ├── [2]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            ├── [3]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            └── [4]: at YTS.Log.Test(IList`1 rlist) in  110",
            }, log.GetMsgLines());

            logArgs["StructModel"] = new StructModel()
            {
                Id = 21,
                Name = "张飞",
                UpdateTime = new DateTime(2015, 11, 14, 22, 23, 24),
                UserID = null,
                Ex = new ArgumentOutOfRangeException("DDD", "StructModel的内容Ex赋值异常类型"),
                AddTime = null,
                ClassModelsField = null,
                StructModels = null,
                StructModelsField = null,
                ClassModels = new ClassModel[] {
                    new ClassModel()
                    {
                        Id = 21,
                        Name = "张飞",
                        UpdateTime = new DateTime(2015, 11, 14, 22, 23, 24),
                        UserID = null,
                        Ex = new ArgumentOutOfRangeException("DDD", "StructModel的内容Ex赋值异常类型"),
                    },
                },
            };
            log.Error("测试错误2", ex, logArgs);
            Verif(new string[] {
                "[ErrorException] 测试错误2:" +
                "    ├── arg[0]:",
                "    |   ├── Name: 张三",
                "    |   ├── fileUrl: D:\\Work\\Image\\1.jpg",
                "    |   └── StructModel:",
                "    |       ├── Id: 21",
                "    |       ├── Name: 张飞",
                "    |       ├── UpdateTime: 2015-11-14 22:23:24",
                "    |       ├── UserID: null",
                "    |       ├── Ex:",
                "    |       |   ├── Message: (DDD) StructModel的内容Ex赋值异常类型",
                "    |       |   ├── Data:",
                "    |       |   └── StackTrace:",
                "    |       |       ├── [0]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |       ├── [1]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |       ├── [2]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |       ├── [3]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |       └── [4]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       ├── AddTime: null",
                "    |       ├── StructModels: null",
                "    |       ├── StructModelsField: null",
                "    |       ├── ClassModels: null",
                "    |       |   └── [0]:",
                "    |       |       └── Id: 21",
                "    |       |       └── Name: 张飞",
                "    |       |       └── UpdateTime: 2015-11-14 22:23:24",
                "    |       |       └── UserID: null",
                "    |       |       └── Ex:",
                "    |       |           ├── Message: (DDD) StructModel的内容Ex赋值异常类型",
                "    |       |           ├── Data:",
                "    |       |           └── StackTrace:",
                "    |       |               ├── [0]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |               ├── [1]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |               ├── [2]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |               ├── [3]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       |               └── [4]: at YTS.Log.Test(IList`1 rlist) in  110",
                "    |       └── ClassModelsField: null",
                "    └── Exception:",
                "        ├── Message: 异常内容标题描述",
                "        ├── Data:",
                "        └── StackTrace:",
                "            ├── [0]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            ├── [1]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            ├── [2]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            ├── [3]: at YTS.Log.Test(IList`1 rlist) in  110",
                "            └── [4]: at YTS.Log.Test(IList`1 rlist) in  110",
            }, log.GetMsgLines());
        }

        private struct StructModel
        {
            public int Id { get; set; }
            public long? UserID { get; set; }
            public string Name;
            public DateTime? AddTime { get; set; }
            public DateTime UpdateTime { get; set; }

            public Exception Ex { get; set; }

            public ICollection<StructModel> StructModels { get; set; }
            public ICollection<StructModel> StructModelsField;

            public ICollection<ClassModel> ClassModels { get; set; }
            public ICollection<ClassModel> ClassModelsField;
        }

        private class ClassModel
        {
            public int Id { get; set; }
            public long? UserID { get; set; }
            public string Name;
            public DateTime UpdateTime { get; set; }
            public Exception Ex { get; set; }
        }

        /// <summary>
        /// 验证是否相等
        /// </summary>
        /// <param name="expected">预期</param>
        /// <param name="actual">实际</param>
        private void Verif(IList<string> expected, IList<string> actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
