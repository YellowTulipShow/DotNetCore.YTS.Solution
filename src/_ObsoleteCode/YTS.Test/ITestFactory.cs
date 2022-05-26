using System.Collections.Generic;

namespace YTS.Test
{
    /// <summary>
    /// 接口: 测试工厂, 用于批量注册测试实例
    /// </summary>
    public interface ITestFactory
    {
        /// <summary>
        /// 获取测试实例
        /// </summary>
        /// <returns>列表: 测试实例</returns>
        IList<ITestItem> GetItems();
    }

    /// <summary>
    /// 接口: 泛型测试工厂, 为多种测试实例预留的接口
    /// </summary>
    /// <typeparam name="T">测试实例的类型</typeparam>
    public interface ITestFactory<T> : ITestFactory where T : ITestItem
    {
        /// <summary>
        /// 重写数据类型的获取测试实例方法
        /// </summary>
        /// <returns>列表: 泛型测试实例</returns>
        new IList<T> GetItems();
    }
}
