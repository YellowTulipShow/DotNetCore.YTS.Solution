using System;
using System.Text;
using System.Threading;

namespace YTS.Tools
{
    /// <summary>
    /// 生成订单号类
    /// </summary>
    public class OrderForm
    {
        /// <summary>
        /// 临时计算用。
        /// </summary>
        private static long np1 = 0, np2 = 0, np3 = 1;

        /// <summary>
        /// 线程并行锁，以保证同一时间点只有一个用户能够操作流水号。
        /// 如果分多个流水号段，放多个锁，并行压力可以更好的解决，大家自己想法子扩充吧
        /// </summary>
        private static object orderFormNumberLock = new object();

        /// <summary>
        /// 初始化订单号码
        /// 编码规则：（16进制，从DateTime.MinValue起到此时的）总天数 + 今天的总秒数 + 当前秒内产生的订单序号，其中今天的订单序号每秒清零。
        /// 该方法线程安全。
        /// </summary>
        public static string CreateOrderNumber()
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now - DateTime.MinValue;
            long tmpDays = span.Days;
            long seconds = span.Hours * 3600 + span.Minutes * 60 + span.Seconds;
            StringBuilder sb = new StringBuilder();
            Monitor.Enter(orderFormNumberLock); //锁定资源
            if (tmpDays != np1)
            {
                np1 = tmpDays;
                np2 = 0;
                np3 = 1;
            }
            if (np2 != seconds)
            {
                np2 = seconds;
                np3 = 1;
            }
            sb.Append(Convert.ToString(np1, 10).PadLeft(6, '0') + Convert.ToString(np2, 10).PadLeft(5, '0') + Convert.ToString(np3++, 10));
            Monitor.Exit(orderFormNumberLock); //释放资源
            return sb.ToString();
        }

        /// <summary>
        /// 获取订单号表示的日期
        /// 即：反向获取订单号的日期
        /// </summary>
        public static DateTime GetDateTimeFromOrderNumber(string orderNumber)
        {
            if (!string.IsNullOrEmpty(orderNumber))
            {
                return DateTime.MinValue.AddDays(Convert.ToInt64(orderNumber.Substring(0, 6), 10)).AddSeconds(Convert.ToInt64(orderNumber.Substring(6, 5), 10));
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <param name="BusinessID">商户ID编号</param>
        /// <returns>结果:订单号</returns>
        public static string GenerateOutTradeNo(string BusinessID)
        {
            var ran = new Random();
            string tradeNo;
            Monitor.Enter(orderFormNumberLock); //锁定资源
            string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            tradeNo = string.Format("{0}{1}{2}", BusinessID, time, ran.Next(999));
            Monitor.Exit(orderFormNumberLock); //释放资源
            return tradeNo;
        }
    }
}
