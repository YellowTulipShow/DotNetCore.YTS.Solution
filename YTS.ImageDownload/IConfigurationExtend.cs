using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

/// <summary>
/// 配置扩展静态类
/// </summary>
public static class IConfigurationExtend
{
    /// <summary>
    /// 编译创建配置接口
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns>配置接口返回</returns>
    public static IConfiguration BuildCreateConfiguration(this string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddCommandLine(args);
        return builder.Build();
    }

    /// <summary>
    /// 获取配置键项
    /// </summary>
    /// <param name="configuration">配置对象</param>
    /// <param name="commandLineKey">命令行参数键名称</param>
    /// <param name="appConfigFileKey">应用程序配置文件参数键名称</param>
    /// <returns></returns>
    public static string GetConfigKey(this IConfiguration configuration, string commandLineKey, string appConfigFileKey)
    {
        string path = configuration[commandLineKey];
        if (string.IsNullOrEmpty(path) && string.IsNullOrWhiteSpace(path))
        {
            path = configuration[appConfigFileKey];
        }
        return path;
    }

    /// <summary>
    /// 解析配置文件编码
    /// </summary>
    /// <param name="configuration">配置接口</param>
    /// <returns>文件编码</returns>
    public static Encoding ExplainConfigurationEncoding(this IConfiguration configuration)
    {
        string encode = configuration.GetConfigKey("file-encode", "File:Encoding");
        return encode switch
        {
            "UTF7" => Encoding.UTF7,
            "UTF32" => Encoding.UTF32,
            "Unicode" => Encoding.Unicode,
            "BigEndianUnicode" => Encoding.BigEndianUnicode,
            "ASCII" => Encoding.ASCII,
            "Default" => Encoding.Default,
            _ => Encoding.UTF8,
        };
    }
}
