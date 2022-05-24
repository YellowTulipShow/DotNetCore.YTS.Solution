using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections;

namespace CSharpLearn.Test.Async
{
    public class TestObjectAsIs
    {
        [Fact]
        public void Exe()
        {
            object value = new List<string>() { "1" };
            IEnumerable arr = value as IEnumerable;
            Assert.NotNull(arr);

            value = new Dictionary<string, object>() { { "name", "уехЩ" } };
            IDictionary dic = value as IDictionary;
            Assert.NotNull(arr);
        }
    }
}
