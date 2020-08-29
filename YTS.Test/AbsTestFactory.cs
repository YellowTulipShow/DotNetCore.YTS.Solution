using System.Linq;
using System.Collections.Generic;

namespace YTS.Test
{
    public abstract class AbsTestFactory<TItem> : ITestFactory<TItem> where TItem : ITestItem
    {
        public abstract IList<TItem> GetItems();

        IList<ITestItem> ITestFactory.GetItems()
        {
            IList<TItem> list = GetItems();
            return list.Select(m => (ITestItem)m).ToList();
        }
    }
}
