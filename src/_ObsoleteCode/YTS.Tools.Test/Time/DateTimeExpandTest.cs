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

        [Theory]
        [InlineData("2020-03-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Year, 1)]
        [InlineData("2021-02-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Month, 1)]
        [InlineData("2021-03-15 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Day, 1)]
        [InlineData("2021-03-16 11:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Hour, 1)]
        [InlineData("2021-03-16 12:14:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Minute, 1)]
        [InlineData("2021-03-16 12:15:43", "2021-03-16 12:15:44", DateTimeSplit.ESection.Second, 1)]
        [InlineData("2022-03-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Year, -1)]
        [InlineData("2021-04-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Month, -1)]
        [InlineData("2021-03-17 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Day, -1)]
        [InlineData("2021-03-16 13:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Hour, -1)]
        [InlineData("2021-03-16 12:16:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Minute, -1)]
        [InlineData("2021-03-16 12:15:45", "2021-03-16 12:15:44", DateTimeSplit.ESection.Second, -1)]
        [InlineData("2021-03-17 00:15:44", "2021-03-16 23:15:44", DateTimeSplit.ESection.Hour, -1)]
        public void DateTime_GoBackTime(string outputTimeStr, string inputTimeStr, DateTimeSplit.ESection section, int stepLength)
        {
            DateTime outputTime = Convert.ToDateTime(outputTimeStr);
            DateTime inputTime = Convert.ToDateTime(inputTimeStr);
            Assert.Equal(
                outputTime.ToDateTimeString(),
                inputTime.GoBackTime(section, stepLength).ToDateTimeString());
        }
        [Theory]
        [InlineData("2022-03-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Year, 1)]
        [InlineData("2021-04-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Month, 1)]
        [InlineData("2021-03-17 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Day, 1)]
        [InlineData("2021-03-16 13:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Hour, 1)]
        [InlineData("2021-03-16 12:16:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Minute, 1)]
        [InlineData("2021-03-16 12:15:45", "2021-03-16 12:15:44", DateTimeSplit.ESection.Second, 1)]
        [InlineData("2021-03-17 00:15:44", "2021-03-16 23:15:44", DateTimeSplit.ESection.Hour, 1)]
        [InlineData("2020-03-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Year, -1)]
        [InlineData("2021-02-16 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Month, -1)]
        [InlineData("2021-03-15 12:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Day, -1)]
        [InlineData("2021-03-16 11:15:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Hour, -1)]
        [InlineData("2021-03-16 12:14:44", "2021-03-16 12:15:44", DateTimeSplit.ESection.Minute, -1)]
        [InlineData("2021-03-16 12:15:43", "2021-03-16 12:15:44", DateTimeSplit.ESection.Second, -1)]
        public void DateTime_GoAhead(string outputTimeStr, string inputTimeStr, DateTimeSplit.ESection section, int stepLength)
        {
            DateTime outputTime = Convert.ToDateTime(outputTimeStr);
            DateTime inputTime = Convert.ToDateTime(inputTimeStr);
            Assert.Equal(
                outputTime.ToDateTimeString(),
                inputTime.GoAhead(section, stepLength).ToDateTimeString());
        }
    }
}
