using System;
using System.Collections.Generic;
using System.Reflection;

namespace YTS.Tools
{
    /// <summary>
    /// 解析枚举表示其指标值数据模型
    /// </summary>
    public class EnumInfo : AbsBasicDataModel
    {
        /// <summary>
        /// 默认 Int 类型的值
        /// </summary>
        public const int DEFAULT_INT_VALUE = 0;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Int 类型标识值
        /// </summary>
        public int IntValue { get; set; } = DEFAULT_INT_VALUE;

        /// <summary>
        /// 注释值
        /// </summary>
        public string Explain { get; set; } = string.Empty;

        /// <summary>
        /// 解析一种枚举的所有选项
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <returns>解析结果</returns>
        public static EnumInfo[] AnalysisList<E>()
        {
            Type type = typeof(E);
            return AnalysisList(type);
        }

        /// <summary>
        /// 解析一种枚举的所有选项
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>解析结果</returns>
        public static EnumInfo[] AnalysisList(Type type)
        {
            if (!type.IsEnum)
            {
                return new EnumInfo[] { };
            }
            List<EnumInfo> list = new List<EnumInfo>();
            foreach (int ival in Enum.GetValues(type))
            {
                string name = Enum.GetName(type, ival);
                FieldInfo info = type.GetField(name);
                ExplainAttribute exp = ExplainAttribute.Extract(info);
                list.Add(new EnumInfo()
                {
                    Name = name,
                    IntValue = ival,
                    Explain = exp.Text,
                });
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取所有选项
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <returns>枚举类型的所有选项</returns>
        public static E[] GetALLItem<E>() where E : struct
        {
            Type type = typeof(E);
            if (!type.IsEnum)
            {
                return new E[] { };
            }
            List<E> list = new List<E>();
            foreach (int ival in Enum.GetValues(type))
            {
                string name = Enum.GetName(type, ival);
                E v;
                if (Enum.TryParse<E>(name, out v))
                {
                    list.Add(v);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 解析一种枚举的一个选项
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="item">需要解析的选项</param>
        /// <returns>解析结果</returns>
        public static EnumInfo AnalysisItem<E>(E item)
        {
            Type type = typeof(E);
            if (!type.IsEnum)
            {
                return null;
            }
            string item_name = item.ToString();
            foreach (int ival in Enum.GetValues(type))
            {
                string name = Enum.GetName(type, ival);
                if (item_name != name)
                {
                    continue;
                }
                FieldInfo info = type.GetField(name);
                ExplainAttribute exp = ExplainAttribute.Extract(info);
                return new EnumInfo()
                {
                    Name = name,
                    IntValue = ival,
                    Explain = exp.Text,
                };
            }
            return null;
        }

        /// <summary>
        /// 是否存在这种名称的枚举选项
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <param name="keyname">枚举名称</param>
        /// <returns>是否存在</returns>
        public static bool IsContains<E>(string keyname)
        {
            if (!typeof(E).IsEnum || CheckData.IsStringNull(keyname))
            {
                return false;
            }
            EnumInfo[] array = AnalysisList<E>();
            foreach (EnumInfo info in array)
            {
                if (info.Name == keyname)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 输出为键值对集合 <枚举名称, 枚举信息>
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <returns></returns>
        public static Dictionary<string, EnumInfo> ToDictionary<E>()
        {
            EnumInfo[] array = AnalysisList<E>();
            Dictionary<string, EnumInfo> dic = new Dictionary<string, EnumInfo>();
            for (int i = 0; i < array.Length; i++)
            {
                dic.Add(array[i].Name, array[i]);
            }
            return dic;
        }
    }
}
