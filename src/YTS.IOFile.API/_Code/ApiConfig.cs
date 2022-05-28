namespace YTS.IOFile.API
{
    /// <summary>
    /// API静态配置字符串内容
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>
        /// API路由模板
        /// </summary>
        public const string APIRoute = "api/[controller]/[action]";
        /// <summary>
        /// 跨域名称
        /// </summary>
        public const string CorsName = "YTSAllowSpecificOrigins";
        /// <summary>
        /// Swagger 节点名称
        /// </summary>
        public const string SwaggerEndpointName = "KVDataBase API V1";
        /// <summary>
        /// Swagger 节点文档
        /// </summary>
        public const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";

        /// <summary>
        /// 程序配置名称: Swagger信息名曾
        /// </summary>
        public const string APPSettingName_SwaggerInfo = "SwaggerInfo";
    }
}
