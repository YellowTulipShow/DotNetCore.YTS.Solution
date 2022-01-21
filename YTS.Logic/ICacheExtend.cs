using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace YTS.Logic
{
    /// <summary>
    /// 接口扩展: 缓存
    /// </summary>
    public static class ICacheExtend
    {
        /// <summary>
        /// 计算缓存键 - 当前时间 - MD5加密
        /// </summary>
        /// <param name="cache">缓存结果</param>
        /// <param name="value">传入参数结果</param>
        /// <returns>缓存键结果</returns>
        public static string CalcCacheKey_NowTimeMD5(this ICache cache, object value)
        {
            StringBuilder str = new StringBuilder();
            str.Append(JsonConvert.SerializeObject(value));
            var nowTime = DateTime.Now;
            str.Append(nowTime.ToString("yyyy-MM-dd"));
            string key = str.ToString();
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(key)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}