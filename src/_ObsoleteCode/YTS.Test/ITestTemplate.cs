using System.Collections.Generic;

namespace YTS.Test
{
    /// <summary>
    /// 接口: 测试模板
    /// </summary>
    public interface ITestTemplate
    {
        /// <summary>
        /// 根据定义的模板执行测试实例
        /// </summary>
        /// <param name="item">测试实例</param>
        /// <returns>执行时间</returns>
        decimal ExecuteTest(ITestItem item);

        /// <summary>
        /// 执行完毕后的结尾总结
        /// </summary>
        /// <param name="item">测试实例</param>
        void ExecuteEnd(ITestItem item);
    }

    /// <summary>
    /// 接口: 测试模板 - 泛型版
    /// </summary>
    /// <typeparam name="T">接口: 测试实例子级</typeparam>
    public interface ITestTemplate<in T> : ITestTemplate where T : ITestItem
    {
        /// <summary>
        /// 根据定义的模板执行测试实例
        /// </summary>
        /// <param name="item">测试实例</param>
        /// <returns>执行时间</returns>
        decimal ExecuteTest(T item);

        /// <summary>
        /// 执行完毕后的结尾总结
        /// </summary>
        /// <param name="item">测试实例</param>
        void ExecuteEnd(T item);
    }
}
