using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace CSharpLearn.Test.Version_Two
{
    /// <summary>
    /// µü´úÆ÷
    /// </summary>
    public class Iterator
    {
        [Fact]
        public void ToList()
        {
            IEnumerable<int> answer = new int[] { 1, 2, 3 };
            IEnumerable<int> result = GetResult().ToArray();
            Assert.Equal(answer, result);
        }

        [Fact]
        public void For()
        {
            int i = 0;
            foreach (int item in GetResult())
            {
                i++;
                Assert.Equal(i, item);
            }
        }

        public IEnumerable<int> GetResult()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }
    }
}
