using System.Collections.Generic;

namespace YTS.Test
{
    /// <summary>
    /// 接口: 测试框架
    /// </summary>
    public interface ITestFramework
    {
        /// <summary>
        /// 初始化事件方法
        /// </summary>
        void OnInit();

        /// <summary>
        /// 根据测试模板处理相应的测试工厂
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="factory">测试工厂</param>
        void OnExecute(ITestTemplate temp, ITestFactory factory);

        /// <summary>
        /// 根据测试模板处理相应的测试实例
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="item">测试实例</param>
        void OnExecute(ITestTemplate temp, ITestItem item);

        /// <summary>
        /// 根据测试模板处理相应的测试实例, 额外的测试描述
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="item">测试实例</param>
        /// <param name="description">测试描述</param>
        void OnExecute(ITestTemplate temp, ITestItem item, string description);

        /// <summary>
        /// 输出测试结果
        /// </summary>
        void OutputEndResult();
    }

    /// <summary>
    /// 接口: 泛型测试框架, 为多种测试实例预留的接口
    /// </summary>
    /// <typeparam name="TItem">测试实例的类型</typeparam>
    public interface ITestFramework<TItem> : ITestFramework where TItem : ITestItem
    {
        void OnExecute(ITestTemplate<TItem> temp, ITestFactory<TItem> factory);

        void OnExecute(ITestTemplate<TItem> temp, TItem item);
    }
}
