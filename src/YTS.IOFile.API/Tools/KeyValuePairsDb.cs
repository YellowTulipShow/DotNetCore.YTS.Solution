using System;
using System.Collections.Generic;

using YTS.Log;
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
        private readonly IPathRuleParsingRootConfig pathRuleParsing;
        private readonly IDataFileIO<T> fileIO;

        /// <summary>
        /// 实例化 - 键值对自定义数据源
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="storeConfigs">存储区配置项</param>
        /// <param name="pathRuleParsing">接口: 路径规则解析</param>
        /// <param name="fileIO">接口: 文件数据输入输出</param>
        public KeyValuePairsDb(IDictionary<string, StoreConfiguration> storeConfigs, ILog log, IPathRuleParsingRootConfig pathRuleParsing, IDataFileIO<T> fileIO)
        {
            this.storeConfigs = storeConfigs;
            this.log = log;
            this.pathRuleParsing = pathRuleParsing;
            this.fileIO = fileIO;
        }

        /// <inheritdoc />
        public IEnumerable<OperableStoreShow> GetOperableStores()
        {
            var result = storeConfigs.ToOperableStoreShows();
            var logArgs = log.CreateArgDictionary();
            logArgs["list"] = result;
            log.Info("获取可操作存储区名单!", logArgs);
            return result;
        }

        /// <inheritdoc />
        public int Write(string root, IDictionary<string, T> kvPairs)
        {
            if (kvPairs == null || kvPairs.Count <= 0)
            {
                throw new ArgumentNullException("键值对数据为空");
            }
            var logArgs = log.CreateArgDictionary();
            logArgs[@"root"] = root;
            logArgs[@"kvPairs.Count"] = kvPairs.Count;
            StoreConfiguration config = storeConfigs.ToStoreConfig(root);
            logArgs[@"SystemAbsolutePath"] = config.SystemAbsolutePath;
            int success_count = 0;
            foreach (var key in kvPairs.Keys)
            {
                try
                {
                    logArgs[@"key"] = key;
                    T value = kvPairs[key];
                    logArgs[@"value"] = value;
                    pathRuleParsing.SetRoot(config.SystemAbsolutePath);
                    PathResolutionResult pathResult = pathRuleParsing.ToWrite(key);
                    logArgs[@"AbsolutePathAddress"] = pathResult.AbsolutePathAddress;
                    fileIO.Write(pathResult.AbsolutePathAddress, value);
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
        /// <exception cref="ArgumentNullException">判断传入参数为空异常</exception>
        public IDictionary<string, T> Read(string root, string keyExpression)
        {
            root = root?.Trim();
            keyExpression = keyExpression?.Trim();
            var logArgs = log.CreateArgDictionary();
            logArgs[@"root"] = root;
            logArgs[@"keyExpression"] = keyExpression;
            if (string.IsNullOrEmpty(root) || string.IsNullOrEmpty(keyExpression))
            {
                throw new ArgumentNullException(@"参数为空!");
            }
            StoreConfiguration config = storeConfigs.ToStoreConfig(root);
            logArgs[@"SystemAbsolutePath"] = config.SystemAbsolutePath;
            pathRuleParsing.SetRoot(config.SystemAbsolutePath);
            var pathsResults = pathRuleParsing.ToRead(keyExpression);
            if (pathsResults == null || pathsResults.Count <= 0)
            {
                // throw new ApplicationException(@"无法理解传入的键读取表达式内容或不存在内容");
                return null;
            }
            IDictionary<string, T> result = new Dictionary<string, T>();
            foreach (var pathsResult in pathsResults)
            {
                try
                {
                    logArgs["key"] = pathsResult.Key;
                    string absIOFilePath = pathsResult.AbsolutePathAddress;
                    logArgs["absIOFilePath"] = absIOFilePath;
                    result[pathsResult.Key] = fileIO.Read(absIOFilePath);
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
    public class KeyValuePairsDb : KeyValuePairsDb<object>, IKeyValuePairsDb
    {
        /// <inheritdoc />
        public KeyValuePairsDb(IDictionary<string, StoreConfiguration> storeConfigs, ILog log, IPathRuleParsingRootConfig pathRuleParsing, IDataFileIO fileIO)
            : base(storeConfigs, log, pathRuleParsing, fileIO) { }
    }
}
