using System;
using Xunit;

namespace YTS.Tools.Test.Time
{
    public class DateTimeExpandTest
    {
        [Theory]
        [InlineData("2021-03-16 12:15:44", "2021-03-15 12:15:44")]
        [InlineData("2021-03-18 12:15:44", "2021-03-15 12:15:44")]
        [InlineData("2021-03-21 12:15:44", "2021-03-15 12:15:44")]
        public void TestGetWeekRegionStart(string timeInput, string timeOutput)
        {
            DateTime time = Convert.ToDateTime(timeInput);
            Assert.Equal(timeOutput, time.GetWeekRegionStart().ToString("yyyy-MM-dd HH:mm:ss"));
        }
        [Theory]
        [InlineData("2021-03-16 12:15:44", "2021-03-21 12:15:44")]
        [InlineData("2021-03-18 12:15:44", "2021-03-21 12:15:44")]
        [InlineData("2021-03-21 12:15:44", "2021-03-21 12:15:44")]
        public void TestGetWeekRegionEnd(string timeInput, string timeOutput)
        {
            DateTime time = Convert.ToDateTime(timeInput);
            Assert.Equal(timeOutput, time.GetWeekRegionEnd().ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
