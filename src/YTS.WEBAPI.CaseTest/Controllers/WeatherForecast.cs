using System;

namespace YTS.WEBAPI.CaseTest.Controllers
{
    /// <summary>
    /// 天气预报数据
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// 温度C
        /// </summary>
        public int TemperatureC { get; internal set; }
        /// <summary>
        /// 日期时间
        /// </summary>
        public DateTime Date { get; internal set; }
        /// <summary>
        /// 总结
        /// </summary>
        public string Summary { get; internal set; }
    }
}