using System;
using System.Collections.Generic;

namespace YTS.Tools
{
    public class TestCaseLibray
    {
        public TestCaseLibray() { }

        /// <summary>
        /// 在这里面手动设置要测试的实例
        /// </summary>
        private AbsTestCase[] InitTestCaseSource()
        {
            return new AbsTestCase[] {
            };
        }

        /// <summary>
        /// 自动获取测试实例-(不用更改)
        /// </summary>
        public ITestCase[] GetTestCase(bool isGetITestCase)
        {
            AbsTestCase[] initCases = InitTestCaseSource();
            if (!isGetITestCase)
                return initCases;

            List<ITestCase> list = new List<ITestCase>();
            foreach (AbsTestCase item in initCases)
            {
                if (CheckData.IsObjectNull(item))
                    continue;
                list.Add(item);
                ITestCase[] sonCase = item.SonTestCase();
                if (!CheckData.IsSizeEmpty(sonCase))
                    list.AddRange(sonCase);
            }
            return list.ToArray();
        }
    }
}
