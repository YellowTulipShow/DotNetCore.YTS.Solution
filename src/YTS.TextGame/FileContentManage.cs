using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using YTS.Tools;
using System.Text.RegularExpressions;

namespace YTS.TextGame
{
    /// <summary>
    /// 文件内容管理
    /// </summary>
    public class FileContentManage
    {
        private readonly ApplicationOptions appArgs;
        private IList<FileInfo> rootAllFiles;

        /// <summary>
        /// 初始化文件内容管理
        /// </summary>
        /// <param name="appArgs">应用程序配置项</param>
        public FileContentManage(ApplicationOptions appArgs)
        {
            this.appArgs = appArgs;
        }

        public IList<MatchesContent> GetRandomMatchesContents()
        {
            IList<FileInfo> fileInfos = GetRootAllFileInfos();
            int random_count = RandomData.GetInt(1, 5);
            var rlist = new List<MatchesContent>();
            for (int i = 0; i < random_count; i++)
            {
                var fileInfo = RandomData.GetItem(fileInfos);
                rlist.AddRange(FileContentToRandomMatchesContents(fileInfo));
            }
            return rlist;
        }

        private IList<FileInfo> GetRootAllFileInfos()
        {
            if (rootAllFiles != null && rootAllFiles.Count > 0)
                return rootAllFiles;
            string rootPath = appArgs.Path;
            if (!Directory.Exists(rootPath))
            {
                if (!File.Exists(rootPath))
                {
                    throw new Exception($"路径: {rootPath} 异常错误!");
                }
                rootAllFiles = new List<FileInfo>() {
                    new FileInfo(rootPath),
                };
            }
            rootAllFiles = GetFileInfos(new DirectoryInfo(rootPath));
            return rootAllFiles;
        }

        private IList<FileInfo> GetFileInfos(DirectoryInfo root)
        {
            List<FileInfo> list = root.GetFiles()
                .Where(b => b.Extension == appArgs.FileExtension)
                .ToList() ?? new List<FileInfo>();
            var dirs = root.GetDirectories();
            foreach (var dir in dirs)
            {
                list.AddRange(GetFileInfos(dir));
            }
            return list;
        }

        private IList<MatchesContent> FileContentToRandomMatchesContents(FileInfo fileInfo)
        {
            IList<MatchesContent> list = new List<MatchesContent>();
            string text = File.ReadAllText(fileInfo.FullName);
            Regex re = new Regex(appArgs.Re_Input);
            MatchCollection colls = re.Matches(text);
            int random_count = RandomData.GetInt(5, 8);
            for (int i = 0; i < random_count; i++)
            {
                Match coll = RandomData.GetItem(colls);
                string value = coll.Value;
                var Input = value;
                var Print = re.Replace(value, appArgs.Re_Print);
                var Answer = re.Replace(value, appArgs.Re_Answer);
                list.Add(new MatchesContent()
                {
                    Input = Input,
                    Print = Print,
                    Answer = Answer,
                });
            }
            return list;
        }
    }
}
