using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using YTS.Logic.Data;
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
        /// <summary>
        /// 键表达式间隔符合
        /// </summary>
        private const char KEY_EXPRESSION_JOIN_CHAR = ':';

        private readonly ILog log;

        /// <summary>
        /// 实例化 - 路径规则解析 - 保存为JSON文件格式
        /// </summary>
        /// <param name="log">执行日志</param>
        public PathRuleParsingJSON(ILog log)
        {
            this.log = log;
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

            Regex catalogueRegex = Regex_catalogue_content(catalogue);

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
            string keyPath = keyExpression.Replace(KEY_EXPRESSION_JOIN_CHAR, '/') + EXTENSION_NAME;
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
            // 每一层进行检查
            string[] keys = SplitKeyExpression(keyExpression);
            string[] keys_modify = keys[0..^1];
            return ReadAnalysis(rootDire, keys, keys_modify);
        }
        private string[] SplitKeyExpression(string keyExpression)
        {
            return keyExpression.Split(KEY_EXPRESSION_JOIN_CHAR, StringSplitOptions.RemoveEmptyEntries);
        }
        private string MergeToKeyExpression(string[] keys)
        {
            return string.Join(KEY_EXPRESSION_JOIN_CHAR, keys);
        }
        private IList<PathResolutionResult> ReadAnalysis(DirectoryInfo rootDire, string[] keys, string[] keys_modify, int keyIndex = 0)
        {
            if (rootDire != null)
            {
                return null;
            }
            if (!rootDire.Exists)
            {
                rootDire.Create();
                return null;
            }
            string region = keys.Value(keyIndex, null)?.Trim();
            if (string.IsNullOrEmpty(region))
            {
                return null;
            }
            Regex is_like_query = Regex_is_like_query();
            Match like_query_match = is_like_query.Match(region);
            // 判断是否是模糊查询
            if (!like_query_match.Success)
            {
                return ReadAnalysis_DefalutNameEquals(rootDire, keys, keys_modify, keyIndex, region);
            }
            else
            {
                return ReadAnalysis_LikeQuery(rootDire, keys, keys_modify, keyIndex, region);
            }
        }
        private Regex Regex_is_like_query()
        {
            return new Regex(@"^/(\.+)/(i?)$", RegexOptions.ECMAScript); ;
        }
        private IList<PathResolutionResult> ReadAnalysis_LikeQuery(DirectoryInfo rootDire, string[] keys, string[] keys_modify, int keyIndex, string region)
        {
            List<PathResolutionResult> rlist = new List<PathResolutionResult>();
            Regex regex_catalogue_content = Regex_catalogue_content(region);

            // 不是最后一项表示目录
            if (keyIndex != keys.Length - 1)
            {
                var all_dires = rootDire.GetDirectories();
                foreach (var dire in all_dires)
                {
                    if (regex_catalogue_content.IsMatch(dire.Name))
                    {
                        keys_modify[keyIndex] = region;
                        rlist.AddRange(ReadAnalysis(dire, keys, keys_modify, keyIndex + 1));
                    }
                }
            }
            // 最后一项表示文件
            else
            {
                var all_files = rootDire.GetFiles();
                foreach (var file in all_files)
                {
                    string file_name = file.Name.Replace(EXTENSION_NAME, string.Empty);
                    if (regex_catalogue_content.IsMatch(file_name))
                    {
                        keys_modify[keyIndex] = region;
                        rlist.Add(new PathResolutionResult()
                        {
                            Key = MergeToKeyExpression(keys_modify),
                            AbsolutePathAddress = file.FullName,
                        });
                    }
                }
            }
            return rlist;
        }
        private Regex Regex_catalogue_content(string catalogue)
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
        private IList<PathResolutionResult> ReadAnalysis_DefalutNameEquals(DirectoryInfo rootDire, string[] keys, string[] keys_modify, int keyIndex, string region)
        {
            List<PathResolutionResult> rlist = new List<PathResolutionResult>();
            // 不是最后一项表示目录
            if (keyIndex != keys.Length - 1)
            {
                var all_dires = rootDire.GetDirectories();
                foreach (var dire in all_dires)
                {
                    if (dire.Name == region)
                    {
                        keys_modify[keyIndex] = region;
                        rlist.AddRange(ReadAnalysis(dire, keys, keys_modify, keyIndex + 1));
                    }
                }
            }
            // 最后一项表示文件
            else
            {
                var all_files = rootDire.GetFiles();
                foreach (var file in all_files)
                {
                    string file_name = file.Name.Replace(EXTENSION_NAME, string.Empty);
                    if (file_name == region)
                    {
                        keys_modify[keyIndex] = region;
                        rlist.Add(new PathResolutionResult()
                        {
                            Key = MergeToKeyExpression(keys_modify),
                            AbsolutePathAddress = file.FullName,
                        });
                        break;
                    }
                }
            }
            return rlist;
        }
    }
}
