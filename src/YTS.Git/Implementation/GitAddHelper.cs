using System;
using System.Collections.Generic;
using System.Text;

using YTS.Git.Command;
using YTS.Git.Models;

namespace YTS.Git.Implementation
{
    /// <inheritdoc />
    public class GitAddHelper : AbsGitCommandExecuteHelper, IGitAdd
    {
        /// <inheritdoc />
        public GitAddHelper(Repository repository) : base(repository)
        {
        }

        /// <inheritdoc />
        public virtual void OnCommand()
        {
            OnExecuteOnlyCommand($"add .", null);
        }
        /// <inheritdoc />
        public virtual void OnCommand(string filePath)
        {
            OnExecuteOnlyCommand($"add {filePath}", null);
        }
        /// <inheritdoc />
        public virtual void OnCommand(IList<string> fileNames)
        {
            OnExecuteOnlyCommand($"add {string.Join(' ', fileNames)}", null);
        }
    }
}
