using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTS.Tools
{
    /// <summary>
    /// 接口-测试-实例
    /// </summary>
    public interface ITestCase
    {
        string TestNameSign();
        void TestMethod();
    }

    /// <summary>
    /// 抽象类-测试-实例-可定制子属性类-称为 "大测试类"
    /// </summary>
    public abstract class AbsTestCase : ITestCase
    {
        public abstract string TestNameSign();
        public abstract void TestMethod();
        public virtual ITestCase[] SonTestCase()
        {
            return new ITestCase[] { };
        }
    }
}
