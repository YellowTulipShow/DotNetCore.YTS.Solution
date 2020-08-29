using System;
using System.Collections.Generic;
using YTS.Tools;

namespace Test.ConsoleProgram.TestTools
{
    public class TestJsonHelper : CaseModel
    {
        public TestJsonHelper()
        {
            this.NameSign = "测试JSON解析";
            this.SonCases = new List<CaseModel>()
            {
                new TestToString(),
                new TestToObject(),
                new TestToAnonymousType(),
            };
        }

        public class Model
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class TestToString : CaseModel
        {
            public TestToString()
            {
                this.NameSign = "对象转字符串";
                this.ExeEvent = () =>
                {
                    var rstr = JsonHelper.ToString(new Model()
                    {
                        Name = "张三",
                        Age = 23,
                    });
                    var tstr = "{\"Name\":\"张三\",\"Age\":23}";
                    return Assert.IsEqual(rstr, tstr);
                };
            }
        }

        public class TestToObject : CaseModel
        {
            public TestToObject()
            {
                this.NameSign = "字符串转对象";
                this.ExeEvent = () =>
                {
                    var strmodel = "{Name:\"张三\",Age:\"23\"}";
                    var rmodel = JsonHelper.ToObject<Model>(strmodel);
                    var tmodel = new Model()
                    {
                        Name = "张三",
                        Age = 23,
                    };
                    return Assert.IsEqual(rmodel, tmodel, (rm, tm) => rm.Name == tm.Name && rm.Age == tm.Age);
                };
            }
        }

        public class TestToAnonymousType : CaseModel
        {
            public TestToAnonymousType()
            {
                this.NameSign = "字符串转匿名对象";
                this.ExeEvent = () =>
                {
                    var strmodel = "{Name:\"张三\",Age:\"23\"}";
                    var rmodel = JsonHelper.ToAnonymousType(strmodel, new
                    {
                        Name = "",
                        Age = 0,
                    });
                    var tmodel = new
                    {
                        Name = "张三",
                        Age = 23,
                    };
                    return Assert.IsEqual(rmodel, tmodel, (rm, tm) => rm.Name == tm.Name && rm.Age == tm.Age);
                };
            }
        }
    }
}
