namespace YTS.IOFile.API.Tools.DataSupportIO
{
    /// <summary>
    /// 接口 - 数据支持IO
    /// </summary>
    public interface IDataSupportIO
    {
        /// <summary>
        /// 转为绝对路径的根目录地址
        /// </summary>
        /// <param name="root">(相对的)根目录</param>
        /// <returns>绝对路径</returns>
        string ToAbsPath(string root);
    }
}
