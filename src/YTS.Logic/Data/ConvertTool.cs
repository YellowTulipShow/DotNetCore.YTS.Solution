using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlTypes;

namespace YTS.Logic.Data
{
    /// <summary>
    /// 转化工具
    /// </summary>
    public static class ConvertTool
    {
        /// <summary>
        /// 获取列表索引值
        /// </summary>
        /// <typeparam name="T">列表数据类型</typeparam>
        /// <param name="list">列表集合</param>
        /// <param name="index">索引</param>
        /// <param name="defalutValue">默认值</param>
        /// <returns>结果</returns>
        public static T GetIndexValue<T>(this IList<T> list, int index, T defalutValue = default)
        {
            if (index < 0 || index >= list.Count)
                return defalutValue;
            return list[index];
        }

        /// <summary>
        /// 计算最大值
        /// </summary>
        /// <param name="calcMethod">两个值进行比较, 返回最大值</param>
        /// <param name="value1"></param>
        /// <param name="values">值列表</param>
        /// <typeparam name="T">值数据类型</typeparam>
        /// <returns>最大值</returns>
        public static T ToMaxValue<T>(Func<T, T, T> calcMethod, T value1, params T[] values)
        {
            T max = value1;
            for (int i = 0; i < values.Length; i++)
            {
                max = calcMethod(max, values[i]);
            }
            return max;
        }

