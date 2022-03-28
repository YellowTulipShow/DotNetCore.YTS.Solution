namespace YTS.IOFile.API.Tools.DataSupportIO
{
    /// <summary>
    /// 数据支持IO - 静态扩展方法
    /// </summary>
    public static class DataSupportIOFactory
    {
        /// <summary>
        /// 实例工厂
        /// </summary>
        public static IDataSupportIO Default()
        {
            return new DataSupportIOWindowSystem();
        }
    }
}
