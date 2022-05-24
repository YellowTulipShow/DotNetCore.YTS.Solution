using System.Collections.Generic;

namespace YTS.IOFile.API.Tools.PathRuleParsing
{
    /// <summary>
    /// 接口: 路径规则解析
    /// </summary>
    public interface IPathRuleParsing
    {
        /// <summary>
        /// 转为写入的路径地址
        /// </summary>
        /// <param name="keyExpression">键匹配表达式</param>
        /// <returns>绝对地址路径</returns>
        PathResolutionResult ToWrite(string keyExpression);

        /// <summary>
        /// 转为读取的路径地址队列
        /// </summary>
        /// <param name="keyExpression">键匹配表达式</param>
        /// <returns>绝对地址路径队列(键,地址)</returns>
        IList<PathResolutionResult> ToRead(string keyExpression);
    }

    /// <summary>
    /// 接口: 路径规则解析需配置根项
    /// </summary>
    public interface IPathRuleParsingRootConfig : IPathRuleParsing
    {
        /// <summary>
        /// 需配置的根项
        /// </summary>
        /// <param name="root">根结点</param>
        void SetRoot(string root);
    }

    /// <summary>
    /// 路径解析结果
    /// </summary>
    public sealed class PathResolutionResult
    {
        /// <summary>
        /// 键标识内容
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 绝对路径地址
        /// </summary>
        public string AbsolutePathAddress { get; set; }
    }
}
