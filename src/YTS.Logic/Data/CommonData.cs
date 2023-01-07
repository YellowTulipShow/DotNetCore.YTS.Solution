using System;
using System.Collections.Generic;
using System.Text;

namespace YTS.Logic.Data
{
    /// <summary>
    /// 常用数据
    /// </summary>
    public static class CommonData
    {
        /// <summary>
        /// 创建一个全局的随机数生成器 可以确保在调用生成随机数时, 非重复!
        /// </summary>
        public static readonly Random R = new Random();

        #region ====== ASCII Code: ======
        /// <summary>
        /// ASCII 所有常用 字符 33-127
        /// </summary>
        public static char[] ASCII_ALL() {
            return ASCII_IndexRegion(33, 127);
        }
        /// <summary>
        /// ASCII 常用文本字符
        /// </summary>
        public static char[] ASCII_WordText() {
            List<char> charArr = new List<char>();
            charArr.AddRange(ASCII_Number());
            charArr.AddRange(ASCII_LowerEnglish());
            charArr.AddRange(ASCII_UpperEnglish());
            return charArr.ToArray();
        }
        /// <summary>
        /// 阿拉伯数字
        /// </summary>
        public static char[] ASCII_Number() {
            return ASCII_IndexRegion(48, 58);
        }
        /// <summary>
        /// 大写英文
        /// </summary>
        public static char[] ASCII_UpperEnglish() {
            return ASCII_IndexRegion(65, 91);
        }
        /// <summary>
        /// 小写英文
        /// </summary>
        public static char[] ASCII_LowerEnglish() {
            return ASCII_IndexRegion(97, 123);
        }
        /// <summary>
        /// 特别字符
        /// </summary>
        public static char[] ASCII_Special() {
            List<char> charArr = new List<char>();
            charArr.AddRange(ASCII_IndexRegion(33, 48));
            charArr.AddRange(ASCII_IndexRegion(58, 65));
            charArr.AddRange(ASCII_IndexRegion(91, 97));
            charArr.AddRange(ASCII_IndexRegion(123, 127));
            return charArr.ToArray();
        }
        /// <summary>
        /// ASCII 码指定范围获取对应的字符
        /// </summary>
        /// <param name="min">最小值(包含)</param>
        /// <param name="max">最大值(不包含)</param>
        public static char[] ASCII_IndexRegion(int min, int max) {
            List<char> cl = new List<char>();
            byte[] array = new byte[1];
            for (int i = min; i < max; i++) {
                array[0] = (byte)i; //ASCII码强制转换二进制
                string str = Encoding.ASCII.GetString(array);
                cl.Add(Convert.ToChar(str));
            }
            return cl.ToArray();
        }
        #endregion

        /// <summary>
        /// ASCII 码十六进制组成字符
        /// </summary>
        public static char[] ASCII_Hexadecimal() {
            return new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        }

        /// <summary>
        /// Unicode 中文字符集 最小 十六进制字符串
        /// </summary>
        public static string Unicode_Chinese_MIN_Hexadecimal() {
            return @"4e00";
        }
        /// <summary>
        /// Unicode 中文字符集 最小 十进制标识
        /// </summary>
        public static int Unicode_Chinese_MIN_Decimal() {
            return ConvertTool.HexadecimalToDecimal(Unicode_Chinese_MIN_Hexadecimal());
        }

        /// <summary>
        /// Unicode 中文字符集 最大 十六进制字符串
        /// </summary>
        /// <returns></returns>
        public static string Unicode_Chinese_MAX_Hexadecimal() {
            return @"9fa5";
        }
        /// <summary>
        /// Unicode 中文字符集 最大 十进制标识
        /// </summary>
        /// <returns></returns>
        public static int Unicode_Chinese_MAX_Decimal() {
            return ConvertTool.HexadecimalToDecimal(Unicode_Chinese_MAX_Hexadecimal());
        }

        /// <summary>
        /// 获得最大的天数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>条件的最大天数</returns>
        public static int GetMaxDayCount(int year, int month) {
            if (month == 2) {
                int calc_num = year % 100 == 0 ? 400 : 4;
                return (year % calc_num == 0) ? 29 : 28;
            }
            return (month <= 7 ? month : month + 1) % 2 == 1 ? 31 : 30;
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
        /// 比较时间, 返回最小项
        /// </summary>
        public static DateTime Min(this DateTime time1, DateTime time2)
        {
            return time1 < time2 ? time1 : time2;
        }
        /// <summary>
        /// 比较时间, 返回最大项
        /// </summary>
        public static DateTime Max(this DateTime time1, DateTime time2)
        {
            return time1 > time2 ? time1 : time2;
        }
    }
}
