using System;
using System.Collections.Generic;
using System.IO;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 存储区配置数据模型 - 扩展方法
    /// </summary>
    public static class StoreConfigurationExtend
    {
        /// <summary>
        /// 转为存储区配置
        /// </summary>
        /// <param name="storeConfigs">配置集合</param>
        /// <param name="rootName">存储区名称标识</param>
        /// <returns>配置项 (结果为空会抛出异常)</returns>
        /// <exception cref="ArgumentNullException">未知的存储区名称标识</exception>
        /// <exception cref="NullReferenceException">存储区配置数据为空</exception>
        /// <exception cref="DirectoryNotFoundException">系统绝对路径查找不到或者无权限访问</exception>
        public static StoreConfiguration ToStoreConfig(this IDictionary<string, StoreConfiguration> storeConfigs, string rootName)
        {
            rootName = rootName?.Trim();
            if (!storeConfigs.ContainsKey(rootName))
            {
                throw new ArgumentNullException("未知的存储区名称标识");
            }
            var config = storeConfigs[rootName];
            if (config == null)
            {
                throw new NullReferenceException("存储区配置为空!");
            }
            if (string.IsNullOrEmpty(config.SystemAbsolutePath))
            {
                throw new NullReferenceException("存储区配置.系统绝对路径为空!");
            }
            if (!Directory.Exists(config.SystemAbsolutePath))
            {
                throw new DirectoryNotFoundException("存储区配置.系统绝对路径查找不到或者无权限访问!");
            }
            return config;
        }
    }
}
