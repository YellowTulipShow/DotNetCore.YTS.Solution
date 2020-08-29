using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    /// <summary>
    /// 系统字典表静态方法
    /// </summary>
    public static class SystemSetTypeStaticFunction
    {
        public static EnumInfo[] GetEnumInfos<E>() where E : Enum
        {
            return EnumInfo.AnalysisList<E>();
        }
        public static ExplainAttribute GetExplain<E>() where E : Enum
        {
            Type enumType = typeof(E);
            return ExplainAttribute.Extract(enumType);
        }
        public static string GetKey<E>() where E : Enum
        {
            Type enumType = typeof(E);
            return enumType.FullName.Replace('+', '.');
        }
        public static string GetBelowKey(string ParentKey, string BelowName)
        {
            return string.Join('.', new string[] { ParentKey, BelowName });
        }

        public static string QueryEnumValue<E>(this IQueryable<SystemSetType> list, int? value) where E : Enum
        {
            var ParentKey = GetKey<E>();
            return list
                .Where(a =>
                    list.Any(b => b.ID == a.ParentID && b.Key == ParentKey) &&
                    a.Value == (value ?? 0).ToString())
                .Select(a => a.Explain).FirstOrDefault();
        }

        public static IQueryable<SystemSetType> QueryEnumList<E>(this IQueryable<SystemSetType> list) where E : Enum
        {
            var ParentKey = GetKey<E>();
            return list.Where(a => list.Any(b => b.ID == a.ParentID && b.Key == ParentKey));
        }
    }
}
