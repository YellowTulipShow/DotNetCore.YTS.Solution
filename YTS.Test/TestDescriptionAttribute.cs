using System;
using System.ComponentModel;

/// <summary>
/// 测试描述属性描述
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class TestDescriptionAttribute : DescriptionAttribute
{
    /// <summary>
    /// 初始化描述
    /// </summary>
    /// <param name="description">描述内容</param>
    public TestDescriptionAttribute(string description) : base(description) { }
}
