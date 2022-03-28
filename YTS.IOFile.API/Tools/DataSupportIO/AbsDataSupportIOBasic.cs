using System;
using System.IO;

namespace YTS.IOFile.API.Tools.DataSupportIO
{
    /// <summary>
    /// 抽象基础类型系统数据支持IO
    /// </summary>
    public abstract class AbsDataSupportIOBasic : IDataSupportIO
    {
        /// <inheritdoc />
        public string ToAbsPath(string root)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(path, root);
        }
    }
}
