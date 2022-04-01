using YTS.Git;

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

        private Repository _git;
        /// <summary>
        /// 是否使用Git保存数据
        /// </summary>
        public Repository Git
        {
            get
            {
                if (string.IsNullOrEmpty(_git.SystemPath))
                {
                    _git.SystemPath = SystemAbsolutePath?.Trim();
                }
                return _git;
            }
            set
            {
                _git = value;
            }
        }
    }
}
