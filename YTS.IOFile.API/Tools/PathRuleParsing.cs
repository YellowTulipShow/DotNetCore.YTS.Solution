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
            Regex IsHaveExpressionRegex = new Regex(@"^/(\.+)/(i?)$", RegexOptions.ECMAScript);

            SubDiresQuery(rootDire, 0);
            void SubDiresQuery(DirectoryInfo dire, int catalogueIndex)
            {
                if (catalogueIndex >= catalogues.Length)
                {
                    return;
                }
                var catalogue = catalogues[catalogueIndex];
                Regex catalogueRegex = null;
                if (IsHaveExpressionRegex.IsMatch(catalogue))
                {
                    Match expressionMatch = IsHaveExpressionRegex.Match(catalogue);
                    string pattern = expressionMatch.Groups[1].Value;
                    RegexOptions regexOptions = RegexOptions.ECMAScript;
                    if (expressionMatch.Groups[2].Value == "i")
                    {
                        regexOptions |= RegexOptions.IgnoreCase;
                    }
                    catalogueRegex = new Regex(pattern, regexOptions);
                }

                var subDires = rootDire.GetDirectories();
                for (int i = 0; i < subDires.Length; i++)
                {
                    var subDire = subDires[i];
                    bool IsEqual;
                    if (catalogueRegex != null)
                    {
                        // 使用正则判断是否符合条件
                        IsEqual = catalogueRegex.Match(subDire.Name).Success;
                    }
                    else
                    {
                        // 正常使用字符串判断
                        IsEqual = subDire.Name.Equals(catalogue);
                    }
                    if (IsEqual)
                    {
                        catalogues[catalogueIndex] = subDire.Name;
                        SubDiresQuery(subDire, catalogueIndex + 1);
                    }
                }

                var subFiles = rootDire.GetFiles();
                for (int i = 0; i < subFiles.Length; i++)
                {
                    var subFile = subFiles[i];
                    bool IsEqual;
                    if (catalogueRegex != null)
                    {
                        // 使用正则判断是否符合条件
                        IsEqual = catalogueRegex.Match(subFile.Name).Success;
                    }
                    else
                    {
                        // 正常使用字符串判断
                        IsEqual = subFile.Name.Equals(catalogue);
                    }
                    if (IsEqual && catalogueIndex == catalogues.Length - 1)
                    {
                        catalogues[catalogueIndex] = subFile.Name;
                        // 增加一条记录
                        result.Add(string.Join(":", catalogues), subFile.FullName);
                    }
                }
            };
            return result;
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
