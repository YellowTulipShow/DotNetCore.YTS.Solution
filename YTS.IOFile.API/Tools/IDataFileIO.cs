using System.Collections.Generic;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 接口: 数据转换
    /// </summary>
    /// <typeparam name="T">操作数据类型</typeparam>
    public interface IDataFileIO<T>
    {
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="fileAbsPath">文件绝对路径地址</param>
        /// <param name="data">写入的数据</param>
        void Write(string fileAbsPath, T data);

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="fileAbsPath">文件绝对路径地址</param>
        /// <returns>读取到的数据类型</returns>
        T Read(string fileAbsPath);
    }

    /// <summary>
    /// 接口: Object数据转换
    /// </summary>
    public interface IDataFileIO : IDataFileIO<object>
    {
    }
}
