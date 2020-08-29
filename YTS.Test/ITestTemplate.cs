using System.Collections.Generic;

namespace YTS.Test
{
    public interface ITestTemplate
    {
        decimal ExecuteTest(ITestItem item);

        void ExecuteEnd(ITestItem item);
    }

    public interface ITestTemplate<in T> : ITestTemplate where T : ITestItem
    {
        decimal ExecuteTest(T item);

        void ExecuteEnd(T item);
    }
}
