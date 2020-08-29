using YTS.Tools;

namespace YTS.Test
{
    public class BaseTestTemplate : AbsTestTemplate<AbsBaseTestItem>
    {
        public BaseTestTemplate() { }

        public override void ExecuteEnd(AbsBaseTestItem item)
        {
        }

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
