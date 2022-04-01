using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using YTS.Logic.Log;
using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.PathRuleParsing;
using YTS.IOFile.API.Tools.DataFileIO;
using YTS.Git;

namespace YTS.IOFile.API.Controllers
{
    /// <summary>
    /// 键值对数据库存储接口
    /// </summary>
    public class KeyValuePairsDbController : BaseApiController
    {
        /// <summary>
        /// 配置键名称_目录哈希队列
        /// </summary>
        public const string CONFIG_KEY_NAME_STORE_CONFIGURATION = "StoreConfiguration";

        private readonly ILog log;
        private readonly IDictionary<string, StoreConfiguration> storeConfigs;
        private readonly IKeyValuePairsDb db;

        /// <summary>
        /// 初始化 - 键值对数据库存储接口
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="configuration"></param>
        public KeyValuePairsDbController(ILogger<KeyValuePairsDbController> _logger, IConfiguration configuration)
        {
            log = new APILogicGeneralLog<KeyValuePairsDbController>(_logger);
            var storeConfigs_Section = configuration.GetSection(CONFIG_KEY_NAME_STORE_CONFIGURATION);
            storeConfigs = storeConfigs_Section.Get<IDictionary<string, StoreConfiguration>>();
            IPathRuleParsing pathRuleParsing = new PathRuleParsingJSON(log);
            IDataFileIO fileIO = new DataFileIOJSON();
            db = new KeyValuePairsDb(storeConfigs, log, pathRuleParsing, fileIO);
        }

        /// <summary>
        /// 获取可操作数据区域根名单
        /// </summary>
        [HttpGet]
        public IEnumerable<dynamic> GetOperableStores() => db.GetOperableStores();

        /// <summary>
        /// 写入键值对数据
        /// </summary>
        /// <param name="root">数据区域</param>
        /// <param name="kvPairs">存储键值对</param>
        /// <returns>执行结果, 受影响的条数</returns>
        [HttpPost]
        public Result<int> Write(string root, IDictionary<string, object> kvPairs)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["root"] = root;
            logArgs["kvPairs"] = kvPairs;
            try
            {
                int successCount = db.Write(root, kvPairs);
                if (successCount <= 0)
                {
                    string name = @"无执行成功记录!";
                    log.Error(name, logArgs);
                    return ResultStatueCode.LogicError.To(name, 0);
                }
                var storeConfig = storeConfigs.ToStoreConfig(root);
                if (storeConfig.Git.IsEnable)
                {
                    IGit git = new GitHelper(storeConfig.Git);
                    git.Add().OnCommand();
                    git.Commit().OnCommand($"WebAPI保存写入数据");
                }
                return ResultStatueCode.OK.To("执行完成!", successCount);
            }
            catch (Exception ex)
            {
                string name = $"保存异常: {ex.Message}";
                log.Error(name, ex, logArgs);
                return ResultStatueCode.UnexpectedException.To(name, 0);
            }
        }

        /// <summary>
        /// 读取键值对数据
        /// </summary>
        /// <param name="root">数据区域</param>
        /// <param name="keyExpression">键读取表达式</param>
        /// <returns>匹配键读取表达式的键值对数据</returns>
        [HttpGet]
        public Result<IDictionary<string, object>> Read(string root, string keyExpression)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["root"] = root;
            logArgs["kvPairs"] = keyExpression;
            try
            {
                var result = db.Read(root, keyExpression);
                if (result == null && result.Count <= 0)
                {
                    string name = @"结果为空!";
                    log.Error(name, logArgs);
                    return ResultStatueCode.LogicError.To(name, result);
                }
                return ResultStatueCode.OK.To("执行完成!", result);
            }
            catch (Exception ex)
            {
                string name = $"保存异常: {ex.Message}";
                log.Error(name, ex, logArgs);
                return ResultStatueCode.UnexpectedException.To(name, (IDictionary<string, object>)null);
            }
        }

        /// <summary>
        /// 拉取最新的线上仓库内容
        /// </summary>
        /// <param name="root">数据区域</param>
        /// <returns>响应输出文本内容</returns>
        [HttpGet]
        public Result<string> PullLatestOrigin(string root)
        {
            var storeConfig = storeConfigs.ToStoreConfig(root);
            if (storeConfig.Git.IsEnable)
            {
                IGit git = new GitHelper(storeConfig.Git);
                var responseMsgs = git.Pull().OnCommand($"拉取数据合并");
                return ResultStatueCode.OK.To("拉取数据执行完成", string.Join(Environment.NewLine, responseMsgs));
            }
            return ResultStatueCode.LogicError.To("存储区未开启Git仓库启用配置", string.Empty);
        }
        /// <summary>
        /// 推送当前
        /// </summary>
        /// <param name="root">数据区域</param>
        /// <returns>响应输出文本内容</returns>
        [HttpGet]
        public Result<string> PushOrigin(string root)
        {
            var storeConfig = storeConfigs.ToStoreConfig(root);
            if (storeConfig.Git.IsEnable)
            {
                IGit git = new GitHelper(storeConfig.Git);
                var responseMsgs = git.Push().OnCommand();
                return ResultStatueCode.OK.To("拉取数据执行完成", string.Join(Environment.NewLine, responseMsgs));
            }
            return ResultStatueCode.LogicError.To("存储区未开启Git仓库启用配置", string.Empty);
        }
    }
}
