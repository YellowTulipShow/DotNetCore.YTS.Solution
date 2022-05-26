using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTS.Tools
{
    /// <summary>
    /// 时间值分割帮助类
    /// </summary>
    public class DateTimeSplit
    {
        public class MTime
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is MTime time)
                {
                    if (this.Start.ToDateTimeString() == time.Start.ToDateTimeString()
                        && this.End.ToDateTimeString() == time.End.ToDateTimeString())
                    {
                        return true;
                    }
                }
                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return $"{this.Start.ToDateTimeString()}-{this.Start.ToDateTimeString()}";
            }
        }

        /// <summary>
        /// 定义分割的部分
        /// </summary>
        public enum ESection
        {
            /// <summary>
            /// 年份
            /// </summary>
            Year,
            /// <summary>
            /// 月份
            /// </summary>
            Month,
            /// <summary>
            /// 日期
            /// </summary>
            Day,
            /// <summary>
            /// 小时
            /// </summary>
            Hour,
            /// <summary>
            /// 分钟
            /// </summary>
            Minute,
            /// <summary>
            /// 秒
            /// </summary>
            Second,
        }

        private readonly ESection section;
        private readonly int stepLength = 1;

        /// <summary>
        /// 初始化定义要分割的部分, 默认步长为 1
        /// </summary>
        /// <param name="section">分割部分</param>
        public DateTimeSplit(ESection section)
        {
            this.section = section;
        }

        /// <summary>
        /// 初始化定义要分割的部分, 步长
        /// </summary>
        /// <param name="section">分割部分</param>
        /// <param name="stepLength">步长</param>
        public DateTimeSplit(ESection section, int stepLength)
        {
            this.section = section;
            this.stepLength = stepLength;
        }

        /// <summary>
        /// 将开始时间与结束时间按指定部分与步长进行分割
        /// </summary>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns>结果</returns>
        public IList<MTime> ToSplitList(DateTime start, DateTime end)
        {
            if (start > end)
            {
                var center = start;
                start = end;
                end = center;
            }
            IList<MTime> list = new List<MTime>();
            DateTime temp = start;
            while (temp <= end)
            {
                DateTime Start, End;
                switch (section)
                {
                    case ESection.Year:
                        Start = new DateTime(temp.Year, 1, 1, 0, 0, 0);
                        End = Start.AddYears(stepLength).AddSeconds(-1);
                        temp = Start.AddYears(stepLength);
                        break;
                    case ESection.Month:
                        Start = new DateTime(temp.Year, temp.Month, 1, 0, 0, 0);
                        End = Start.AddMonths(stepLength).AddSeconds(-1);
                        temp = Start.AddMonths(stepLength);
                        break;
                    case ESection.Day:
                        Start = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0);
                        End = Start.AddDays(stepLength).AddSeconds(-1);
                        temp = Start.AddDays(stepLength);
                        break;
                    case ESection.Hour:
                        Start = new DateTime(temp.Year, temp.Month, temp.Day, temp.Hour, 0, 0);
                        End = Start.AddHours(stepLength).AddSeconds(-1);
                        temp = Start.AddHours(stepLength);
                        break;
                    case ESection.Minute:
                        Start = new DateTime(temp.Year, temp.Month, temp.Day, temp.Hour, temp.Minute, 0);
                        End = Start.AddMinutes(stepLength).AddSeconds(-1);
                        temp = Start.AddMinutes(stepLength);
                        break;
                    case ESection.Second:
                        Start = new DateTime(temp.Year, temp.Month, temp.Day, temp.Hour, temp.Minute, temp.Second);
                        End = Start.AddSeconds(stepLength).AddMilliseconds(-1);
                        temp = Start.AddSeconds(stepLength);
                        break;
                    default:
                        throw new Exception("未定义错误时间部分枚举!");
                }
                list.Add(new MTime()
                {
                    Start = Start,
                    End = End,
                });
            }
            return list;
        }

        /// <summary>
        /// 转为常规中文名称展示
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>结果</returns>
        public string ToConventionalName(MTime time)
        {
            string name = string.Empty;
            switch (section)
            {
                case ESection.Year:
                    name = $"{time.Start.Year}年";
                    break;
                case ESection.Month:
                    name = $"{time.Start.Month.ToString().PadLeft(2, '0')}月";
                    if (stepLength >= 6)
                        name = $"{time.Start.Year}年" + name;
                    break;
                case ESection.Day:
                    name = $"{time.Start.Day.ToString().PadLeft(2, '0')}日";
                    if (stepLength >= 15)
                        name = $"{time.Start.Month.ToString().PadLeft(2, '0')}月" + name;
                    break;
                case ESection.Hour:
                    name = $"{time.Start.Hour}时";
                    break;
                case ESection.Minute:
                    name = $"{time.Start.Minute}分钟";
                    break;
                case ESection.Second:
                    name = $"{time.Start.Second}秒";
                    break;
                default:
                    break;
            }
            return name;
        }
    }
}
