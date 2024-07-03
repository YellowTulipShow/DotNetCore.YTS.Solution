using System.Collections.Generic;
using System.IO;
using System.Linq;

using YTS.Log;
using YTS.Logic.IO;

namespace YTS.Command.FileRecognition
{
    /// <summary>
    /// 模型类
    /// </summary>
    public static class M
    {
        public class ExecuteParam
        {
            public string root_path;
            public string inventory_file_name;
            public bool is_recursive;
        }

        /// <summary>
        /// 清单模型
        /// </summary>
        public class Inventory
        {
            /// <summary>
            /// 当前清单名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 下级项信息
            /// </summary>
            public IList<M.Item> Subs { get; set; }
        }

        /// <summary>
        /// 子项模型
        /// </summary>
        public class Item
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 扩展名
            /// </summary>
            public string Extension { get; set; }
        }
    }
}
