using System.Collections.Generic;
using System.IO;
using System.Linq;

using YTS.Log;
using YTS.Logic.IO;

namespace YTS.Command.FileRecognition
{
    /// <summary>
    /// 路径文件清单
    /// </summary>
    public class PathFileInventory
    {
        private readonly ILog log;
        /// <summary>
        /// 是否递归
        /// </summary>
        private readonly bool is_recursive;
        /// <summary>
        /// 清单文件名称
        /// </summary>
        private readonly string inventory_file_name;

        /// <summary>
        /// 初始化文件清单
        /// </summary>
        /// <param name="log">执行日志</param>
        /// <param name="is_recursive">是否递归</param>
        /// <param name="inventory_file_name">清单文件名称</param>
        public PathFileInventory(ILog log, bool is_recursive, string inventory_file_name)
        {
            this.log = log;
            this.is_recursive = is_recursive;
        }

        public IDictionary<string, M.Inventory> ToMDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
                return null;
            IDictionary<string, object> logArgs = log.CreateArgDictionary();

            string inventoryJSONName = $"{inventory_file_name}.json";
            string inventory_json_path = $"{directoryInfo.FullName}\\{inventoryJSONName}";
            inventory_json_path = FilePathExtend.ToAbsolutePath(inventory_json_path);
            logArgs["inventory_json_path"] = inventory_json_path;
            log.Info("计算JSON清单路径", logArgs);

            IDictionary<string, M.Inventory> rdict;
            rdict = new Dictionary<string, M.Inventory>();
            IList<M.Item> items = new List<M.Item>();

            var sub_files = directoryInfo.GetFiles();
            logArgs["sub_files.Length"] = sub_files.Length;

            log.Info("添加文件类型项", logArgs);
            foreach (var file in sub_files)
            {
                if (file.Name == inventoryJSONName)
                    continue;
                items.Add(ToItem(file));
            }

            var sub_directorys = directoryInfo.GetDirectories();
            logArgs["directoryInfo.FullName"] = directoryInfo.FullName;
            logArgs["sub_directorys.Length"] = sub_directorys.Length;
            log.Info("添加目录类型项", logArgs);
            foreach (var dir in sub_directorys)
            {
                if (is_recursive)
                {
                    var sub_dict = ToMDirectory(dir);
                    rdict = rdict.Union(sub_dict).ToDictionary(kv => kv.Key, kv => kv.Value);
                }
                items.Add(ToItem(dir));
            }

            var result = new M.Inventory
            {
                Name = directoryInfo.Name,
                Subs = items,
            };
            rdict[inventory_json_path] = result;
            return rdict;
        }
        private M.Item ToItem(FileInfo m)
        {
            return new M.Item()
            {
                Name = m.Name,
                Type = "File",
                Extension = m.Extension.ToLower(),
            };
        }
        private M.Item ToItem(DirectoryInfo m)
        {
            return new M.Item()
            {
                Name = m.Name,
                Type = "Dir",
                Extension = null,
            };
        }
    }
}
