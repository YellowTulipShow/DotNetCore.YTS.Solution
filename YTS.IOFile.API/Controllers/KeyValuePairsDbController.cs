using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using YTS.Logic.Log;
using YTS.IOFile.API.Tools;
using YTS.IOFile.API.Tools.DataSupportIO;

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
        private readonly KeyValuePairsDb db;

        /// <summary>
        /// 初始化 - 键值对数据库存储接口
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="configuration"></param>
        public KeyValuePairsDbController(ILogger<KeyValuePairsDbController> _logger, IConfiguration configuration)
        {
            log = new APILogicGeneralLog<KeyValuePairsDbController>(_logger);
            var storeConfigs_Section = configuration.GetSection(CONFIG_KEY_NAME_STORE_CONFIGURATION);
            var storeConfigs = storeConfigs_Section.Get<Dictionary<string, StoreConfiguration>>();
            db = new KeyValuePairsDb(storeConfigs, log);
        }
        /// <summary>
        /// 获取可操作数据区域根名单
        /// </summary>
        [HttpGet]
        public IEnumerable<dynamic> GetOperableRootDirectories() => db.GetOperablestoreConfigs();

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
                return ResultStatueCode.OK.To("执行完成!", successCount);
            }
            catch (Exception ex)
            {
                log.Error("保存某项异常", ex, logArgs);
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
            var errNull = (IDictionary<string, object>)null;
            root = root?.Trim();
            if (!rootDirectories.ContainsKey(root))
            {
                return ResultStatueCode.ParameterError
                    .To("未知的数据存储区", errNull);
            }
            keyExpression = keyExpression?.Trim();
            if (string.IsNullOrEmpty(keyExpression))
            {
                return ResultStatueCode.ParameterError
                    .To("键读取表达式为空", errNull);
            }
            root = root?.Trim();
            if (!rootDirectories.ContainsKey(root))
            {
                return ResultStatueCode.ParameterError
                    .To("未知的数据存储区", errNull);
            }
            string root_path = rootDirectories[root]?.Path?.Trim();
            if (string.IsNullOrEmpty(root_path))
            {
                return ResultStatueCode.ParameterError
                    .To("存储区地址配置为空, 请联系管理员", errNull);
            }
            IDictionary<string, string> keyAbsIOFilePath = pathRuleParsing.ToReadIOPath(root_path, keyExpression);
            if (keyAbsIOFilePath == null || keyAbsIOFilePath.Count <= 0)
            {
                return ResultStatueCode.LogicError
                    .To("无法理解传入的键读取表达式内容", errNull);
            }
            var logArgs = new Dictionary<string, object>()
            {
                { "root", root },
                { "keyExpression", keyExpression },
            };
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in keyAbsIOFilePath.Keys)
            {
                try
                {
                    logArgs["key"] = key;
                    string absIOFilePath = keyAbsIOFilePath[key];
                    logArgs["absIOFilePath"] = absIOFilePath;
                    string json = System.IO.File.ReadAllText(absIOFilePath, FILE_ENCODING);
                    logArgs["json"] = json;
                    result[key] = JsonConvert.DeserializeObject(json, serializerSettings);
                    log.Info("读出", logArgs);
                }
                catch (Exception ex)
                {
                    log.Error("读取某项异常", ex, logArgs);
                }
            }
            return ResultStatueCode.OK.To("执行完成!", result);
        }
    }
}