        #region === List Array String Convert ===
        /// <summary>
        /// 数组列表转字符串
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="list">需要合并的字符串数组</param>
        /// <param name="symbolSign">用于间隔内容的间隔符号</param>
        /// <param name="errorvalue">错误值</param>
        /// <returns></returns>
        private static string SourceIListToString<T>(IList list, IConvertible symbolSign, T errorvalue = default)
        {
            try
            {
                if (CheckData.IsObjectNull(list) || CheckData.IsObjectNull(symbolSign))
                    return string.Empty;
                StringBuilder strs = new StringBuilder();
                int firstSign = 0;
                bool isHavefirstValue = false;
                for (int i = firstSign; i < list.Count; i++)
                {
                    if (CheckData.IsObjectNull(list[i]) || CheckData.IsStringNull(list[i].ToString()) || list[i].Equals(errorvalue))
                    {
                        if (!isHavefirstValue)
                        {
                            firstSign = i + 1;
                        }
                        continue;
                    }
                    if (i > firstSign)
                    {
                        strs.Append(symbolSign.ToString());
                    }
                    else
                    {
                        isHavefirstValue = true;
                    }
                    strs.Append(list[i].ToString());
                }
                return strs.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 数组列表转字符串
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="list">需要合并的字符串数组</param>
        /// <param name="symbolSign">用于间隔内容的间隔符号</param>
        /// <param name="errorvalue">错误值</param>
        /// <returns></returns>
        public static string IListToString<T>(T[] list, IConvertible symbolSign, T errorvalue = default)
        {
            return SourceIListToString<T>(list, symbolSign, errorvalue);
        }
        /// <summary>
        /// 数组列表转字符串
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="list">需要合并的字符串数组</param>
        /// <param name="symbolSign">用于间隔内容的间隔符号</param>
        /// <param name="errorvalue">错误值</param>
        /// <returns></returns>
        public static string IListToString<T>(List<T> list, IConvertible symbolSign, T errorvalue = default)
        {
            return SourceIListToString<T>(list, symbolSign, errorvalue);
        }

        /// <summary>
        /// 字符串转字符串数组
        /// </summary>
        /// <param name="strValue">要转化的字符串</param>
        /// <param name="symbolSign">用于分隔的间隔符号</param>
        public static string[] ToArrayList(this string strValue, IConvertible symbolSign)
        {
            try
            {
                if (CheckData.IsStringNull(strValue) || CheckData.IsObjectNull(symbolSign))
                    throw new Exception();
                string[] strarr = strValue.Split(symbolSign.ToString().ToCharArray());
                return strarr;
            }
            catch (Exception)
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// 获取指定范围选项, 分页计算方式: 倍数计算: index * count
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="index">页面索引</param>
        /// <param name="count">每页数量</param>
        /// <returns>选项结果</returns>
        public static T[] ToRangePage<T>(IList<T> source, int index, int count)
        {
            /*
                10条 1页 开始: 1 结束: 10

                10条 2页 开始: 11 结束: 20

                10条 3页 开始: 21 结束: 30

                x条 n页 开始: (n-1)*x + 1 结束 n*x

                8条 1页 开始: 1 结束: 8

                8条 2页 开始: 9 结束: 16
             */

            count = count <= 1 ? 10 : count;
            int max_index = source.Count / count;
            int superfluous_count = source.Count % count;
            int exe_index = index < 1 ? 1 : index;
            int exe_count;
            if (exe_index == max_index + 1)
            {
                exe_count = superfluous_count;
            }
            else if (exe_index > max_index + 1)
            {
                exe_count = 0;
            }
            else
            {
                exe_count = count;
            }
            int startpoint = exe_count <= 0 ? 0 : (exe_index - 1) * count;

            List<T> list = new List<T>(source);
            return list.GetRange(startpoint, exe_count).ToArray();
        }

        /// <summary>
        /// 获取指定范围选项, 长度计算方式: 加法计算: index + length
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="index">页面索引</param>
        /// <param name="length">列表长度</param>
        /// <returns>选项结果</returns>
        public static T[] ToRangeIndex<T>(IList<T> source, int index, int length)
        {
            if (CheckData.IsSizeEmpty(source) || index >= source.Count)
            {
                return new T[] { };
            }
            if (index < 0)
            {
                index = 0;
            }
            if (length < 0)
            {
                length = 0;
            }
            int sumlen = index + length;
            if (sumlen >= source.Count)
            {
                length -= sumlen - source.Count;
            }
            List<T> list = new List<T>(source);
            return list.GetRange(index, length).ToArray();
        }
        #endregion

        #region === Type Convert ===
        /// <summary>
        /// 数组列表之间的类型数据转换
        /// </summary>
        /// <typeparam name="RT">结果返回值-数据类型</typeparam>
        /// <typeparam name="SLT">数据源-数据类型</typeparam>
        /// <typeparam name="SIT">数据源单个选项类型</typeparam>
        /// <param name="sourceslist">数据源数组</param>
        /// <param name="convertMethod">用户实现转换算法</param>
        /// <param name="isClearErrorValue">是否清除指定的错误值</param>
        /// <param name="errorValue">需要排除的错误值</param>
        private static RT[] ListConvertType<RT, SLT, SIT>(SLT sourceslist, Converter<SIT, RT> convertMethod,
            bool isClearErrorValue, RT errorValue = default) where SLT : IEnumerable
        {
            if (CheckData.IsObjectNull(sourceslist))
                return new RT[] { };
            List<RT> list = new List<RT>();
            isClearErrorValue = isClearErrorValue && !CheckData.IsObjectNull(errorValue);
            foreach (SIT item in sourceslist)
            {
                if (CheckData.IsObjectNull(item))
                    continue;
                RT value = convertMethod(item);
                if (CheckData.IsObjectNull(value))
                {
                    continue;
                }
                else if (isClearErrorValue && errorValue.Equals(value))
                {
                    continue;
                }
                list.Add(value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 'ST'类型数组 转 'RT'类型数据结果
        /// </summary>
        public static RT[] ListConvertType<RT, ST>(ST[] sourceList, Converter<ST, RT> convertMethod)
        {
            return ListConvertType(sourceList, convertMethod, false);
        }
        /// <summary>
        /// 'ST'类型数组 转 'RT'类型数据结果 排除指定的错误项
        /// </summary>
        public static RT[] ListConvertType<RT, ST>(ST[] sourceList, Converter<ST, RT> convertMethod, RT errorValue)
        {
            return ListConvertType(sourceList, convertMethod, true, errorValue: errorValue);
        }
        /// <summary>
        /// 'ST'泛型数组 转 'RT'类型数据结果
        /// </summary>
        public static RT[] ListConvertType<RT, ST>(List<ST> sourceList, Converter<ST, RT> convertMethod)
        {
            return ListConvertType(sourceList, convertMethod, false);
        }
        /// <summary>
        /// 'ST'泛型数组 转 'RT'类型数据结果 排除指定的错误项
        /// </summary>
        public static RT[] ListConvertType<RT, ST>(List<ST> sourceList, Converter<ST, RT> convertMethod, RT errorValue)
        {
            return ListConvertType(sourceList, convertMethod, true, errorValue: errorValue);
        }
        /// <summary>
        /// DataTable表 转 'RT'类型数据结果
        /// </summary>
        public static RT[] ListConvertType<RT>(DataTable sourceList, Converter<DataRow, RT> convertMethod)
        {
            return ListConvertType(sourceList.Rows, convertMethod, false);
        }
        /// <summary>
        /// DataTable表 转 'RT'类型数据结果 排除指定的错误项
        /// </summary>
        public static RT[] ListConvertType<RT>(DataTable sourceList, Converter<DataRow, RT> convertMethod, RT errorValue)
        {
            return ListConvertType(sourceList.Rows, convertMethod, true, errorValue: errorValue);
        }
        /// <summary>
        /// Dictionary字典序列 转 'RT'类型数据结果
        /// </summary>
        public static RT[] ListConvertType<RT, STKey, STValue>(Dictionary<STKey, STValue> sourceList, Converter<KeyValuePair<STKey, STValue>, RT> convertMethod)
        {
            return ListConvertType(sourceList, convertMethod, false);
        }
        /// <summary>
        /// Dictionary字典序列 转 'RT'类型数据结果 排除指定的错误项
        /// </summary>
        public static RT[] ListConvertType<RT, STKey, STValue>(Dictionary<STKey, STValue> sourceList, Converter<KeyValuePair<STKey, STValue>, RT> convertMethod, RT errorValue)
        {
            return ListConvertType(sourceList, convertMethod, true, errorValue: errorValue);
        }
        #endregion

        /// <summary>
        /// 获得 time.Year time.Month time.Day 00:00:00 点时间
        /// </summary>
        public static DateTime GetTimeZero(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
        }
        /// <summary>
        /// 获得 time.Year time.Month time.Day 23:59:59 点时间
        /// </summary>
        public static DateTime GetTimeTwoFour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
        }
        /// <summary>
        /// 输出 Enum 类型内容的 Int 值 默认为0
        /// </summary>
        public static int[] EnumInts(Enum enumobj)
        {
            List<int> ints = new List<int>();
            try
            {
                Array arr = enumobj.GetType().GetEnumValues();
                foreach (int item in arr)
                {
                    ints.Add(item);
                }
                return ints.ToArray();
            }
            catch (Exception)
            {
                return ints.ToArray();
            }
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static DateTime ObjToDateTime(object obj, DateTime defValue)
        {
            if (CheckData.IsObjectNull(obj))
                return defValue;
            string objStr = obj.ToString();
            if (CheckData.IsStringNull(objStr))
                return defValue;
            return DateTime.TryParse(objStr, out DateTime time) ? time : defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object obj, DateTime? defValue)
        {
            if (CheckData.IsObjectNull(obj))
                return defValue;
            string objStr = obj.ToString();
            if (CheckData.IsStringNull(objStr))
                return defValue;
            return DateTime.TryParse(objStr, out DateTime time) ? time : defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static SqlDateTime ObjToSqlDateTime(object obj, SqlDateTime defValue)
        {
            DateTime defTime = defValue.Value;
            DateTime resuTime = ObjToDateTime(obj, defTime);
            if (resuTime < SqlDateTime.MinValue.Value)
                return SqlDateTime.MinValue;
            else if (SqlDateTime.MaxValue.Value < resuTime)
                return SqlDateTime.MaxValue;
            return new SqlDateTime(resuTime);
        }

        /// <summary>
        /// 将对象转换为String类型, 区别在于判断是否为Null
        /// </summary>
        public static string ObjToString(object source)
        {
            return CheckData.IsObjectNull(source) ? string.Empty : source.ToString();
        }

        /// <summary>
        /// 截取 数组元素
        /// </summary>
        /// <typeparam name="T">数组的数据类型</typeparam>
        /// <param name="list">数据源</param>
        /// <param name="start_sign">开始下标标识</param>
        /// <param name="end_sign">结束下标标识, 结果不包含</param>
        /// <returns>结果</returns>
        public static T[] Interception<T>(this T[] list, int start_sign, int end_sign)
        {
            List<T> RL = new List<T>();
            if (start_sign > end_sign)
            {
                int zhong = start_sign;
                start_sign = end_sign;
                end_sign = zhong;
            }
            if (start_sign > list.Length)
            {
                start_sign = list.Length;
            }
            if (end_sign > list.Length)
            {
                end_sign = list.Length;
            }
            for (int i = start_sign; i < end_sign; i++)
            {
                try
                {
                    RL.Add(list[i]);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return RL.ToArray();
        }

        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="gb2312_str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string GB2312ToUnicode(string gb2312_str)
        {
            if (CheckData.IsStringNull(gb2312_str))
            {
                return string.Empty;
            }
            byte[] bts = Encoding.Unicode.GetBytes(gb2312_str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2)
                r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }
        /// <summary>
        /// 将Unicode编码转换为汉字字符串
        /// </summary>
        /// <param name="unicode_str">Unicode编码字符串</param>
        /// <returns>汉字字符串</returns>
        public static string UnicodeToGB2312(string unicode_str)
        {
            if (CheckData.IsStringNull(unicode_str))
            {
                return string.Empty;
            }
            string r = "";
            MatchCollection mc = Regex.Matches(unicode_str, @"\\u([\w]{2})([\w]{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            byte[] bts = new byte[2];
            foreach (Match m in mc)
            {
                bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
                bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
                r += Encoding.Unicode.GetString(bts);
            }
            return r;
        }

        /// <summary>
        /// 十六进制字符串 转 十进制Int32类型值
        /// </summary>
        /// <param name="hexadecimal_string">十六进制字符串</param>
        /// <param name="deferrorval">默认错误值</param>
        /// <returns>十进制Int32类型值</returns>
        public static int HexadecimalToDecimal(string hexadecimal_string, int deferrorval = 0)
        {
            hexadecimal_string = StringToHexadecimal(hexadecimal_string);
            if (CheckData.IsStringNull(hexadecimal_string))
            {
                return deferrorval;
            }
            return int.Parse(hexadecimal_string, NumberStyles.HexNumber);
        }
        /// <summary>
        /// 任意字符串转十六进制字符数据
        /// </summary>
        /// <param name="obj_string">任意字符串</param>
        /// <returns></returns>
        public static string StringToHexadecimal(string obj_string)
        {
            if (CheckData.IsStringNull(obj_string))
            {
                return string.Empty;
            }
            return Regex.Replace(obj_string, @"[^0-9a-fA-F]", "", RegexOptions.IgnoreCase).ToLower();
        }

        /// <summary>
        /// 十进制Int32类型值 转 十六进制字符串
        /// </summary>
        /// <param name="decimal_value">十进制Int32类型值</param>
        /// <returns>十六进制字符串</returns>
        public static string DecimalToHexadecimal(Int32 decimal_value)
        {
            return decimal_value.ToString("x");
        }

        /// <summary>
        /// 十六进制值转为Unicode格式字符串
        /// </summary>
        /// <param name="hexadecimal_string">十六进制字符串</param>
        /// <returns></returns>
        public static string Unicode_Format_String(string hexadecimal_string)
        {
            string result_str = hexadecimal_string; // string 引用地址的问题
            result_str = StringToHexadecimal(result_str);
            if (CheckData.IsStringNull(result_str))
            {
                return string.Empty;
            }
            if (result_str.Length < 4)
            {
                int cha = 4 - result_str.Length;
                for (int i = 0; i < cha; i++)
                {
                    result_str = @"0" + result_str;
                }
            }
            else if (result_str.Length > 4)
            {
                result_str = result_str.Substring(result_str.Length - 4, 4);
            }
            return string.Format("\\u{0}", result_str);
        }

        /// <summary>
        /// 获取枚举类型所有Int值
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        public static int[] EnumToInts<E>()
        {
            try
            {
                Array arr = Enum.GetValues(typeof(E));
                return arr.Cast<int>().ToArray();
            }
            catch (Exception)
            {
                return new int[] { };
            }
        }
        /// <summary>
        /// 枚举遍历所有枚举值
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        public static E[] EnumForeachArray<E>() where E : struct
        {
            try
            {
                Array arr = Enum.GetValues(typeof(E));
                return arr.Cast<E>().ToArray();
            }
            catch (Exception)
            {
                return new E[] { };
            }
        }

        /// <summary>
        /// 获取字典的第一项键名
        /// </summary>
        /// <typeparam name="K">键类型</typeparam>
        /// <typeparam name="V">值类型</typeparam>
        /// <param name="dict">数据源</param>
        /// <returns>错误值返回 K键类型 default()默认值</returns>
        public static K GetDictionaryFirstItemKey<K, V>(Dictionary<K, V> dict)
        {
            if (CheckData.IsSizeEmpty(dict))
            {
                return default;
            }
            foreach (KeyValuePair<K, V> item in dict)
            {
                return item.Key;
            }
            return default;
        }

        /// <summary>
        /// 过滤禁用的字符
        /// </summary>
        /// <param name="source">需要处理的字符串</param>
        /// <param name="disable_chars">禁用字符列表</param>
        /// <returns>结果</returns>
        public static string FilterDisableChars(string source, char[] disable_chars)
        {
            if (CheckData.IsStringNull(source))
            {
                return string.Empty;
            }
            if (CheckData.IsSizeEmpty(disable_chars))
            {
                return source;
            }
            foreach (char c in disable_chars)
            {
                source = source.Replace(c.ToString(), "");
            }
            return source;
        }


        /// <summary>
        /// 将字符串去除前后多余空格
        /// </summary>
        public static string ToTrim(string sv)
        {
            return ToString(sv).Trim();
        }

        /// <summary>
        /// 将 不确定类型数据 转换为 指定数据类型值
        /// </summary>
        /// <param name="type">指定数据类型</param>
        /// <param name="ov">不确定类型数据</param>
        /// <returns>确定类型数据</returns>
        public static object ToObject(Type type, object ov)
        {
            if (CheckData.IsObjectNull(type))
            {
                return ov;
            }
            if (CheckData.IsObjectNull(ov))
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            if (CheckData.IsTypeEqualDepth(type, typeof(int), true))
            {
                return ToInt(ov, default);
            }
            if (CheckData.IsTypeEqualDepth(type, typeof(Enum), true))
            {
                return ov.GetType().IsEnum ? (int)ov : ToInt(ov, default);
            }
            if (CheckData.IsTypeEqualDepth(type, typeof(float), true))
            {
                return ToFloat(ov, default);
            }
            if (CheckData.IsTypeEqualDepth(type, typeof(double), true))
            {
                return ToDouble(ov, default);
            }
            if (CheckData.IsTypeEqualDepth(type, typeof(DateTime), true))
            {
                Convert.ToDateTime(ov);
                return ToDateTime(ov, default);
            }
            if (CheckData.IsTypeEqualDepth(type, typeof(bool), true))
            {
                return ToBool(ov, default);
            }
            if (CheckData.IsTypeEqualDepth(type, typeof(Type), true))
            {
                string typename = ToString(ov);
                return ToType(typename);
            }
            return ov;
        }

        /// <summary>
        /// 将对象转换为String类型
        /// 特定的数据类型将进行格式化处理
        /// 如为空, 则返回 string.Empty
        /// </summary>
        /// <param name="ov">通用object对象数据</param>
        /// <returns>格式化/ToString得到的string结果</returns>
        public static string ToString(object ov)
        {
            if (CheckData.IsObjectNull(ov))
            {
                return string.Empty;
            }
            Type vt = ov.GetType();
            if (CheckData.IsTypeEqualDepth(vt, typeof(string), true))
            {
                return ov.ToString();
            }
            if (CheckData.IsTypeEqualDepth(vt, typeof(DateTime), true))
            {
                return ToString((DateTime)ov);
            }
            if (vt.IsEnum || CheckData.IsTypeEqualDepth(vt, typeof(Enum), true))
            {
                return ((int)ov).ToString();
            }
            if (CheckData.IsTypeEqualDepth(vt, typeof(Type), true))
            {
                return ((Type)ov).FullName;
            }
            return ov.ToString();
        }
        /// <summary>
        /// 转为程序可用不报错字符串
        /// </summary>
        /// <param name="sv">字符串类型值</param>
        /// <returns>可用不报错字符串</returns>
        public static string ToString(string sv)
        {
            return CheckData.IsStringNull(sv) ? string.Empty : sv.ToString();
        }
        /// <summary>
        /// 数组列表转字符串
        /// </summary>
        /// <param name="list">需要合并的字符串数组</param>
        /// <param name="symbolSign">用于间隔内容的间隔符号</param>
        public static string ToString<T>(IList<T> list, IConvertible symbolSign)
        {
            try
            {
                if (CheckData.IsObjectNull(list) || CheckData.IsObjectNull(symbolSign))
                    return string.Empty;
                StringBuilder strs = new StringBuilder();
                int firstSign = 0;
                bool isHavefirstValue = false;
                for (int i = firstSign; i < list.Count; i++)
                {
                    if (CheckData.IsObjectNull(list[i]) || CheckData.IsStringNull(list[i].ToString()))
                    {
                        if (!isHavefirstValue)
                        {
                            firstSign = i + 1;
                        }
                        continue;
                    }
                    if (i > firstSign)
                    {
                        strs.Append(symbolSign.ToString());
                    }
                    else
                    {
                        isHavefirstValue = true;
                    }
                    strs.Append(list[i].ToString());
                }
                return strs.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 数字转字符串
        /// </summary>
        public static string ToString(int iv)
        {
            return iv.ToString();
        }

        /// <summary>
        /// 根据类型名称查找类型
        /// </summary>
        /// <param name="typename">类型名称</param>
        /// <returns>类型</returns>
        public static Type ToType(string typename)
        {
            if (CheckData.IsStringNull(typename))
            {
                return null;
            }
            Type rtype = Type.GetType(typename);
            try
            {
                return CheckData.IsObjectNull(rtype) ? null : rtype;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(object expression, int defValue)
        {
            if (!CheckData.IsObjectNull(expression))
                return ToInt(expression.ToString(), defValue);
            return defValue;
        }
        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            if (int.TryParse(expression, out int rv))
                return rv;

            return Convert.ToInt32(ToFloat(expression, defValue));
        }
        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int? ToInt(string expression, int? defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;
            if (int.TryParse(expression, out int rv))
                return rv;
            return defValue;
        }


        /// <summary>
        /// 将对象转换为 Long (int64)类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的 Long 类型结果</returns>
        public static long ToLong(object expression, int defValue)
        {
            if (!CheckData.IsObjectNull(expression))
            {
                return ToLong(expression.ToString(), defValue);
            }
            return defValue;
        }
        /// <summary>
        /// 将字符串转换为 Long (int64)类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的 Long 类型结果</returns>
        public static long ToLong(string expression, int defValue)
        {
            if (CheckData.IsStringNull(expression))
            {
                return defValue;
            }
            return long.TryParse(expression, out long rv) ? rv : defValue;
        }

        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return ToDecimal(expression.ToString(), defValue);

            return defValue;
        }
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ToDecimal(string expression, decimal defValue)
        {
            if (decimal.TryParse(expression, out decimal value))
            {
                return value;
            }
            return defValue;
        }
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal? ToDecimal(string expression, decimal? defValue)
        {
            if (decimal.TryParse(expression, out decimal value))
                return value;
            return defValue;
        }

        /// <summary>
        /// 将小数值按指定的小数位数截断
        /// </summary>
        /// <param name="d">要截断的小数</param>
        /// <param name="s">小数位数，s大于等于0，小于等于28</param>
        /// <returns></returns>
        public static decimal ToFixed(this decimal d, int s)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, s));
            if (d < 0)
            {
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            }
            else
            {
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
            }
        }

        /// <summary>
        /// 将小数值按指定的小数位数截断
        /// </summary>
        /// <param name="d">要截断的小数</param>
        /// <param name="s">小数位数，s大于等于0，小于等于28</param>
        /// <returns></returns>
        public static decimal ToFixed(this decimal? d, int s)
        {
            return (d ?? 0).ToFixed(s);
        }

        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的float类型结果</returns>
        public static float ToFloat(object expression, float defValue)
        {
            if (expression != null)
                return ToFloat(expression.ToString(), defValue);

            return defValue;
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的float类型结果</returns>
        public static float ToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }


        /// <summary>
        /// Object型转换为double型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defvalue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double ToDouble(object expression, double defvalue)
        {
            string s = ToString(expression);
            return ToDouble(s, defvalue);
        }
        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defvalue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double ToDouble(string expression, double defvalue)
        {
            if (CheckData.IsStringNull(expression))
            {
                return defvalue;
            }
            return double.TryParse(expression, out double v) ? v : defvalue;
        }


        /// <summary>
        /// object型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBool(object expression, bool defValue)
        {
            if (expression != null)
                return ToBool(expression.ToString(), defValue);
            return defValue;
        }
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBool(String strValue, bool defValue)
        {
            if (strValue != null)
            {
                if (String.Compare(strValue, "true", true) == 0)
                    return true;
                else if (String.Compare(strValue, "false", true) == 0)
                    return false;
            }
            return defValue;
        }

        /// <summary>
        /// 时间格式转为字符串
        /// </summary>
        /// <param name="dateTime">需要转换的时间</param>
        /// <returns>时间格式: yyyy-MM-dd HH:mm:ss</returns>
        public static string ToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获得 日期 00:00:00 点时间
        /// </summary>
        public static DateTime GetDateZeroHour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
        }
        /// <summary>
        /// 获得 日期 23:59:59 点时间
        /// </summary>
        public static DateTime GetDateTwoFourHour(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, DateTime defValue)
        {
            if (CheckData.IsObjectNull(obj))
            {
                return defValue;
            }
            if (CheckData.IsTypeEqual(obj.GetType(), typeof(DateTime)))
            {
                return (DateTime)obj;
            }
            string s = obj.ToString();
            if (CheckData.IsStringNull(s))
            {
                return defValue;
            }
            return DateTime.TryParse(s, out DateTime time) ? time : defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static SqlDateTime ToSqlDateTime(object obj, SqlDateTime defValue)
        {
            DateTime defTime = defValue.Value;
            DateTime resuTime = ToDateTime(obj, defTime);
            if (resuTime < SqlDateTime.MinValue.Value)
                return SqlDateTime.MinValue;
            else if (SqlDateTime.MaxValue.Value < resuTime)
                return SqlDateTime.MaxValue;
            return new SqlDateTime(resuTime);
        }

        /// <summary>
        /// 查找序列索引的值
        /// </summary>
        /// <typeparam name="T">泛型序列的数据类型</typeparam>
        /// <param name="list">泛型序列对象</param>
        /// <param name="index">索引标识</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>如果序列为空或索引超出范围返回默认值</returns>
        public static T FindIndex<T>(this IList<T> list, int index, T defaultValue)
        {
            if (list == null)
            {
                return defaultValue;
            }
            if (index >= list.Count)
            {
                return defaultValue;
            }
            return list[index];
        }

        /// <summary>
        /// Null或空白字符或空字符串判断返回默认值
        /// </summary>
        /// <param name="value">需要判断的字符串值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>结果</returns>
        public static string NullOrEmptyDefault(this string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return value;
        }

        /// <summary>
        /// 将父类的值赋值到子类中
        /// </summary>
        /// <typeparam name="TParent">父类</typeparam>
        /// <typeparam name="TChild">子类</typeparam>
        /// <param name="parent">父类内容</param>
        /// <returns>子类结果</returns>
        public static TChild AutoCopy<TParent, TChild>(this TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var ParentType = typeof(TParent);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }
            var Fields = ParentType.GetFields();
            foreach (var Field in Fields)
            {
                //循环遍历字段
                if (!Field.IsInitOnly && !Field.IsStatic)
                {
                    //进行字段拷贝
                    Field.SetValue(child, Field.GetValue(parent));
                }
            }
            return child;
        }

        /// <summary>
        /// 计算同比比例
        /// </summary>
        /// <param name="Current">当前数据</param>
        /// <param name="Previous">过去数据</param>
        /// <returns>比例(%)</returns>
        public static decimal CalcScale(this decimal Current, decimal Previous)
        {
            if (Previous == 0)
                return 0;
            return ((Current - Previous) / Previous * 100).ToFixed(2);
        }
    }
}
