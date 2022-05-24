using YTS.Tools;

namespace YTS.Test
{
    /// <summary>
    /// 基础测试模板
    /// </summary>
    public class BaseTestTemplate : AbsTestTemplate<AbsBaseTestItem>
    {
        /// <summary>
        /// 初始化基础测试模板
        /// </summary>
        public BaseTestTemplate() { }

        /// <summary>
        /// 执行完毕后的结尾总结
        /// </summary>
        /// <param name="item">测试实例</param>
        public override void ExecuteEnd(AbsBaseTestItem item) { }

        /// <summary>
        /// 根据定义的模板执行测试实例
        /// </summary>
        /// <param name="item">测试实例</param>
        /// <returns>执行时间</returns>
        public override decimal ExecuteTest(AbsBaseTestItem item)
        {
            double time_exe = RunHelp.GetRunTime(() =>
            {
                item.OnTest();
            });
            return (decimal)time_exe;
        }
    }
}
