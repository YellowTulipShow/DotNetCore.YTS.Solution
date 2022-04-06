using System.Collections.Generic;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 接口 - 键值对自定义数据源
    /// </summary>
    /// <typeparam name="T">值数据类型</typeparam>
    public interface IKeyValuePairsDb<T>
    {
        /// <summary>
        /// 获取可操作存储区名单
        /// </summary>
        IEnumerable<OperableStoreShow> GetOperableStores();

        /// <summary>
        /// 写入键值对数据
        /// </summary>
        /// <param name="root">数据区域</param>
        /// <param name="kvPairs">存储键值对</param>
        /// <returns>执行结果, 处理成功的记录条数</returns>
        int Write(string root, IDictionary<string, T> kvPairs);

        /// <summary>
        /// 读取键值对数据
        /// </summary>
        /// <param name="root">数据区域</param>
        /// <param name="keyExpression">键读取表达式 (正则表达式)</param>
        /// <returns>匹配键读取表达式的键值对数据</returns>
        IDictionary<string, T> Read(string root, string keyExpression);
    }

    /// <inheritdoc />
    public interface IKeyValuePairsDb : IKeyValuePairsDb<object>
    {
    }
}
