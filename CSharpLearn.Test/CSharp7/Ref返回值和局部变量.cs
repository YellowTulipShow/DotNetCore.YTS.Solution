using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace CSharpLearn.Test.CSharp7
{
    public class Ref返回值和局部变量
    {
        [Fact]
        public void FindNumber()
        {
            var store = new NumberStore();
            Assert.Equal("12345", store.ToString());
            int number = 4;
            
            // 只获取返回的值内容
            int only_value = store.FindNumber(number);
            only_value *= 2;
            Assert.Equal(8, only_value);
            Assert.Equal("12345", store.ToString());

            // 获取值代表的对象, 更改对象的值就同步修改了
            ref int value = ref store.FindNumber(number);
            value *= 2;
            Assert.Equal("12385", store.ToString());
        }

        private class NumberStore
        {
            int[] numbers = { 1, 2, 3, 4, 5 };

            public ref int FindNumber(int target)
            {
                for (int ctr = 0; ctr < numbers.Length; ctr++)
                {
                    if (numbers[ctr] >= target)
                        return ref numbers[ctr];
                }
                return ref numbers[0];
            }

            public override string ToString() => string.Join("", numbers);
        }
    }
}
