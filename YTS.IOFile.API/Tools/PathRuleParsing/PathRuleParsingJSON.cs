﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using YTS.Logic.Log;

namespace YTS.IOFile.API.Tools.PathRuleParsing
{
    /// <summary>
    /// 路径规则解析 - 保存为JSON文件格式
    /// </summary>
    public class PathRuleParsingJSON : IPathRuleParsingRootConfig
    {
        /// <summary>
        /// 间隔内容
        /// </summary>
        private const string INTERVAL_PATH = @"_data";

        /// <summary>
        /// 文件扩展名称 .json
        /// </summary>
        private const string EXTENSION_NAME = @".json";

        private readonly ILog log;

        /// <summary>
        /// 实例化 - 路径规则解析 - 保存为JSON文件格式
        /// </summary>
        /// <param name="log">执行日志</param>
        public PathRuleParsingJSON(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc />
        public string ToWriteIOPath(string root, string key)
        {
            root = FilterHazardousContent(root);
            key = FilterHazardousContent(key);
            string absPath = ToAbsPath(root);
            string keyPath = key.Replace(":", "/") + EXTENSION_NAME;
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
        private string FilterHazardousContent(string content)
        {
            content = content?.Trim();
            if (string.IsNullOrEmpty(content))
                return content;
            content = Regex.Replace(content, @"\.{2,}", "");
            content = Regex.Replace(content, @"\/{2,}", "");
            content = Regex.Replace(content, @"\\{2,}", "");
            return content;
        }
        private string ToAbsPath(string rootPath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(path, rootPath);
        }

        /// <inheritdoc />
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
            if (!dire.Exists)
            {
                dire.Create();
            }
            if (catalogueIndex >= catalogues.Length)
            {
                return;
            }
            var catalogue = catalogues[catalogueIndex];
            keys[catalogueIndex] = catalogue;

            Regex catalogueRegex = ToCatalogueRegex(catalogue);

            // 最后一项才输出文件内容
            if (catalogueIndex != catalogues.Length - 1)
            {
                // 目录
                var subDires = dire.GetDirectories();
                for (int i = 0; i < subDires.Length; i++)
                {
                    var subDire = subDires[i];
                    var subDireName = subDire.Name;
                    if (IsNameEqual(catalogue, catalogueRegex, subDireName))
                    {
                        keys[catalogueIndex] = subDireName;
                        SubDiresQuery(subDire, catalogues, catalogueIndex + 1, keys, saveKeyPathFunc);
                    }
                }
            }
            else
            {
                // 文件
                var subFiles = dire.GetFiles();
                for (int i = 0; i < subFiles.Length; i++)
                {
                    var subFile = subFiles[i];
                    var subFileName = subFile.Name.Replace(EXTENSION_NAME, string.Empty);
                    if (IsNameEqual(catalogue, catalogueRegex, subFileName))
                    {
                        keys[catalogueIndex] = subFileName;
                        // 增加一条记录
                        saveKeyPathFunc(string.Join(":", keys), subFile.FullName);
                    }
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
        private bool IsNameEqual(string catalogue, Regex catalogueRegex, string subDireName)
        {
            if (catalogueRegex != null)
            {
                // 使用正则判断是否符合条件
                return catalogueRegex.Match(subDireName).Success;
            }
            else
            {
                // 正常使用字符串判断
                return subDireName.Equals(catalogue);
            }
        }


        private string Root { get; set; }
        /// <inheritdoc />
        public void SetRoot(string root)
        {
            Root = FilterHazardousContent(root);
        }

        /// <inheritdoc />
        public PathResolutionResult ToWrite(string keyExpression)
        {
            keyExpression = FilterHazardousContent(keyExpression);
            string absPath = ToAbsPath(Root);
            string keyPath = keyExpression.Replace(":", "/") + EXTENSION_NAME;
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
            return new PathResolutionResult()
            {
                Key = keyExpression,
                AbsolutePathAddress = file.FullName,
            };
        }

        /// <inheritdoc />
        public IEnumerable<PathResolutionResult> ToRead(string keyExpression)
        {
            keyExpression = FilterHazardousContent(keyExpression);
            string absPath = ToAbsPath(Root);
            absPath = Path.Combine(absPath, INTERVAL_PATH);
            DirectoryInfo rootDire = new DirectoryInfo(absPath);
            if (!rootDire.Exists)
            {
                rootDire.Create();
                return null;
            }
            IList<PathResolutionResult> rlist = new List<PathResolutionResult>();
            // 每一层进行检查
            string[] regions = keyExpression.Split(':', StringSplitOptions.RemoveEmptyEntries);
            string[] keys = new string[regions.Length];
            Regex is_like_query = new Regex(@"^/(\.+)/(i?)$", RegexOptions.ECMAScript);
            for (int index_regions = 0; index_regions < regions.Length; index_regions++)
            {
                string region = regions[index_regions];
                Match like_query_match = is_like_query.Match(region);
                // 判断是否是模糊查询
                if (!like_query_match.Success)
                {
                    keys[index_regions] = region;
                    // 不是模糊查询
                    if (index_regions != regions.Length - 1)
                    {
                        // 不是最后一项
                        continue;
                    }
                    // 是最后一项判断文件
                    string merge_key = string.Join('/', keys);
                    string relative_path = $"{merge_key}{EXTENSION_NAME}";
                    string file_path = Path.Combine(rootDire.FullName, relative_path);
                    if (File.Exists(file_path))
                    {
                        rlist.Add(new PathResolutionResult()
                        {
                            Key = keyExpression,
                            AbsolutePathAddress = file_path,
                        });
                    }
                }
                else
                {

                    string[] ww = regions[0..^3];
                    Regex re_name = ToCatalogueRegex(region);
                    var subDires = rootDire.GetDirectories();
                    for (int i = 0; i < subDires.Length; i++)
                    {
                        var subDire = subDires[i];
                        var subDireName = subDire.Name;
                        if (re_name.IsMatch(subDireName))
                        {
                        }
                    }
                }
            }
        }
    }
}
