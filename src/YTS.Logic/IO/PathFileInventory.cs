﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

namespace YTS.Logic.IO
{
    /// <summary>
    /// 路径文件清单
    /// </summary>
    public class PathFileInventory
    {
        private readonly ILog log;

        /// <summary>
        /// 初始化文件清单
        /// </summary>
        public PathFileInventory(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 转为描述数据
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="IsCalcSub">是否计算子级</param>
        /// <returns>键: 清单文件路径, 值: 清单内容</returns>
        public IDictionary<string, MInventory> ToExplainDatas(string directoryPath, bool IsCalcSub = true)
        {
            DirectoryInfo info = new DirectoryInfo(directoryPath);
            return ToMDirectory(info, IsCalcSub);
        }

        /// <summary>
        /// 转为描述数据
        /// </summary>
        /// <param name="directoryInfo">目录信息</param>
        /// <param name="IsCalcSub">是否计算子级</param>
        /// <returns>键: 清单文件路径, 值: 清单内容</returns>
        public IDictionary<string, MInventory> ToMDirectory(DirectoryInfo directoryInfo, bool IsCalcSub = true)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
                return null;
            const string inventoryJSONName = "_inventory.json";
            string inventory_json_path = $"{directoryInfo.FullName}\\{inventoryJSONName}";
            inventory_json_path = FilePathExtend.ToAbsolutePath(inventory_json_path);
            log.Info("计算JSON清单路径", new Dictionary<string, object>()
            {
                { "inventory_json_path", inventory_json_path },
            });
            var dict = new Dictionary<string, MInventory>();
            var items = new List<MItem>();

            var sub_files = directoryInfo.GetFiles();
            log.Info("添加文件类型项", new Dictionary<string, object>()
            {
                { "sub_files.Length", sub_files.Length },
            });
            foreach (var file in sub_files)
            {
                if (file.Name == inventoryJSONName)
                    continue;
                items.Add(ToMItem(file.Name, "File"));
            }

            var sub_directorys = directoryInfo.GetDirectories();
            log.Info("添加目录类型项", new Dictionary<string, object>()
            {
                { "directoryInfo.FullName", directoryInfo.FullName },
                { "sub_directorys.Length", sub_directorys.Length },
            });
            foreach (var dir in sub_directorys)
            {
                if (IsCalcSub)
                {
                    var sub_dict = ToMDirectory(dir);
                    dict = dict.Union(sub_dict).ToDictionary(kv => kv.Key, kv => kv.Value);
                }
                items.Add(ToMItem(dir.Name, "Dir"));
            }

            var result = new MInventory
            {
                Name = directoryInfo.Name,
                SubMItems = items.ToArray(),
            };
            dict[inventory_json_path] = result;
            return dict;
        }
        private MItem ToMItem(string name, string type) => new MItem() { Name = name, Type = type };

        /// <summary>
        /// 清单数据模型
        /// </summary>
        public class MInventory
        {
            /// <summary>
            /// 当前清单名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 下级项信息
            /// </summary>
            public MItem[] SubMItems { get; set; }
        }
        /// <summary>
        /// 清单数据模型内容项描述
        /// </summary>
        public class MItem
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 类型: File/Dir
            /// </summary>
            public string Type { get; set; }
        }

        /// <summary>
        /// 录入文件清单信息
        /// </summary>
        /// <param name="dicts">清单键值对</param>
        /// <param name="log">执行日志</param>
        /// <param name="encoding">清单文件写入编码</param>
        public void WriteInventoryInfo(IDictionary<string, MInventory> dicts, ILog log, Encoding encoding)
        {
            foreach (var file in dicts.Keys)
            {
                log.Info("输出清单内容", new Dictionary<string, object>()
                {
                    { "file", file },
                });
                File.WriteAllText(file, JsonConvert.SerializeObject(dicts[file]), encoding);
            }
        }
    }
}
