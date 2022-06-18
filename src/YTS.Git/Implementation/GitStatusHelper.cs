using System.Linq;
using System.Collections.Generic;

using YTS.Git.Command;
using YTS.Git.Models;

namespace YTS.Git.Implementation
{
    /// <inheritdoc />
    public class GitStatusHelper : AbsGitCommandExecuteHelper, IGitStatus
    {
        /// <inheritdoc />
        public GitStatusHelper(Repository repository) : base(repository)
        {
        }

        /// <inheritdoc />
        public virtual IList<string> OnCommand()
        {
            IList<string> lines = new List<string>();
            OnExecuteOnlyCommand("status", line =>
            {
                lines.Add(line);
            });
            return lines.ToArray();
        }
    }
}
