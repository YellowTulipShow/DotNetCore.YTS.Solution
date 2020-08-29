using System;
using System.Linq;
using System.Collections;
using System.Reflection;

namespace YTS.Test
{
    /// <summary>
    /// 测试不通过异常
    /// </summary>
    public class TestFailException : Exception
    {
        public class TestInfo
        {
            public MethodInfo MethodInfo { get; set; }

            public object[] Answer { get; set; }

            public object Result { get; set; }

            public object[] Args { get; set; }
        }

        public TestFailException(string message, TestInfo testInfo) : base(message)
        {
            this.testInfo = testInfo;
        }

        private TestInfo testInfo;
        public TestInfo Info
        {
            get
            {
                return testInfo;
            }
        }

        /// <summary>
        /// 触发测试异常
        /// </summary>
        /// <param name="methodInfo">执行方法类型</param>
        /// <param name="answer">预期结果</param>
        /// <param name="result">计算结果</param>
        /// <param name="args">传入参数</param>
        /// <typeparam name="R">结果数据类型</typeparam>
        public static void OutputTestFail<R>(MethodInfo methodInfo, R[] answer, object result, params object[] args)
        {
            var methodFullName = $"{methodInfo.DeclaringType.FullName} + {methodInfo.Name}";
            throw new TestFailException($"测试方法: {methodFullName} 执行结果不匹配!",
                new TestFailException.TestInfo()
                {
                    MethodInfo = methodInfo,
                    Answer = answer.Select(b => (object)b).ToArray(),
                    Result = result,
                    Args = args,
                });
        }
    }
}
