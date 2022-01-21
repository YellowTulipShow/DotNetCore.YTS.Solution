
namespace YTS.Logic
{
    /// <summary>
    /// 接口: 缓存
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获取指定键 'key' 的数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>结果值</returns>
        string GetData(string key);

        /// <summary>
        /// 设置指定键 'key' 的数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void SetData(string key, string value);
    }
}