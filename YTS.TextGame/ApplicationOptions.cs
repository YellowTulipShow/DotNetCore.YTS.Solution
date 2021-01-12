using CommandLine;

namespace YTS.TextGame
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        /// 文件/夹路径地址
        /// </summary>
        [Option('p', "path", Required = true,
            HelpText = "文件/夹路径地址")]
        public string Path { get; set; }

        /// <summary>
        /// 需要读取的文件扩展名
        /// </summary>
        [Option("extname", Default = ".md",
            HelpText = "需要读取的文件扩展名")]
        public string FileExtension { get; set; }

        /// <summary>
        /// 提取文件内容的正则表达式
        /// </summary>
        [Option("re-input", Default = @"([a-zA-Z]+) \| \[([^\[\]]+)\] \| ([^\n]+)",
            HelpText = "提取文件内容的正则表达式")]
        public string Re_Input { get; set; }

        /// <summary>
        /// 打印在屏幕上的正则表达式
        /// </summary>
        [Option("re-print", Default = @"$1 - $2 - $3",
            HelpText = "打印在屏幕上的正则表达式")]
        public string Re_Print { get; set; }

        /// <summary>
        /// 需要充当答案的正则表达式
        /// </summary>
        [Option("re-answer", Default = "$1",
            HelpText = "需要充当答案的正则表达式")]
        public string Re_Answer { get; set; }

        /// <summary>
        /// 单个匹配项重复次数
        /// </summary>
        [Option("count-work-repeat", Default = 5,
            HelpText = "单个匹配项重复次数")]
        public int CountWorkRepeat { get; set; }

        /// <summary>
        /// 一回合几个匹配项
        /// </summary>
        [Option("count-round-work", Default = 3,
            HelpText = "一回合几个匹配项")]
        public int CountRoundWork { get; set; }
    }
}
