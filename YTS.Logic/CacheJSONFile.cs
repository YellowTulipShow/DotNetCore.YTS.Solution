using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace YTS.Logic
{
    /// <summary>
    /// JSON 格式缓存文件
    /// </summary>
    public class CacheJSONFile : ICache
    {
        private readonly string fileName;
        private readonly Encoding encoding;
        private readonly ILog log;

        /// <summary>
        /// 初始化创建 JSON 格式缓存帮助工具
        /// </summary>
        /// <param name="log">执行日志创建</param>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        public CacheJSONFile(ILog log, string fileName, Encoding encoding)
        {
            //fileName = configuration.GetConfigKey("cache-name", "Cache:JSONFileName");
            //encoding = ExplainConfigurationEncoding(configuration);
            this.fileName = fileName;
            this.encoding = encoding;
            this.log = log;
        }

        public string GetData(string key)
        {
            string cacheFilePath = GetCacheFilePath(key);
            log.Info("获取文件缓存", new Dictionary<string, object>()
            {
                { "key", key },
                { "cacheFilePath", cacheFilePath },
            });
            if (!File.Exists(cacheFilePath))
            {
                return string.Empty;
            }
            using FileStream stream = File.OpenRead(cacheFilePath);
            using StreamReader reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }

        public void SetData(string key, string value)
        {
            log.Info("设置文件缓存", new Dictionary<string, object>()
            {
                { "key", key },
            });
            string cacheFilePath = GetCacheFilePath(key);
            File.WriteAllText(cacheFilePath, value, encoding);
        }

        private string GetCacheFilePath(string key)
        {
            string path = $"cache/{fileName}_{key}.json";
            path = ToAbsolutePath(path);
            FileInfo fi = new FileInfo(path);
            var di = fi.Directory;
            if (!di.Exists)
                di.Create();
            return path;
        }
        private static string ToAbsolutePath(string relative)
        {
            relative ??= string.Empty;
            if (Regex.IsMatch(relative, @"^([a-zA-Z]:\\){1}[^\/\:\*\?\""\<\>\|\,]*$"))
                return relative;
            relative = relative.Trim('/');
            if (string.IsNullOrEmpty(relative))
                return relative;
            relative = Regex.Replace(relative, @"/{2,}", @"/");
            relative = relative.Replace(@"/", @"\");
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relative);
        }
    }
}