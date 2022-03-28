using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using YTS.IOFile.API.Tools.DataSupportIO;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 路径解析
    /// </summary>
    public class PathRuleParsing
    {
        //private const string Interval path
        // 间隔内容
        private const string INTERVAL_PATH = @"_data";

        private readonly IDataSupportIO dataSupport;
        private readonly ILog log;

        /// <summary>
        /// 实例化 - 路径解析
        /// </summary>
        /// <param name="dataSupport">数据支持</param>
        /// <param name="log">执行日志</param>
        public PathRuleParsing(IDataSupportIO dataSupport, ILog log)
        {
            this.dataSupport = dataSupport;
            this.log = log;
        }

        /// <summary>
        /// 转为写入的路径地址
        /// </summary>
        /// <param name="root">根目录地址</param>
        /// <param name="key">键</param>
        /// <returns>绝对地址路径</returns>
        public string ToWriteIOPath(string root, string key)
        {
            root = FilterHazardousContent(root);
            key = FilterHazardousContent(key);
            string absPath = dataSupport.ToAbsPath(root);
            string keyPath = key.Replace(":", "/") + ".json";
            keyPath = Path.Combine(absPath, INTERVAL_PATH, keyPath);
            FileInfo file = new FileInfo(keyPath);
            if (!file.Exists)
            {
                var dire = file.Directory;
                if (!dire.Exists)
                {
                    dire.Create();
                }
            }
            return file.FullName;
        }

        /// <summary>
        /// 转为读取的路径地址队列
        /// </summary>
        /// <param name="root">根目录地址</param>
        /// <param name="keyExpression">键匹配表达式</param>
        /// <returns>绝对地址路径队列(键,地址)</returns>
        public IDictionary<string, string> ToReadIOPath(string root, string keyExpression)
        {
            root = FilterHazardousContent(root);
            keyExpression = FilterHazardousContent(keyExpression);
            string absPath = dataSupport.ToAbsPath(root);
            absPath = Path.Combine(absPath, INTERVAL_PATH);
            DirectoryInfo rootDire = new DirectoryInfo(absPath);
            IDictionary<string, string> result = new Dictionary<string, string>();
            // 每一层进行检查
            var catalogues = keyExpression.Split(':', StringSplitOptions.RemoveEmptyEntries);
            int catalogueIndex = 0;
            Regex isReExpression = new Regex(@"^/(\.+)/(i?)$", RegexOptions.Singleline);
            while (catalogueIndex < catalogues.Length)
            {
                var catalogue = catalogues[catalogueIndex];
                //var  isReExpression.IsMatch(catalogue);
                var subDires = rootDire.GetDirectories();
                for (int i = 0; i < subDires.Length; i++)
                {
                    var subDire = subDires[i];
                }
                catalogueIndex++;
            }

        }


        /// <summary>
        /// 过滤危险内容
        /// </summary>
        private string FilterHazardousContent(string content) {
            content = content?.Trim();
            if (string.IsNullOrEmpty(content))
                return content;
            content = Regex.Replace(content, @"\.+", "");
            content = Regex.Replace(content, @"\/+", "");
            content = Regex.Replace(content, @"\\+", "");
            return content;
        }
    }
}
