namespace YTS
{
    /// <summary>
    /// 响应结果模板
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 执行状态代码
        /// </summary>
        public ResultStatueCode Code { get; set; }

        /// <summary>
        /// 执行结果描述
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 响应结果模板带泛型结果
    /// </summary>
    /// <typeparam name="T">结果数据类型</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 数据结果
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 响应结果静态扩展类
    /// </summary>
    public static class CStaticExtend_Result
    {
        /// <summary>
        /// 快速转换响应结果
        /// </summary>
        public static Result To(this ResultStatueCode code, string message)
        {
            return new Result() { Code = code, Message = message };
        }
        /// <summary>
        /// 快速转换响应结果
        /// </summary>
        public static Result<T> To<T>(this ResultStatueCode code, string message, T data)
        {
            return new Result<T>() { Code = code, Message = message , Data = data };
        }
    }
}
