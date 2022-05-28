namespace YTS.IOFile.API
{
    /// <summary>
    /// API��̬�����ַ�������
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>
        /// API·��ģ��
        /// </summary>
        public const string APIRoute = "api/[controller]/[action]";
        /// <summary>
        /// ��������
        /// </summary>
        public const string CorsName = "YTSAllowSpecificOrigins";
        /// <summary>
        /// Swagger �ڵ�����
        /// </summary>
        public const string SwaggerEndpointName = "KVDataBase API V1";
        /// <summary>
        /// Swagger �ڵ��ĵ�
        /// </summary>
        public const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";

        /// <summary>
        /// ������������: Swagger��Ϣ����
        /// </summary>
        public const string APPSettingName_SwaggerInfo = "SwaggerInfo";
    }
}
