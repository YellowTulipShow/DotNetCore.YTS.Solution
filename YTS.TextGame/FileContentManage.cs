using System;
using System.Collections.Generic;

namespace YTS.TextGame
{
    /// <summary>
    /// 文件内容管理
    /// </summary>
    public class FileContentManage
    {
        private readonly ApplicationOptions appArgs;

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
            return new List<MatchesContent>()
            {
            };
        }
    }
}
