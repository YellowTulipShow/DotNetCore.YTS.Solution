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
    }
}
