using YTS.Test;

/// <summary>
/// 抽象基础测试实例类
/// </summary>
public abstract class AbsBaseTestItem : ITestItem
{
    /// <summary>
    /// 执行测试内容
    /// </summary>
    public abstract void OnTest();
}
