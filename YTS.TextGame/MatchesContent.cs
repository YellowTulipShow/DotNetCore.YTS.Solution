using CommandLine;

namespace YTS.TextGame
{
    /// <summary>
    /// 匹配项内容
    /// </summary>
    public class MatchesContent
    {
        /// <summary>
        /// 提取的匹配项内容
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// 打印输出的匹配项内容
        /// </summary>
        public string Print { get; set; }

        /// <summary>
        /// 匹配项内容的答案
        /// </summary>
        public string Answer { get; set; }
    }
}
