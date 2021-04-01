using System;

namespace YTS.Tools
{
    /// <summary>
    /// 时间扩展帮助
    /// </summary>
    public static class DateTimeExpand
    {
        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime dateTime, bool isRemoveSecond = false)
        {
            if (isRemoveSecond)
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，带时分秒，格式："yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="isRemoveSecond">是否移除秒</param>
        public static string ToDateTimeString(this DateTime? dateTime, bool isRemoveSecond = false)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateTimeString(dateTime.Value, isRemoveSecond);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 获取格式化字符串，带毫秒，格式："yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToMillisecondString(this DateTime? dateTime, string v)
        {
            if (dateTime == null)
                return string.Empty;
            return ToMillisecondString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy年MM月dd日"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToChineseDateString(this DateTime dateTime)
        {
            return string.Format("{0}年{1}月{2}日", dateTime.Year, dateTime.Month, dateTime.Day);
        }

        /// <summary>
        /// 获取周范围时间开始日期
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>开始日期, 时分秒值随原数据不变</returns>
        public static DateTime GetWeekRegionStart(this DateTime time)
        {
            int week = (int)time.DayOfWeek;
            week = week == 0 ? 7 : week;
            return time.AddDays(-week + 1);
        }
        /// <summary>
        /// 获取周范围时间结束日期
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>结束日期, 时分秒值随原数据不变</returns>
        public static DateTime GetWeekRegionEnd(this DateTime time)
        {
            int week = (int)time.DayOfWeek;
            week = week == 0 ? 7 : week;
            return time.AddDays(7 - week);
        }

        /// <summary>
        /// 回退时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="section">回退的部分</param>
        /// <param name="stepLength">操作的步长</param>
        /// <returns>结果</returns>
        public static DateTime GoBackTime(DateTime time, DateTimeSplit.ESection section, int stepLength)
        {
            return GoSetTime(time, section, stepLength);
        }
        private static DateTime GoSetTime(DateTime time, DateTimeSplit.ESection section, int stepLength)
        {
            switch (section)
            {
                case DateTimeSplit.ESection.Year:
                    return time.AddYears(stepLength);
                case DateTimeSplit.ESection.Month:
                    return time.AddMonths(stepLength);
                case DateTimeSplit.ESection.Day:
                    return time.AddDays(stepLength);
                case DateTimeSplit.ESection.Hour:
                    return time.AddHours(stepLength);
                case DateTimeSplit.ESection.Minute:
                    return time.AddMinutes(stepLength);
                case DateTimeSplit.ESection.Second:
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
        public static DateTime GoAhead(DateTime time, DateTimeSplit.ESection section, int stepLength)
        {
            return GoSetTime(time, section, -stepLength);
        }
    }
}
