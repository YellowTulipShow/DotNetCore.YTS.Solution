using System;

namespace YTS.Tools
{
    /// <summary>
    /// 位枚举
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 字符串类型 '枚举名称' or '枚举值' 转为枚举对象
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="value">字符串'值'</param>
        /// <param name="defaultValue">默认返回的枚举对象</param>
        /// <returns>返回: 枚举对象</returns>
        public static E ToEnum<E>(this string value, E defaultValue) where E : Enum
        {
            try
            {
                Type type = typeof(E);
                bool isHaveZero = false;
                foreach (int item in Enum.GetValues(type))
                    if (item == 0)
                        isHaveZero = true;
                if (!isHaveZero && value == "0")
                    return defaultValue;
                return (E)Enum.Parse(type, value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 数值类型 '枚举值' 转为枚举对象
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="value">数字'值'</param>
        /// <param name="defaultValue">默认返回的枚举对象</param>
        /// <returns>返回: 枚举对象</returns>
        public static E ToEnum<E>(this int value, E defaultValue) where E : Enum
        {
            return ToEnum(value.ToString(), defaultValue);
        }
    }
}
