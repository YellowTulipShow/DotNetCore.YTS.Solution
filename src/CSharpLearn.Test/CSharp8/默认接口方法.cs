using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace CSharpLearn.Test.CSharp8
{
    public class 默认接口方法
    {
        [Fact]
        public void 多重继承()
        {
            C c = new();
            IA a = c;
            IB b = c;
            Assert.Equal("C", a.GetName());
            Assert.Equal("C", b.GetName());
            Assert.Equal("C", c.GetName());
            Assert.Equal(17, a.GetAge());
            Assert.Equal(18, b.GetAge());
            
            // c没有重写, 所以在 c 不能使用, 也不会出现无法判断继承谁的问题, 如在 c 中实现重写也就没有这个问题
            //Assert.Equal(18, c.GetAge());
        }

        private interface IA
        {
            string GetName();
            public int GetAge() => 17;
        }
        private interface IB
        {
            public string GetName() => "IB";
            public int GetAge() => 18;
        }
        private class C : IA, IB
        {
            public string GetName() => "C";
        }
    }
}
