using System;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    [TestDescription("基础: 测试转换工具 - 转为:数值类型:Decimal")]
    public class Test_ConvertTool_ToDecimal : AbsBaseTestItem
    {
        public override void OnTest()
        {
            Assert.TestExe(ConvertTool.ToDecimal, "35.1", 99M, 35.1M);
            Assert.TestExe(ConvertTool.ToDecimal, "eee", 99M, 99M);
            Assert.TestExe(ConvertTool.ToDecimal, (object)DateTime.Now, 99.2M, 99.2M);
            Assert.TestExe(ConvertTool.ToDecimal, (object)45.2M, 99.2M, 45.2M);
        }
    }
}
