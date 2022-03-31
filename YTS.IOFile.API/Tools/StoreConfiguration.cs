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
    /// 存储区配置数据模型 - Git
    /// </summary>
    public class StoreConfigurationGit
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 存储分支名称
        /// </summary>
        public string BranchName { get; set; } = "master";

        /// <summary>
        /// 远程仓库
        /// </summary>
        public StoreConfigurationGitRemoteWarehouse RemoteWarehouse { get; set; }
    }

    /// <summary>
    /// 存储区配置数据模型 - Git - 远程仓库
    /// </summary>
    public class StoreConfigurationGitRemoteWarehouse
    {
        /// <summary>
        /// 远程存储库标识名称
        /// </summary>
        public string OriginName { get; set; } = "origin";

        /// <summary>
        /// 远程仓库地址
        /// </summary>
        public string Address { get; set; }
    }
}
