using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using YTS.Log;
using YTS.Logic.IO;

namespace YTS.Command.FileRecognition
{
    /// <summary>
    /// 主程序逻辑
    /// </summary>
    public interface IMain
    {
        /// <summary>
        /// 执行程序逻辑
        /// </summary>
        /// <param name="param">参数</param>
        void OnExecute(M.ExecuteParam param);
    }

    /// <summary>
    /// 执行程序
    /// </summary>
    public class Main : IMain
    {
        private readonly ILog log;
        private readonly Encoding encoding;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">执行编码标准</param>
        public Main(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
        }

        void IMain.OnExecute(M.ExecuteParam param)
        {
            Console.WriteLine($"路径: {param.root_path}");
            string r_name = param.is_recursive ? "递归" : "不递归";
            Console.WriteLine($"是否递归: {r_name}");

            DirectoryInfo root_dire = new DirectoryInfo(param.root_path);
            if (!root_dire.Exists)
            {
                Console.WriteLine($"根目录不存在, 执行错误: {param.root_path}");
                return;
            }
            PathFileInventory tool = new PathFileInventory(log, param.is_recursive, param.inventory_file_name);
            IDictionary<string, M.Inventory> rdict = tool.ToMDirectory(root_dire);
            WriteInventoryInfo(rdict);
        }
        private void WriteInventoryInfo(IDictionary<string, M.Inventory> dicts)
        {
            foreach (var file in dicts.Keys)
            {
                File.WriteAllText(file, JsonConvert.SerializeObject(dicts[file]), encoding);
            }
        }
    }
}
