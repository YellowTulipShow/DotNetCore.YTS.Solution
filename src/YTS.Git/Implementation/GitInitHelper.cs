using System.IO;
using System.Linq;

using YTS.Git.Command;
using YTS.Git.Models;

namespace YTS.Git.Implementation
{
    /// <inheritdoc />
    public class GitInitHelper : AbsGitCommandExecuteHelper, IGitInit
    {
        /// <inheritdoc />
        public GitInitHelper(Repository repository) : base(repository)
        {
        }

        /// <inheritdoc />
        public virtual void OnCommand()
        {
            DirectoryInfo dire = repository.RootPath;
            if (!dire.Exists)
            {
                dire.Create();
            }
            if (!dire.GetDirectories().Any(b => b.Name.ToLower() == @".git"))
            {
                OnExecuteOnlyCommand($"init", null);
            }
        }
    }
}
