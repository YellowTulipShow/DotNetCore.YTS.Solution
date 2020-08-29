using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YTS.Tools
{
    /// <summary>
    /// 运行帮助类
    /// </summary>
    public class RunHelp
    {
        /// <summary>
        /// 获取运行时间 单位: 秒
        /// </summary>
        /// <param name="method">需要执行的方法</param>
        /// <returns>执行方法所需的时间 单位: 秒</returns>
        public static double GetRunTime(Action method)
        {
            if (CheckData.IsObjectNull(method))
            {
                method = () => { };
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); // 开始
            method();
            stopwatch.Stop(); // 结束
            TimeSpan runtimeSpan = stopwatch.Elapsed;
            return runtimeSpan.TotalSeconds;
        }
    }
}
