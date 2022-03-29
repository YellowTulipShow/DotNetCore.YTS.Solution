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
        /// <summary>
        /// 间隔内容
        /// </summary>
        private const string INTERVAL_PATH = @"_data";

        private readonly ILog log;

        /// <summary>
        /// 实例化 - 路径解析
        /// </summary>
        /// <param name="log">执行日志</param>
        public PathRuleParsing(ILog log)
        {
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
            string absPath = ToAbsPath(root);
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
        private string ToAbsPath(string root)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(path, root);
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
            string absPath = ToAbsPath(root);
            absPath = Path.Combine(absPath, INTERVAL_PATH);
            DirectoryInfo rootDire = new DirectoryInfo(absPath);
            IDictionary<string, string> result = new Dictionary<string, string>();
            // 每一层进行检查
            var catalogues = keyExpression.Split(':', StringSplitOptions.RemoveEmptyEntries);
            Regex IsHaveExpressionRegex = new Regex(@"^/(\.+)/(i?)$", RegexOptions.ECMAScript);

            SubDiresQuery(rootDire, catalogues, 0, new string[catalogues.Length], (key, path) =>
            {
                result[key] = path;
            });
            return result;
        }
        private void SubDiresQuery(DirectoryInfo dire, string[] catalogues, int catalogueIndex, string[] keys, Action<string, string> saveKeyPathFunc)
        {
            if (catalogueIndex >= catalogues.Length)
            {
                return;
            }
            var catalogue = catalogues[catalogueIndex];
            keys[catalogueIndex] = catalogue;

            Regex catalogueRegex = ToCatalogueRegex(catalogue);

            var subDires = dire.GetDirectories();
            for (int i = 0; i < subDires.Length; i++)
            {
                var subDire = subDires[i];
                var subDireName = subDire.Name;
                bool IsEqual;
                if (catalogueRegex != null)
                {
                    // 使用正则判断是否符合条件
                    IsEqual = catalogueRegex.Match(subDireName).Success;
                }
                else
                {
                    // 正常使用字符串判断
                    IsEqual = subDireName.Equals(catalogue);
                }
                if (IsEqual)
                {
                    keys[catalogueIndex] = subDireName;
                    SubDiresQuery(subDire, catalogues, catalogueIndex + 1, keys, saveKeyPathFunc);
                }
            }

            var subFiles = dire.GetFiles();
            for (int i = 0; i < subFiles.Length; i++)
            {
                var subFile = subFiles[i];
                var subFileName = subFile.Name;
                subFileName = Regex.Replace(subFileName, @"(\.[a-zA-Z]+)$", "",
                    RegexOptions.ECMAScript | RegexOptions.IgnoreCase);
                bool IsEqual;
                if (catalogueRegex != null)
                {
                    // 使用正则判断是否符合条件
                    IsEqual = catalogueRegex.Match(subFileName).Success;
                }
                else
                {
                    // 正常使用字符串判断
                    IsEqual = subFileName.Equals(catalogue);
                }
                if (IsEqual && catalogueIndex == catalogues.Length - 1)
                {
                    keys[catalogueIndex] = subFileName;
                    // 增加一条记录
                    saveKeyPathFunc(string.Join(":", keys), subFile.FullName);
                }
            }
        }

        private Regex ToCatalogueRegex(string catalogue)
        {
            RegexOptions regexOptions = RegexOptions.ECMAScript;
            if (catalogue[^1] == 'i')
            {
                regexOptions |= RegexOptions.IgnoreCase;
                catalogue = catalogue[0..^1];
            }
            if (catalogue[0] == '/' && catalogue[^1] == '/')
            {
                return new Regex(catalogue[1..^1]);
            }
            return null;
        }


        /// <summary>
        /// 过滤危险内容
        /// </summary>
        private string FilterHazardousContent(string content) {
            content = content?.Trim();
            if (string.IsNullOrEmpty(content))
                return content;
            content = Regex.Replace(content, @"\.{2,}", "");
            content = Regex.Replace(content, @"\/{2,}", "");
            content = Regex.Replace(content, @"\\{2,}", "");
            return content;
        }
    }
}
