using YTS.Git.Command;
using YTS.Git.Models;

namespace YTS.Git.Implementation
{
    /// <summary>
    /// Git 命令操作工具类
    /// </summary>
    public class GitCommands : IGit
    {
        private readonly Repository repository;

        private IGitInit git_init;
        private IGitAdd git_add;
        private IGitCommit git_commit;
        private IGitStatus git_status;

        /// <summary>
        /// 实例化 - Git 操作工具类
        /// </summary>
        /// <param name="repository">需操作的存储库配置</param>
        public GitCommands(Repository repository)
        {
            this.repository = repository;
        }

        /// <inheritdoc />
        public virtual IGitInit Init()
        {
            git_init ??= new GitInitHelper(repository);
            return git_init;
        }
        /// <inheritdoc />
        public virtual IGitAdd Add()
        {
            git_add ??= new GitAddHelper(repository);
            return git_add;
        }
        /// <inheritdoc />
        public virtual IGitCommit Commit()
        {
            git_commit ??= new GitCommitHelper(repository);
            return git_commit;
        }
        /// <inheritdoc />
        public virtual IGitStatus Status()
        {
            git_status ??= new GitStatusHelper(repository);
            return git_status;
        }
    }
}
