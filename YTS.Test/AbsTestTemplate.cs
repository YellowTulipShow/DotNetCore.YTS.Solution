using System.Collections.Generic;

namespace YTS.Test
{
    public abstract class AbsTestTemplate<TItem> : ITestTemplate<TItem> where TItem : ITestItem
    {
        public abstract decimal ExecuteTest(TItem item);

        public decimal ExecuteTest(ITestItem item)
        {
            if (item is TItem)
            {
                TItem model = (TItem)item;
                return ExecuteTest(model);
            }
            return -0.1M;
        }

        public abstract void ExecuteEnd(TItem item);

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
