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
                lines.Clear();
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

            Exception ex = new Exception("异常内容标题描述");

            logArgs["aName"] = "张三";
            logArgs["bFileUrl"] = "D:\\Work\\Image\\1.jpg";

            static void Verif(IList<string> expected, IList<string> actual)
            {
                Assert.IsNotNull(expected);
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Count, actual.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    string expected_str = expected[i];
                    string actual_str = actual[i];
                    Assert.AreEqual(expected_str, actual_str);
                }
            };

            log.Info("测试信息1", logArgs);
            Verif(new string[] {
                "[Info] 测试信息1:",
                "    ├── aName: 张三",
                "    └── bFileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Error("测试信息1", logArgs);
            Verif(new string[] {
                "[Error] 测试信息1:",
                "    ├── aName: 张三",
                "    └── bFileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Info("测试错误2", logArgs, logArgs, logArgs);
            Verif(new string[] {
                "[Info] 测试错误2:",
                "    ├── arg[0]:",
                "    |   ├── aName: 张三",
                "    |   └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[1]:",
                "    |   ├── aName: 张三",
                "    |   └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    └── arg[2]:",
                "        ├── aName: 张三",
                "        └── bFileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Error("测试错误2", logArgs, logArgs, logArgs);
            Verif(new string[] {
                "[Error] 测试错误2:",
                "    ├── arg[0]:",
                "    |   ├── aName: 张三",
                "    |   └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[1]:",
                "    |   ├── aName: 张三",
                "    |   └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    └── arg[2]:",
                "        ├── aName: 张三",
                "        └── bFileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            log.Error("测试错误带有异常", ex, logArgs, logArgs, logArgs);
            Verif(new string[] {
                "[ErrorException] 测试错误带有异常:",
                "    ├── Exception:",
                "    |   ├── Message: 异常内容标题描述",
                "    |   ├── Data:",
                "    |   ├── Source:",
                "    |   ├── StackTrace:",
                "    |   └── InnerException:",
                "    ├── arg[0]:",
                "    |   ├── aName: 张三",
                "    |   └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    ├── arg[1]:",
                "    |   ├── aName: 张三",
                "    |   └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    └── arg[2]:",
                "        ├── aName: 张三",
                "        └── bFileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            ILogParamException logParamEx = new ILogParamException(logArgs, "日志参数异常消息", ex);
            log.Error("测试错误带有异常", logParamEx, logArgs);
            Verif(new string[] {
                "[ErrorException] 测试错误带有异常:",
                "    ├── Exception:",
                "    |   ├── Message: 日志参数异常消息",
                "    |   ├── Data:",
                "    |   ├── Source:",
                "    |   ├── StackTrace:",
                "    |   ├── InnerException:",
                "    |   |   ├── Message: 异常内容标题描述",
                "    |   |   ├── Data:",
                "    |   |   ├── Source:",
                "    |   |   ├── StackTrace:",
                "    |   |   └── InnerException:",
                "    |   └── Param:",
                "    |       ├── aName: 张三",
                "    |       └── bFileUrl: D:\\Work\\Image\\1.jpg",
                "    └── arg[0]:",
                "        ├── aName: 张三",
                "        └── bFileUrl: D:\\Work\\Image\\1.jpg",
            }, log.GetMsgLines());

            var model = new StructModel()
            {
                Id = 21,
                Name = "张飞",
                UserID = null,
                AddTime = null,
                UpdateTime = new DateTime(2015, 11, 14, 22, 23, 24),

                Ex = new ArgumentOutOfRangeException("DDD", "StructModel的内容Ex赋值异常类型"),

                StructModels = null,
                StructModelsField = null,
                ClassModels = new ClassModel[] {
                    new ClassModel()
                    {
                        Id = 21,
                        Name = "张飞",
                        UserID = null,
                        UpdateTime = new DateTime(2015, 11, 14, 22, 23, 24),
                        Ex = new ArgumentOutOfRangeException("DDD", "StructModel的内容Ex赋值异常类型"),
                    },
                },
                ClassModelsField = null,
            };
            model.StructModels = new StructModel[] { model };
            logArgs["StructModel"] = model;
            log.Error("测试错误2", ex, logArgs);
            Verif(new string[] {
                "[ErrorException] 测试错误2:",
                "    ├── Exception:",
                "    |   ├── Message: 异常内容标题描述",
                "    |   ├── Data:",
                "    |   ├── Source:",
                "    |   ├── StackTrace:",
                "    |   └── InnerException:",
                "    └── arg[0]:",
                "        ├── aName: 张三",
                "        ├── bFileUrl: D:\\Work\\Image\\1.jpg",
                "        └── StructModel:",
                "            ├── Id: 21",
                "            ├── UserID: null",
                "            ├── AddTime: null",
                "            ├── UpdateTime: 2015-11-14 22:23:24",
                "            ├── Ex:",
                "            |   ├── Message: StructModel的内容Ex赋值异常类型 (Parameter 'DDD')",
                "            |   ├── Data:",
                "            |   ├── Source:",
                "            |   ├── StackTrace:",
                "            |   └── InnerException:",
                "            ├── StructModels:",
                "            |   └── [0]: 重复赋值参数输出",
                "            ├── ClassModels:",
                "            |   └── [0]:",
                "            |       ├── Id: 21",
                "            |       ├── UserID: null",
                "            |       ├── UpdateTime: 2015-11-14 22:23:24",
                "            |       ├── Ex:",
                "            |       |   ├── Message: StructModel的内容Ex赋值异常类型 (Parameter 'DDD')",
                "            |       |   ├── Data:",
                "            |       |   ├── Source:",
                "            |       |   ├── StackTrace:",
                "            |       |   └── InnerException:",
                "            |       └── Name: 张飞",
                "            ├── Name: 张飞",
                "            ├── StructModelsField: null",
                "            └── ClassModelsField: null",
            }, log.GetMsgLines());
        }

        private struct StructModel
        {
            public int Id { get; set; }
            public string Name;
            public long? UserID { get; set; }
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
            public string Name;
            public long? UserID { get; set; }
            public DateTime UpdateTime { get; set; }
            public Exception Ex { get; set; }
        }
    }
}
