using System;
using System.Collections.Generic;
using System.ComponentModel;
using SystemCommonLibrary.Reflect;
using Xunit;

namespace UnitTest
{
    public enum TestEnum
    { 
        [Description("TestName")]
        Test1 = 0,
        Test2 = 2,
        Test3 = 4
    }

    public class EnumExtensionTest
    {
        [Fact]
        public void GetDescription_Test()
        {
            var test1 = TestEnum.Test1;
            var test2 = TestEnum.Test2;
            var test3 = TestEnum.Test3;

            var val1 = test1.GetDescription();
            var val2 = test2.GetDescription();
            var val3 = test3.GetDescription();

            Assert.Equal("TestName", val1);
            Assert.Equal("Test2", val2);
            Assert.NotEqual("4", val3);
        }
    }
}
