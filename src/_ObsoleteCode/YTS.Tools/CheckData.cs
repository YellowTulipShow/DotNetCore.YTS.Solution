using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace YTS.Tools
{
    /// <summary>
    /// 检查数据 Is: True为符合条件 False不匹配条件
    /// </summary>
    public static class CheckData
    {
        /// <summary>
        /// Object 对象 是否为空 无值
        /// </summary>
        public static bool IsObjectNull(object obj)
        {
            return (Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value));
        }
        /// <summary>
        /// String 对象 是否为空 无值
        /// </summary>
        public static bool IsStringNull(string str)
        {
            return IsObjectNull(str) || string.Equals(str, String.Empty) || string.Equals(str, "") || str.Length <= 0;
        }

        #region  === Is Size Empty ===
        /// <summary>
        /// 判断是否: IList.T 泛型集合 大小 为 '空'
        /// </summary>
        public static bool IsSizeEmpty<T>(this IList<T> list)
        {
            return IsObjectNull(list) || list.Count <= 0;
        }
        /// <summary>
        /// 判断是否: DataSet 数据集 大小 为 '空'
        /// </summary>
        public static bool IsSizeEmpty(this DataSet ds)
        {
            return IsObjectNull(ds) || ds.Tables.Count <= 0;
        }
        /// <summary>
        /// 判断是否: DataTable 数据表 大小 为 '空'
        /// </summary>
        public static bool IsSizeEmpty(this DataTable dt)
        {
            return IsObjectNull(dt) || dt.Rows.Count <= 0;
        }
        /// <summary>
        /// 判断是否: DataRow 数据行 大小 为 '空'
        /// </summary>
        public static bool IsSizeEmpty(this DataRow row)
        {
            return IsObjectNull(row) || row.Table.Rows.Count <= 0;
        }
        /// <summary>
        /// 判断是否: Dictionary 数据行 大小 为 '空'
        /// </summary>
        public static bool IsSizeEmpty<K, V>(this Dictionary<K, V> dic)
        {
            return IsObjectNull(dic) || dic.Count <= 0;
        }
        #endregion

        #region DTcms Utils.cs Code
        /// <summary>
        /// 是否错误的SQL时间
        /// </summary>
        public static bool IsErrorSQLDateTime(this DateTime date)
        {
            return date < SqlDateTime.MinValue.Value || SqlDateTime.MaxValue.Value < date;
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 截取字符串长度，超出部分使用后缀suffix代替，比如abcdevfddd取前3位，后面使用...代替
        /// </summary>
        /// <param name="orginStr"></param>
        /// <param name="length"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string SubStrAddSuffix(string orginStr, int length, string suffix)
        {
            string ret = orginStr;
            if (orginStr.Length > length)
            {
                ret = orginStr.Substring(0, length) + suffix;
            }
            return ret;
        }
        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }
        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// 判断对象是否可以转成int型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNumber(object o)
        {
            int tmpInt;
            if (o == null)
            {
                return false;
            }
            if (o.ToString().Trim().Length == 0)
            {
                return false;
            }
            if (!int.TryParse(o.ToString(), out tmpInt))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region 检测是否有Sql危险字符
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary>
        /// 检查过滤设定的危险字符
        /// </summary>
        /// <param name="InText">要过滤的字符串 </param>
        /// <returns>如果参数存在不安全字符，则返回true </returns>
        public static bool SqlFilter(string word, string InText)
        {
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Str_InputFilter(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 检查字符串
        #endregion
        #endregion

        #region === IsType ===
        /// <summary>
        /// 比较两个类型是否是一样的
        /// </summary>
        /// <param name="t1">类型: 1</param>
        /// <param name="t2">类型: 2</param>
        /// <returns>是否相同</returns>
        public static bool IsTypeEqual(Type t1, Type t2)
        {
            if (CheckData.IsObjectNull(t1) && CheckData.IsObjectNull(t2))
            {
                // 都为空
                return true;
            }
            if (CheckData.IsObjectNull(t1) && !CheckData.IsObjectNull(t2))
            {
                // t1为空 t2不为空
                return false;
            }
            if (!CheckData.IsObjectNull(t1) && CheckData.IsObjectNull(t2))
            {
                // t1不为空 t2为空
                return false;
            }
            // 都不为空
            return t1.Equals(t2) && t1.FullName == t2.FullName;
        }

        /// <summary>
        /// 比较两个类型是否是一样的 深入查询类型的继承链 (递归)
        /// </summary>
        /// <param name="depth_find_type">需要递归查询的类型</param>
        /// <param name="type">用于比较的类型</param>
        /// <param name="is_depth">是否深入查询</param>
        /// <returns>是否相同</returns>
        public static bool IsTypeEqualDepth(Type depth_find_type, Type type, bool is_depth)
        {
            if (IsTypeEqual(typeof(object), type))
            {
                return true;
            }
            if (!is_depth)
            {
                return IsTypeEqual(depth_find_type, type);
            }
            if (IsTypeEqual(depth_find_type, type))
            {
                return true;
            }
            if (CheckData.IsObjectNull(depth_find_type) ||
                CheckData.IsObjectNull(type))
            {
                return false;
            }
            return IsTypeEqualDepth(depth_find_type.BaseType, type, true);
        }
        #endregion
    }
}
