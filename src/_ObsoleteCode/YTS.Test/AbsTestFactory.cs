using System.Linq;
using System.Collections.Generic;

namespace YTS.Test
{
    /// <summary>
    /// 抽象类: 泛型测试工厂, 为多种测试实例预留的接口
    /// </summary>
    /// <typeparam name="TItem">接口: 测试实例</typeparam>
    public abstract class AbsTestFactory<TItem> : ITestFactory<TItem> where TItem : ITestItem
    {
        /// <summary>
        /// 提供测试实例
        /// </summary>
        /// <returns>测试实例列表</returns>
        public abstract IList<TItem> GetItems();

        /// <summary>
        /// 提供测试实例
        /// </summary>
        /// <returns>测试实例列表</returns>
        IList<ITestItem> ITestFactory.GetItems()
        {
            IList<TItem> list = GetItems();
            return list.Select(m => (ITestItem)m).ToList();
        }
    }
}
