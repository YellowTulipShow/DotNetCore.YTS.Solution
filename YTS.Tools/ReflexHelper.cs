using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace YTS.Tools
{
    /// <summary>
    /// 反射帮助操作类
    /// </summary>
    public static class ReflexHelper
    {
        /// <summary>
        /// 获取指定 "内容" 名称 用法: ***.Name(() => new ModelClass().ID)
        /// </summary>
        public static String Name<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        #region ====== Attributes: ======
        /// <summary>
        /// 查找指定的 Attribute '特性' 内容列表 默认不查找继承链
        /// </summary>
        /// <typeparam name="A">指定的 Attribute '特性'</typeparam>
        /// <param name="memberInfo">元数据</param>
        /// <returns></returns>
        public static A[] AttributeFindALL<A>(this MemberInfo memberInfo) where A : System.Attribute
        {
            object[] attrs = memberInfo.GetCustomAttributes(typeof(A), false);
            return ConvertTool.ListConvertType(attrs, o => o as A);
        }
        /// <summary>
        /// 查找指定的 Attribute '特性' 内容对象 默认不查找继承链
        /// </summary>
        /// <typeparam name="A">指定的 Attribute '特性'</typeparam>
        /// <param name="memberInfo">元数据</param>
        /// <returns></returns>
        public static A AttributeFindOnly<A>(this MemberInfo memberInfo) where A : System.Attribute
        {
            A[] attrs = memberInfo.AttributeFindALL<A>();
            return CheckData.IsSizeEmpty(attrs) ? null : attrs[0];
        }
        #endregion

        /// <summary>
        /// 获得 "Object" 对象公共属性 名称列表
        /// </summary>
        public static String[] AttributeNames(Object obj)
        {
            List<String> names = new List<String>();
            Dictionary<String, String> attrKeyValues = AttributeKeyValues(obj);
            foreach (KeyValuePair<String, String> item in attrKeyValues)
            {
                names.Add(item.Key);
            }
            return names.ToArray();
        }

        /// <summary>
        /// 获得 "Object" 对象公共属性 及其值
        /// </summary>
        public static Dictionary<String, String> AttributeKeyValues(Object obj)
        {
            Dictionary<String, String> dicArray = new Dictionary<String, String>();
            PropertyInfo[] pis = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                dicArray.Add(pi.Name, pi.GetValue(obj, null).ToString());
            }
            return dicArray;
        }

        /// <summary>
        /// 获得 "Object" 对象公共字段 及其值
        /// </summary>
        public static Dictionary<String, String> FieldKeyValues(Object obj)
        {
            Dictionary<String, String> dicArray = new Dictionary<String, String>();
            FieldInfo[] fis = obj.GetType().GetFields();
            foreach (FieldInfo fi in fis)
            {
                dicArray.Add(fi.Name, fi.GetValue(obj).ToString());
            }
            return dicArray;
        }

        /// <summary>
        /// 克隆 对象 公共属性属性值 (但克隆DataGridView 需调用CloneDataGridView()方法)
        /// </summary>
        public static T CloneAllAttribute<T>(this T obj) where T : class
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            T model = (T)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, obj, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(obj, null);
                    pi.SetValue(model, value, null);
                }
            }
            return model;
        }

        /// <summary>
        /// 复制时间
        /// </summary>
        public static DateTime CopyDateTime(DateTime sourcetime)
        {
            return new DateTime(sourcetime.Year, sourcetime.Month, sourcetime.Day, sourcetime.Hour, sourcetime.Minute, sourcetime.Second, sourcetime.Millisecond, sourcetime.Kind);
        }
    }
}
