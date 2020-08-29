using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

/// <summary>
/// 自定义属性扩展
/// 代码来源: https://www.cnblogs.com/junjieok/p/4949806.html
/// </summary>
public static class CustomAttributeExtensions
{
    /// <summary>
    /// Cache Data
    /// </summary>
    private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();

    /// <summary>
    /// 获取CustomAttribute Value
    /// </summary>
    /// <typeparam name="TAttribute">Attribute的子类型</typeparam>
    /// <typeparam name="TReturn">TReturn的子类型</typeparam>
    /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
    /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
    /// <returns>返回Attribute的值，没有则返回null</returns>
    public static TReturn GetCustomAttributeValue<TAttribute, TReturn>(this Type sourceType, Func<TAttribute, TReturn> attributeValueAction)
        where TAttribute : Attribute
    {
        return _getAttributeValue(sourceType, attributeValueAction, null);
    }

    /// <summary>
    /// 获取枚举项的特性值Attribute Value
    /// </summary>
    /// <typeparam name="E">枚举类型</typeparam>
    /// <typeparam name="A">特性类型</typeparam>
    /// <typeparam name="R">结果类型</typeparam>
    /// <param name="vEnum">枚举对象</param>
    /// <param name="toResultFunc">将特性转为结果的自定义方法</param>
    /// <returns>枚举附带的特性转化结果, 如果中途无法找到则返回默认结果类型值, 多个相同的特性则只返回第一个</returns>
    public static R GetEnumAttributeValue<E, A, R>(this E vEnum, Func<A, R> toResultFunc) where E : struct where A : Attribute
    {
        A[] attrArr = GetEnumAttributeList<E, A>(vEnum);
        if (attrArr == null || attrArr.Length <= 0)
        {
            return default(R);
        }
        return toResultFunc(attrArr[0]);
    }
    /// <summary>
    /// 获取枚举项的特性值Attribute
    /// </summary>
    /// <typeparam name="E">枚举类型</typeparam>
    /// <typeparam name="A">特性类型</typeparam>
    /// <param name="vEnum">枚举对象</param>
    /// <returns>枚举附带的特性列表</returns>
    public static A[] GetEnumAttributeList<E, A>(this E vEnum) where E : struct where A : Attribute
    {
        Type type = typeof(E);
        if (!type.IsEnum)
        {
            return new A[] { };
        }
        FieldInfo memberInfo = type.GetField(vEnum.ToString());
        object[] attrs = memberInfo.GetCustomAttributes(typeof(A), false);
        if (attrs == null || attrs.Length <= 0)
        {
            return new A[] { };
        }
        A[] attrArr = attrs
            .Select(o => o is A ? o as A : null)
            .Where(a => a != null)
            .ToArray();
        return attrArr;
    }
    /// <summary>
    /// 获取枚举项的特性值Attribute Value
    /// </summary>
    /// <typeparam name="E">枚举类型</typeparam>
    /// <typeparam name="A">特性类型</typeparam>
    /// <typeparam name="R">结果类型</typeparam>
    /// <param name="vEnum">枚举对象</param>
    /// <param name="toResultFunc">将特性转为结果的自定义方法</param>
    /// <returns>枚举附带的特性转化结果, 如果中途无法找到则返回默认结果类型值</returns>
    public static R[] GetEnumAttributeValueList<E, A, R>(this E vEnum, Func<A, R> toResultFunc) where E : struct where A : Attribute
    {
        A[] attrArr = GetEnumAttributeList<E, A>(vEnum);
        if (attrArr == null || attrArr.Length <= 0)
        {
            return new R[] { };
        }
        return attrArr.Select(toResultFunc).ToArray();
    }

    /// <summary>
    /// 获取CustomAttribute Value
    /// </summary>
    /// <typeparam name="TAttribute">Attribute的子类型</typeparam>
    /// <typeparam name="TReturn">TReturn的子类型</typeparam>
    /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
    /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
    /// <param name="propertyName">field name或property name</param>
    /// <returns>返回Attribute的值，没有则返回null</returns>
    public static TReturn GetCustomAttributeValue<TAttribute, TReturn>(this Type sourceType, Func<TAttribute, TReturn> attributeValueAction, string propertyName)
        where TAttribute : Attribute
    {
        return _getAttributeValue(sourceType, attributeValueAction, propertyName);
    }

    #region private methods

    private static TReturn _getAttributeValue<TAttribute, TReturn>(Type sourceType, Func<TAttribute, TReturn> attributeFunc, string propertyName)
        where TAttribute : Attribute
    {
        var cacheKey = BuildKey<TAttribute>(sourceType, propertyName);
        var value = Cache.GetOrAdd(cacheKey, k => GetValue(sourceType, attributeFunc, propertyName));
        if (value is TReturn) return (TReturn)Cache[cacheKey];
        return default(TReturn);
    }

    private static string BuildKey<TAttribute>(Type type, string propertyName) where TAttribute : Attribute
    {
        var attributeName = typeof(TAttribute).FullName;
        if (string.IsNullOrEmpty(propertyName))
        {
            return type.FullName + "." + attributeName;
        }

        return type.FullName + "." + propertyName + "." + attributeName;
    }

    private static TReturn GetValue<TAttribute, TReturn>(this Type type, Func<TAttribute, TReturn> attributeValueAction, string name)
        where TAttribute : Attribute
    {
        TAttribute attribute = default(TAttribute);
        if (string.IsNullOrEmpty(name))
        {
            attribute = type.GetCustomAttribute<TAttribute>(false);
        }
        else
        {
            var propertyInfo = type.GetProperty(name);
            if (propertyInfo != null)
            {
                attribute = propertyInfo.GetCustomAttribute<TAttribute>(false);
            }
            else
            {
                var fieldInfo = type.GetField(name);
                if (fieldInfo != null)
                {
                    attribute = fieldInfo.GetCustomAttribute<TAttribute>(false);
                }
            }
        }

        return attribute == null ? default(TReturn) : attributeValueAction(attribute);
    }

    #endregion
}
