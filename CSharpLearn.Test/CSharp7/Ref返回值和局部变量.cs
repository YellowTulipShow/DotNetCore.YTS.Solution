using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace CSharpLearn.Test.CSharp7
{
    public class Ref����ֵ�;ֲ�����
    {
        [Fact]
        public void FindNumber()
        {
            var store = new NumberStore();
            Assert.Equal("12345", store.ToString());
            int number = 4;
            
            // ֻ��ȡ���ص�ֵ����
            int only_value = store.FindNumber(number);
            only_value *= 2;
            Assert.Equal(8, only_value);
            Assert.Equal("12345", store.ToString());

            // ��ȡֵ����Ķ���, ���Ķ����ֵ��ͬ���޸���
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
