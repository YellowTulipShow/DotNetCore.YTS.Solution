using Newtonsoft.Json;

namespace YTS.WebApi
{
    /// <summary>
    /// Token 管理模块 (从appsetting.json配置文件中读取)
    /// </summary>
    public class TokenManagement
    {
        /// <summary>
        /// 密钥字符串, 用于生成的Token的身份识别标识
        /// </summary>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// 发行人
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [JsonProperty("audience")]
        public string Audience { get; set; }

        /// <summary>
        /// 访问到期(分钟)
        /// </summary>
        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

        /// <summary>
        /// 刷新过期时间
        /// </summary>
        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; }
    }
}
