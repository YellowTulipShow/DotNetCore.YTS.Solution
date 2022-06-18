using System;
using System.Collections.Generic;
using System.Text;

using YTS.Git.Command;
using YTS.Git.Models;

namespace YTS.Git.Implementation
{
    /// <inheritdoc />
    public class GitCommitHelper : AbsGitCommandExecuteHelper, IGitCommit
    {
        /// <inheritdoc />
        public GitCommitHelper(Repository repository) : base(repository)
        {
        }

        /// <inheritdoc />
        public virtual void OnCommand(string message)
        {
            OnExecuteOnlyCommand($"commit -m \"{message}\"", null);
        }
    }
}
