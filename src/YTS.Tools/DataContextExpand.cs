using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace YTS.Tools
{
    public static class DataContextExpand
    {
        public readonly static string[] OrderByKeys = new string[] { "desc", "asc" };

        /// <summary>
        /// 传入排序方法, 自定义并限制类型字段排序
        /// </summary>
        /// <param name="list">数据集</param>
        /// <param name="sort">排序字段</param>
        /// <param name="order">排序顺序: desc, asc</param>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <returns>返回: 集合对象</returns>
        public static IQueryable<T> ToOrderBy<T>(
            this IQueryable<T> list,
            string sort, string order)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return list;
            order = string.IsNullOrWhiteSpace(order) ? OrderByKeys[0] : order;
            order = OrderByKeys.Contains(order) ? order : OrderByKeys[0];
            var propertyInfos = typeof(T).GetProperties();
            if (propertyInfos.Length <= 0)
                throw new NotSupportedException("属性列表为空无法设置排序!");
            sort = propertyInfos.Any(p => p.Name == sort) ? sort : propertyInfos[0].Name;
            return list.OrderBy($"{sort} {order}");
        }

        /// <summary>
        /// 分页方法, 可自定义分页
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="list">数据集</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示几条</param>
        /// <param name="changeRecordCount">输出总页数</param>
        /// <returns>返回: 集合对象</returns>
        public static IQueryable<T> ToPager<T>(
            this IQueryable<T> list,
            int? page, int? rows,
            Action<int> changeRecordCount = null)
        {
            changeRecordCount?.Invoke(list.Count());
            if (page == null)
                return list;
            int pageIndex = page ?? 1;
            int pageSize = rows ?? 10;
            return list.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }
    }
}
