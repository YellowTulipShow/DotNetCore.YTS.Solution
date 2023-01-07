using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace YTS.Logic.Data
{
    /// <summary>
    /// 随机帮助类
    /// </summary>
    public static class RandomData
    {
        /// <summary>
        /// 创建一个全局的随机数生成器 可以确保在调用生成随机数时, 非重复!
        /// </summary>
        public static readonly Random R = new Random();

        /// <summary>
        /// 随机获取布尔值
        /// </summary>
        public static bool GetBoolean() {
            return GetItem(new bool[] { true, false });
        }

        /// <summary>
        /// 随机选取其中一个选项
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>结果选项, 数据源为空返回:数据类型默认值</returns>
        public static T GetItem<T>(T[] source)
        {
            if (CheckData.IsSizeEmpty(source)) {
                return default;
            }
            return source[R.Next(0, source.Length)];
        }

        /// <summary>
        /// 随机选取其中一个选项
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>结果选项, 数据源为空返回:数据类型默认值</returns>
        public static T GetItem<T>(IList<T> source)
        {
            if (CheckData.IsSizeEmpty(source))
            {
                return default;
            }
            return source[R.Next(0, source.Count)];
        }

        /// <summary>
        /// 生成指定数量的列表数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="count">数量</param>
        /// <param name="generateFunc">生成方法</param>
        /// <returns>列表数据</returns>
        public static IList<T> GenerateSpecifiedQuantityList<T>(int count, Func<T> generateFunc)
        {
            IList<T> list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(generateFunc());
            }
            return list;
        }

        /// <summary>
        /// 生成指定数量的列表数据
        /// </summary>
        /// <typeparam name="TSource">来源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="sourceList">数据来源</param>
        /// <param name="generateFunc">生成方法</param>
        /// <returns>列表数据</returns>
        public static IList<TTarget> GenerateNotRepeatList<TSource, TTarget>(IList<TSource> sourceList, Func<TSource, TTarget> generateFunc)
        {
            int source_length = sourceList.Count;
            int result_length = GetInt(1, source_length + 1);
            if (result_length == source_length)
                return sourceList.Select(generateFunc).ToList();
            IList<TTarget> rlist = new List<TTarget>();
            HashSet<int> index_hash = new HashSet<int>();
            while (result_length > 0)
            {
                int index = GetInt(0, source_length);
                if (index_hash.Contains(index))
                    continue;
                TSource s = sourceList[index];
                TTarget t = generateFunc.Invoke(s);
                rlist.Add(t);
                result_length--;
            }
            return rlist;
        }

        /// <summary>
        /// 拼接随机字符串
        /// </summary>
        /// <param name="source">指定字符进行拼接</param>
        /// <param name="max_charlength">指定字符个数</param>
        /// <returns>拼接结果</returns>
        public static string GetString(char[] source, int max_charlength) {
            if (CheckData.IsSizeEmpty(source)) {
                return string.Empty;
            }
            StringBuilder strbu = new StringBuilder();
            for (int i = 0; i < max_charlength; i++) {
                strbu.Append(source[R.Next(0, source.Length)]);
            }
            return strbu.ToString();
        }
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="max_charlength">指定字符个数, 默认32个</param>
        /// <returns>拼接结果</returns>
        public static string GetString(int max_charlength = 32) {
            return GetString(CommonData.ASCII_ALL(), max_charlength);
        }
        /// <summary>
        /// 拼接随机字符串
        /// </summary>
        /// <param name="source">指定字符进行拼接</param>
        /// <returns>拼接结果</returns>
        public static string GetString(char[] source) {
            return GetString(source, source.Length);
        }

        /// <summary>
        /// 随机获取日期, 指定时间范围区间
        /// </summary>
        public static DateTime GetDateTime(DateTime min, DateTime max) {
            if (min > max) {
                DateTime zhong = min;
                min = max;
                max = zhong;
            }
            int upstatue = 0;
            int r_Year = TimeRangeSelect(ref upstatue, 1, 9999 + 1, min.Year, max.Year);
            int r_Month = TimeRangeSelect(ref upstatue, 1, 12 + 1, min.Month, max.Month);
            int r_Day = TimeRangeSelect(ref upstatue, 1, CommonData.GetMaxDayCount(r_Year, r_Month) + 1, min.Day, max.Day);
            int r_Hour = TimeRangeSelect(ref upstatue, 0, 23 + 1, min.Hour, max.Hour);
            int r_Minute = TimeRangeSelect(ref upstatue, 0, 59 + 1, min.Minute, max.Minute);
            int r_Second = TimeRangeSelect(ref upstatue, 0, 59 + 1, min.Second, max.Second);
            int r_Millisecond = TimeRangeSelect(ref upstatue, 0, 999 + 1, min.Millisecond, max.Millisecond);
            return new DateTime(r_Year, r_Month, r_Day, r_Hour, r_Minute, r_Second, r_Millisecond);
        }
        private static int TimeRangeSelect(ref int upstatue, int min, int max, int start, int end) {
            if (upstatue == 4) {
                return R.Next(min, max);
            }

            int minvalue = (upstatue == 3) ? min : start;
            int maxvalue = (upstatue == 2) ? max - 1 : end;
            if (minvalue > maxvalue) {
                int zhong = minvalue;
                minvalue = maxvalue;
                maxvalue = zhong;
            }
            int result = R.Next(minvalue, maxvalue + 1);

            int selfstatus = 0;
            if (minvalue == result && result == maxvalue) {
                selfstatus = 1;
            }
            if (minvalue == result && result < maxvalue) {
                selfstatus = (upstatue == 3) ? 4 : 2;
            }
            if (minvalue < result && result == maxvalue) {
                selfstatus = (upstatue == 2) ? 4 : 3;
            }
            if (minvalue < result && result < maxvalue) {
                selfstatus = 4;
            }
            upstatue = (selfstatus < upstatue) ? upstatue : selfstatus;
            return result;
        }
        /// <summary>
        /// 随机获取日期
        /// </summary>
        public static DateTime GetDateTime() {
            return GetDateTime(DateTime.MinValue, DateTime.MaxValue);
        }
        /// <summary>
        /// 随机获取日期, 指定最大时间区间
        /// </summary>
        public static DateTime GetDateTime(DateTime maxtime) {
            return GetDateTime(DateTime.MinValue, maxtime);
        }

        /// <summary>
        /// 随机获取 Int 值
        /// </summary>
        /// <param name="minval">最小值, 默认为 [int.MinValue + 1]</param>
        /// <param name="maxval">最大值, 默认为 [int.MaxValue] 计算时不取其值</param>
        /// <returns></returns>
        public static int GetInt(int minval = int.MinValue + 1, int maxval = int.MaxValue) {
            if (minval > maxval) {
                int zhong = minval;
                minval = maxval;
                maxval = zhong;
            }
            return R.Next(minval, maxval);
        }

        /// <summary>
        /// 随机获取 double 值
        /// </summary>
        public static double GetDouble(int multiplication_number = 1000) {
            return GetDouble(multiplication_number, GetBoolean());
        }

        /// <summary>
        /// 随机获取 double 值, 范围在 (0.0~0.1) * multiplication_number 之间, 
        /// </summary>
        /// <param name="multiplication_number">倍数</param>
        /// <param name="IsNegativeNumber">是否为负数</param>
        /// <returns></returns>
        public static double GetDouble(int multiplication_number, bool IsNegativeNumber)
        {
            double result = R.NextDouble() * multiplication_number;
            if (IsNegativeNumber)
            {
                result *= -1;
            }
            return result;
        }

        /// <summary>
        /// 随机获取 double 值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static double GetDouble(double min, double max) {
            if (min > max)
            {
                double zhong = max;
                min = max;
                max = zhong;
            }
            return R.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// 随机获取 double 值
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static decimal GetDecimal(decimal min, decimal max)
        {
            return (decimal)GetDouble((double)min, (double)max);
        }

        /// <summary>
        /// 获取中文随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        public static string GetChineseString(int length = 5) {
            StringBuilder result_str = new StringBuilder();
            for (int i = 0; i < length; i++) {
                int hexa_int_min_sign = CommonData.Unicode_Chinese_MIN_Decimal();
                int hexa_int_max_sign = CommonData.Unicode_Chinese_MAX_Decimal();
                int random_value = R.Next(hexa_int_min_sign, hexa_int_max_sign + 1);
                string random_heax_string = ConvertTool.DecimalToHexadecimal(random_value);
                string unicode_format_str = ConvertTool.Unicode_Format_String(random_heax_string);
                string chinese_char = ConvertTool.UnicodeToGB2312(unicode_format_str);
                result_str.Append(chinese_char);
            }
            return result_str.ToString();
        }

        #region ====== Color: ======
        /// <summary>
        /// RGB颜色 三项数字值0-255
        /// </summary>
        public static int[] RGBColor_NumberList() {
            const int min = 0;
            const int max = 255;
            return new int[3] {
                GetInt(min, max + 1),
                GetInt(min, max + 1),
                GetInt(min, max + 1),
            };
        }
        /// <summary>
        /// RGB颜色 六位字符串
        /// </summary>
        /// <param name="isNeedAdd_Hashtag">是否需要加 '#' 号</param>
        public static string RGBColor_SixDigitString(bool isNeedAdd_Hashtag = true) {
            char[] clist = CommonData.ASCII_Hexadecimal();
            StringBuilder str = new StringBuilder();
            if (isNeedAdd_Hashtag) {
                str.Append('#');
            }
            for (int i = 0; i < 6; i++) {
                str.Append(GetItem(clist));
            }
            return str.ToString();
        }
        #endregion
    }
}
