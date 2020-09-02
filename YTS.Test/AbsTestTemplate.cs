using System.Collections.Generic;

namespace YTS.Test
{
    /// <summary>
    /// 抽象实体类: 测试模板 - 泛型版
    /// </summary>
    /// <typeparam name="TItem">接口: 测试实例子级</typeparam>
    public abstract class AbsTestTemplate<TItem> : ITestTemplate<TItem> where TItem : ITestItem
    {
        /// <summary>
        /// 根据定义的模板执行测试实例
        /// </summary>
        /// <param name="item">测试实例</param>
        /// <returns>执行时间</returns>
        public abstract decimal ExecuteTest(TItem item);

        /// <summary>
        /// 根据定义的模板执行测试实例
        /// </summary>
        /// <param name="item">测试实例</param>
        /// <returns>执行时间</returns>
        public decimal ExecuteTest(ITestItem item)
        {
            if (item is TItem)
            {
                TItem model = (TItem)item;
                return ExecuteTest(model);
            }
            return -0.1M;
        }

        /// <summary>
        /// 执行完毕后的结尾总结
        /// </summary>
        /// <param name="item">测试实例</param>
        public abstract void ExecuteEnd(TItem item);

        /// <summary>
        /// 执行完毕后的结尾总结
        /// </summary>
        /// <param name="item">测试实例</param>
        public void ExecuteEnd(ITestItem item)
        {
            if (item is TItem)
            {
                TItem model = (TItem)item;
                ExecuteEnd(model);
            }
        }
    }
}
