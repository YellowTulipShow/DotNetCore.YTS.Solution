using System.IO;
using System.Text;

namespace YTS.Git.Models
{
    /// <summary>
    /// 存储库
    /// </summary>
    public struct Repository
    {
        /// <summary>
        /// 所在系统目录地址
        /// </summary>
        public DirectoryInfo RootPath { get; set; }

        /// <summary>
        /// 输出的文本编码格式定义
        /// </summary>
        public Encoding OutputTextEncoding { get; set; }
    }
}
