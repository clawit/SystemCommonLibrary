using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SystemCommonLibrary.Convertion;

namespace UnitTest
{
    public class DateTimeConvertExtensionsTest
    {
        [Fact]
        public void ToUnixEpoch_Test()
        {
            DateTime dt = new DateTime(2020, 11, 28, 21, 54, 32, 321, DateTimeKind.Local);
            var unixTimestamp = dt.ToUnixEpoch();

            Assert.Equal(1606571672.321, unixTimestamp);
        }

        [Fact]
        public void ToLocalTime_test()
        {
            var dt = 1606571672.321.ToLocalTime();
            DateTime expected = new DateTime(2020, 11, 28, 21, 54, 32, 321, DateTimeKind.Local);

            Assert.Equal(expected, dt);
        }
    }
}
