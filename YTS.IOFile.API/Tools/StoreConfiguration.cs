namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 存储区配置数据模型
    /// </summary>
    public class StoreConfiguration
    {
        /// <summary>
        /// 系统绝对路径
        /// </summary>
        public string SystemAbsolutePath { get; set; }

        /// <summary>
        /// 描述备注
        /// </summary>
        public string DescriptionRemarks { get; set; }

        /// <summary>
        /// 是否使用Git保存数据
        /// </summary>
        public StoreConfigurationGit Git { get; set; }
    }

    /// <summary>
    /// 存储区配置数据模型 - Git类型存储库相关配置
    /// </summary>
    public class StoreConfigurationGit
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 远程仓库地址
        /// </summary>
        public string RemoteWarehouseAddress { get; set; }
    }
}
