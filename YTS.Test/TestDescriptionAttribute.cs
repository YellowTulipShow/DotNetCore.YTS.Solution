using System;
using System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public class TestDescriptionAttribute : DescriptionAttribute
{
    public TestDescriptionAttribute(string description) : base(description) { }
}
