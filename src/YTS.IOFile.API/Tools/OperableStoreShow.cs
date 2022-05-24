using YTS.Git;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 可操作存储库展示数据模型
    /// </summary>
    public class OperableStoreShow
    {
        /// <summary>
        /// 存储库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述备注
        /// </summary>
        public string DescriptionRemarks { get; set; }
    }
}
