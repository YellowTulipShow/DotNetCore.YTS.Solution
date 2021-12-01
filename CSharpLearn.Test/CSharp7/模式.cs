using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace CSharpLearn.Test.CSharp7
{
    public class 模式
    {
        [Fact]
        public void TestSumPositiveNumbers()
        {
            object obj = 123;
            int sum = 321;
            if (obj is int int_value)
            {
                sum += int_value;
            }
            Assert.Equal(444, sum);

            IEnumerable<object> enumerablelist = new List<object>()
            {
                0,
                new List<int>() { 1,2,3,4,5 },
                100,
                null,
            };
            sum = 0;
            sum += SumPositiveNumbers(enumerablelist);
            Assert.Equal(114, sum);
        }

        private static int SumPositiveNumbers(IEnumerable<object> enumerablelist)
        {
            int sum = 0;
            foreach (object item in enumerablelist)
            {
                switch (item)
                {
                    case int int_value:
                        sum += int_value;
                        break;
                    case IEnumerable<int> int_value_list:
                        sum += int_value_list.Sum();
                        break;
                    case IEnumerable<object> son_list:
                        sum += SumPositiveNumbers(son_list);
                        break;
                    case null:
                        sum -= 1;
                        break;
                    default:
                        break;
                }
            }
            return sum;
        }
    }
}
