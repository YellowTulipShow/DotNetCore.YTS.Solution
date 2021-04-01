using System;
using Xunit;

namespace YTS.Tools.Test.Time
{
    public class DateTimeSplitExpandTest
    {
        [Theory]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:33",
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:33")]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:36",
            @"2021-01-06 21:41:30", @"2021-01-06 21:41:33")]
        [InlineData(
            @"2021-01-06 00:00:00", @"2021-01-06 23:59:59",
            @"2021-01-05 00:00:01", @"2021-01-06 00:00:00")]
        public void MTime_GoBackTime(string inputStart, string inputEnd, string outputStart, string outputEnd)
        {
            var inputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(inputStart),
                End = Convert.ToDateTime(inputEnd),
            };
            var outputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(outputStart),
                End = Convert.ToDateTime(outputEnd),
            };
            var resultTime = inputTime.GoBackTime();
            Assert.Equal(outputTime, resultTime);
        }

        [Theory]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:33",
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:33")]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:36",
            @"2021-01-06 21:41:36", @"2021-01-06 21:41:39")]
        public void MTime_GoAhead(string inputStart, string inputEnd, string outputStart, string outputEnd)
        {
            var inputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(inputStart),
                End = Convert.ToDateTime(inputEnd),
            };
            var outputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(outputStart),
                End = Convert.ToDateTime(outputEnd),
            };
            var resultTime = inputTime.GoAhead();
            Assert.Equal(outputTime, resultTime);
        }

        [Theory]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:33",
            DateTimeSplit.ESection.Day, 1,
            @"2021-01-05 21:41:33", @"2021-01-05 21:41:33")]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:36",
            DateTimeSplit.ESection.Second, 3,
            @"2021-01-06 21:41:30", @"2021-01-06 21:41:33")]
        [InlineData(
            @"2021-01-06 00:00:00", @"2021-01-06 23:59:59",
            DateTimeSplit.ESection.Day, 1,
            @"2021-01-05 00:00:00", @"2021-01-05 23:59:59")]
        public void MTime_GoBackTime_ESection(string inputStart, string inputEnd, DateTimeSplit.ESection section, int stepLength, string outputStart, string outputEnd)
        {
            var inputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(inputStart),
                End = Convert.ToDateTime(inputEnd),
            };
            var outputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(outputStart),
                End = Convert.ToDateTime(outputEnd),
            };
            var resultTime = inputTime.GoBackTime(section, stepLength);
            Assert.Equal(outputTime, resultTime);
        }

        [Theory]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:33",
            DateTimeSplit.ESection.Day, 1,
            @"2021-01-07 21:41:33", @"2021-01-07 21:41:33")]
        [InlineData(
            @"2021-01-06 21:41:33", @"2021-01-06 21:41:36",
            DateTimeSplit.ESection.Second, 3,
            @"2021-01-06 21:41:36", @"2021-01-06 21:41:39")]
        [InlineData(
            @"2021-01-06 00:00:00", @"2021-01-06 23:59:59",
            DateTimeSplit.ESection.Day, 1,
            @"2021-01-07 00:00:00", @"2021-01-07 23:59:59")]
        public void MTime_GoAhead_ESection(string inputStart, string inputEnd, DateTimeSplit.ESection section, int stepLength, string outputStart, string outputEnd)
        {
            var inputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(inputStart),
                End = Convert.ToDateTime(inputEnd),
            };
            var outputTime = new DateTimeSplit.MTime()
            {
                Start = Convert.ToDateTime(outputStart),
                End = Convert.ToDateTime(outputEnd),
            };
            var resultTime = inputTime.GoAhead(section, stepLength);
            Assert.Equal(outputTime, resultTime);
        }
    }
}
