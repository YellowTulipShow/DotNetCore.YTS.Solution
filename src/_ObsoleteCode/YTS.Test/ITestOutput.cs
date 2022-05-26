namespace YTS.Test
{
    /// <summary>
    /// 测试输出
    /// </summary>
    public interface ITestOutput
    {
        /// <summary>
        /// 初始化方法
        /// </summary>
        void OnInit();

        /// <summary>
        /// 结尾方法
        /// </summary>
        void OnEnd();

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="msg">消息</param>
        void Write(string msg);

        /// <summary>
        /// 输出错误消息
        /// </summary>
        /// <param name="msg">错误消息</param>
        void WriteError(string msg);

        /// <summary>
        /// 输出信息消息
        /// </summary>
        /// <param name="msg">信息消息</param>
        void WriteInfo(string msg);

        /// <summary>
        /// 输出警告消息
        /// </summary>
        /// <param name="msg">警告消息</param>
        void WriteWarning(string msg);

        /// <summary>
        /// 输出消息 - 换行
        /// </summary>
        /// <param name="msg">消息</param>
        void WriteLine(string msg);

        /// <summary>
        /// 输出错误消息 - 换行
        /// </summary>
        /// <param name="msg">错误消息</param>
        void WriteLineError(string msg);

        /// <summary>
        /// 输出信息消息 - 换行
        /// </summary>
        /// <param name="msg">信息消息</param>
        void WriteLineInfo(string msg);

        /// <summary>
        /// 输出警告消息 - 换行
        /// </summary>
        /// <param name="msg">警告消息</param>
        void WriteLineWarning(string msg);
    }
}
