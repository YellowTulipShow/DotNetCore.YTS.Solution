namespace YTS.Logic.Data
{
    /// <summary>
    /// 数组相关扩展
    /// </summary>
    public static class ArrayExtend
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">数组内容数据类型</typeparam>
        /// <param name="arr">数组</param>
        /// <param name="index">索引标识</param>
        /// <param name="def">默认返回值</param>
        /// <returns>结果</returns>
        public static T Value<T>(this T[] arr, int index, T def = default)
        {
            if (arr == null || arr.Length <= 0 || index < 0 || index >= arr.Length)
            {
                return def;
            }
            return arr[index];
        }
    }
}
