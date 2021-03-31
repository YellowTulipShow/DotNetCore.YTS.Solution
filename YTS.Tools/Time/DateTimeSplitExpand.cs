using System;
using static YTS.Tools.DateTimeSplit;

namespace YTS.Tools
{
    /// <summary>
    /// 时间值分割帮助类
    /// </summary>
    public static class DateTimeSplitExpand
    {
        /// <summary>
        /// 回退时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="section">回退的部分</param>
        /// <param name="stepLength">操作的步长</param>
        /// <returns>结果</returns>
        public static DateTime GoBackTime(this DateTime time, ESection section, int stepLength)
        {
            return GoSetTime(time, section, stepLength);
        }
        private static DateTime GoSetTime(DateTime time, ESection section, int stepLength)
        {
            switch (section)
            {
                case ESection.Year:
                    return time.AddYears(stepLength);
                case ESection.Month:
                    return time.AddMonths(stepLength);
                case ESection.Day:
                    return time.AddDays(stepLength);
                case ESection.Hour:
                    return time.AddHours(stepLength);
                case ESection.Minute:
                    return time.AddMinutes(stepLength);
                case ESection.Second:
                    return time.AddSeconds(stepLength);
            }
            return time.AddSeconds(0);
        }
        /// <summary>
        /// 前进时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="section">回退的部分</param>
        /// <param name="stepLength">操作的步长</param>
        /// <returns>结果</returns>
        public static DateTime GoAhead(this DateTime time, ESection section, int stepLength)
        {
            return GoSetTime(time, section, -stepLength);
        }

        /// <summary>
        /// 回退时间区间
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>结果</returns>
        public static MTime GoBackTime(this MTime time)
        {
            TimeSpan span = time.Start - time.End;
            return new MTime()
            {
                Start = time.Start.Add(span),
                End = time.Start,
            };
        }
        /// <summary>
        /// 前进时间区间
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>结果</returns>
        public static MTime GoAhead(this MTime time)
        {
            TimeSpan span = time.End - time.Start;
            return new MTime()
            {
                Start = time.End,
                End = time.End.Add(span),
            };
        }
    }
}
