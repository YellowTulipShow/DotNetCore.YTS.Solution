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
        /// <summary>
        /// 测试结果信息
        /// </summary>
        public class TestInfo
        {
            /// <summary>
            /// 测试方法元素属性信息
            /// </summary>
            /// <value>测试方法属性信息</value>
            public MethodInfo MethodInfo { get; set; }

            /// <summary>
            /// 预期结果答案
            /// </summary>
            /// <value>答案队列</value>
            public object[] Answer { get; set; }

            /// <summary>
            /// 测试结果
            /// </summary>
            /// <value>测试执行完结果</value>
            public object Result { get; set; }

            /// <summary>
            /// 测试的方法传入的值参数
            /// </summary>
            /// <value>值参数</value>
            public object[] Args { get; set; }
        }

        /// <summary>
        /// 初始化异常信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="testInfo">测试信息</param>
        public TestFailException(string message, TestInfo testInfo) : base(message)
        {
            this.Info = testInfo;
        }

        /// <summary>
        /// 初始化传入的测试信息
        /// </summary>
        /// <value>测试信息</value>
        public TestInfo Info { get; private set; }

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
