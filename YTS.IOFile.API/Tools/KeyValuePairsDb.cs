using System;
using System.IO;
using System.Collections.Generic;

using YTS.Logic.Log;
using YTS.IOFile.API.Tools.PathRuleParsing;
using YTS.IOFile.API.Tools.DataFileIO;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 键值对自定义数据源-泛型数据类型
    /// </summary>
    public class KeyValuePairsDb<T> : IKeyValuePairsDb<T>
    {
        private readonly IDictionary<string, StoreConfiguration> storeConfigs;
        private readonly ILog log;
        private readonly IPathRuleParsing pathRuleParsing;
        private readonly IDataFileIO<T> fileIO;

        /// <summary>
        /// 实例化 - 键值对自定义数据源
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="storeConfigs">存储区配置项</param>
        public KeyValuePairsDb(IDictionary<string, StoreConfiguration> storeConfigs, ILog log, IPathRuleParsing pathRuleParsing, IDataFileIO<T> fileIO)
        {
            this.storeConfigs = storeConfigs;
            this.log = log;
            this.pathRuleParsing = pathRuleParsing;
            this.fileIO = fileIO;
        }

        /// <inheritdoc />
        public IEnumerable<dynamic> GetOperableStores()
        {
            IList<dynamic> result = new List<dynamic>();
            foreach (string rootName in storeConfigs.Keys)
            {
                var config = storeConfigs[rootName];
                result.Add(new
                {
                    Name = rootName,
                    config.DescriptionRemarks,
                });
            }

            var logArgs = log.CreateArgDictionary();
            logArgs["list"] = result;
            log.Info("获取可操作存储区名单!", logArgs);
            return result;
        }

        /// <summary>
        /// 转为存储区配置
        /// </summary>
        /// <param name="rootName">存储区名称标识</param>
        /// <returns>配置项 (结果为空会抛出异常)</returns>
        /// <exception cref="ArgumentNullException">未知的存储区名称标识</exception>
        /// <exception cref="NullReferenceException">存储区配置数据为空</exception>
        /// <exception cref="DirectoryNotFoundException">系统绝对路径查找不到或者无权限访问</exception>
        private StoreConfiguration ToStoreConfig(string rootName)
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

        /// <inheritdoc />
        public int Write(string root, IDictionary<string, T> kvPairs)
        {
            if (kvPairs == null || kvPairs.Count <= 0)
            {
                throw new ArgumentNullException("键值对数据为空");
            }
            var logArgs = new Dictionary<string, object>()
            {
                { "root", root },
                { "kvPairs.Count", kvPairs.Count },
            };
            StoreConfiguration config = ToStoreConfig(root);
            int success_count = 0;
            foreach (var key in kvPairs.Keys)
            {
                try
                {
                    logArgs["key"] = key;
                    T value = kvPairs[key];
                    logArgs["value"] = value;
                    string absIOFilePath = pathRuleParsing.ToWriteIOPath(config.SystemAbsolutePath, key);
                    logArgs["absIOFilePath"] = absIOFilePath;
                    fileIO.Write(absIOFilePath, value);
                    log.Info("写入", logArgs);
                    success_count++;
                }
                catch (Exception ex)
                {
                    log.Error("保存某项异常", ex, logArgs);
                }
            }
            return success_count;
        }

        /// <inheritdoc />
        public IDictionary<string, T> Read(string root, string keyExpression)
        {
            root = root?.Trim();
            keyExpression = keyExpression?.Trim();
            var logArgs = new Dictionary<string, object>()
            {
                { "root", root },
                { "keyExpression", keyExpression },
            };
            if (string.IsNullOrEmpty(keyExpression))
            {
                throw new ArgumentNullException("键读取表达式为空");
            }
            StoreConfiguration config = ToStoreConfig(root);
            string root_path = config.SystemAbsolutePath;
            var keyAbsIOFilePaths = pathRuleParsing.ToReadIOPath(root_path, keyExpression);
            if (keyAbsIOFilePaths == null || keyAbsIOFilePaths.Count <= 0)
            {
                throw new ApplicationException("无法理解传入的键读取表达式内容");
            }
            IDictionary<string, T> result = new Dictionary<string, T>();
            foreach (string key in keyAbsIOFilePaths.Keys)
            {
                try
                {
                    logArgs["key"] = key;
                    string absIOFilePath = keyAbsIOFilePaths[key];
                    logArgs["absIOFilePath"] = absIOFilePath;
                    result[key] = fileIO.Read(absIOFilePath);
                    log.Info("读出", logArgs);
                }
                catch (Exception ex)
                {
                    log.Error("读取某项异常", ex, logArgs);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 键值对自定义数据源
    /// </summary>
    public class KeyValuePairsDb : KeyValuePairsDb<object>
    {
        /// <inheritdoc />
        public KeyValuePairsDb(IDictionary<string, StoreConfiguration> storeConfigs, ILog log, IPathRuleParsing pathRuleParsing, IDataFileIO fileIO)
            : base(storeConfigs, log, pathRuleParsing, fileIO) { }
    }
}
