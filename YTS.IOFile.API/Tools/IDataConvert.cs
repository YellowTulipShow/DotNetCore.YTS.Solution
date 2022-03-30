using System.Collections.Generic;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 接口: 数据
    /// </summary>
    interface IDataConvert
    {
        /// <summary>
        /// 转为写入的路径地址
        /// </summary>
        /// <param name="root">根目录地址</param>
        /// <param name="key">键</param>
        /// <returns>绝对地址路径</returns>
        string ToWriteIOPath(string root, string key);

        /// <summary>
        /// 转为读取的路径地址队列
        /// </summary>
        /// <param name="root">根目录地址</param>
        /// <param name="keyExpression">键匹配表达式</param>
        /// <returns>绝对地址路径队列(键,地址)</returns>
        IDictionary<string, string> ToReadIOPath(string root, string keyExpression);
    }
}
