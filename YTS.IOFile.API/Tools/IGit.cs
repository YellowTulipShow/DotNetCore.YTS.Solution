using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 接口: Git 操作工具
    /// </summary>
    public interface IGit
    {
        /// <summary>
        /// 相当于执行命令: `git add [filePath]` 命令, 添加文件进入暂存区
        /// </summary>
        /// <param name="filePath">文件相对路径, 为空则指定全部</param>
        void Add(string filePath);

        /// <summary>
        /// 相当于执行命令: `git status` 命令, 查询当前工作区状态
        /// </summary>
        /// <returns>响应输出文本内容</returns>
        StringBuilder Status();

        /// <summary>
        /// 相当于执行命令: `git commit -m "[message]"` 命令, 提交数据到存储库
        /// </summary>
        /// <param name="message">提交消息标题 (为空默认消息为: "save data")</param>
        /// <returns>响应输出文本内容</returns>
        StringBuilder Commit(string message);

        /// <summary>
        /// 相当于执行命令: `git fetch origin`, `git merge origin/master -m "[message]"` 命令, 拉取远程数据
        /// </summary>
        /// <param name="message">提交消息标题 (为空默认消息为: "save data")</param>
        /// <returns>响应输出文本内容</returns>
        StringBuilder Pull(string message);

        /// <summary>
        /// 相当于执行命令: `git push origin master` 命令, 推送数据到远程存储库
        /// </summary>
        /// <returns>响应输出文本内容</returns>
        StringBuilder Push();
    }
}
