using Newtonsoft.Json;

namespace YTS.IOFile.API
{
    /// <summary>
    /// Swagger 配置信息 (从appsetting.json配置文件中读取)
    /// </summary>
    public class SwaggerInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("Version")]
        public string Version { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [JsonProperty("Contact")]
        public MContact Contact { get; set; }

        /// <summary>
        /// 开源许可证信息
        /// </summary>
        [JsonProperty("License")]
        public MLicense License { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public class MContact : MLicense
        {
            /// <summary>
            /// 邮箱
            /// </summary>
            [JsonProperty("Email")]
            public string Email { get; set; }
        }

        /// <summary>
        /// 开源许可证
        /// </summary>
        public class MLicense
        {
            /// <summary>
            /// 名称
            /// </summary>
            [JsonProperty("Name")]
            public string Name { get; set; }

            /// <summary>
            /// 地址
            /// </summary>
            [JsonProperty("Url")]
            public string Url { get; set; }
        }
    }
}
