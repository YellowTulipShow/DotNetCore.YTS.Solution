using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using YTS.Logic.Log;
using YTS.IOFile.API.Tools;
using System.Text;

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
        public const string CONFIG_KEY_NAME_DIRECTORYHASH = "DirectoryHash";

        private readonly ILog log;
        private readonly IDictionary<string, DirectoryHashItem> rootDirectories;
        private readonly PathRuleParsing pathRuleParsing;
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// 初始化 - 键值对数据库存储接口
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="configuration"></param>
        public KeyValuePairsDbController(ILogger<KeyValuePairsDbController> _logger, IConfiguration configuration)
        {
            log = new APILogicGeneralLog<KeyValuePairsDbController>(_logger);
            var rootDirectoriesSection = configuration.GetSection(CONFIG_KEY_NAME_DIRECTORYHASH);
            rootDirectories = rootDirectoriesSection.Get<Dictionary<string, DirectoryHashItem>>();
            pathRuleParsing = new PathRuleParsing();
            serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
        }

        /// <summary>
        /// 可操作目录项配置
        /// </summary>
        public class DirectoryHashItem
        {
            /// <summary>
            /// 磁盘地址
            /// </summary>
            public string Path { get; set; }
        }

        /// <summary>
        /// 获取可操作数据区域根名单
        /// </summary>
        [HttpGet]
        public IEnumerable<string> GetOperableRootDirectories()
        {
            var list = rootDirectories?.Keys.ToArray();
            var logArgs = new Dictionary<string, object>() { { "list", list } };
            log.Info("File.GetOperableRootDirectories execute!", logArgs);
            return list;
        }

        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="root"></param>
        /// <param name="kvPairs"></param>
        /// <returns></returns>
        [HttpPost]
        public Result<int> Write(string root, IDictionary<string, object> kvPairs)
        {
            if (kvPairs == null || kvPairs.Count <= 0)
            {
                return ResultStatueCode.ParameterError
                    .To("键值对为空", 0);
            }
            root = root?.Trim();
            if (!rootDirectories.ContainsKey(root))
            {
                return ResultStatueCode.ParameterError
                    .To("未知的数据存储区", 0);
            }
            string root_path = rootDirectories[root]?.Path?.Trim();
            if (string.IsNullOrEmpty(root_path))
            {
                return ResultStatueCode.ParameterError
                    .To("存储区地址配置为空, 请联系管理员", 0);
            }
            var logArgs = new Dictionary<string, object>()
            {
                { "root", root },
                { "kvPairs.Count", kvPairs.Count },
            };
            int success_count = 0;
            foreach (var key in kvPairs.Keys)
            {
                try
                {
                    logArgs["key"] = key;
                    object value = kvPairs[key];
                    logArgs["value"] = value;
                    string absIOFilePath = pathRuleParsing.ToWriteIOPath(root_path, key);
                    logArgs["absIOFilePath"] = absIOFilePath;
                    string json = JsonConvert.SerializeObject(value, serializerSettings);
                    System.IO.File.WriteAllText(absIOFilePath, json, Encoding.UTF8);
                    success_count++;
                }
                catch (Exception ex)
                {
                    log.Error("保存某项异常", ex, logArgs);
                }
            }
            if (success_count <= 0)
            {
                return ResultStatueCode.LogicError.To("执行出错, 请联系管理员查看日志错误!", 0);
            }
            return ResultStatueCode.OK.To("执行完成!", success_count);
        }

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
            IDictionary<string, string> keyAbsIOFilePath = pathRuleParsing.ToReadIOPath(root, keyExpression);
            if (keyAbsIOFilePath == null || keyAbsIOFilePath.Count <= 0)
            {
                return ResultStatueCode.LogicError
                    .To("无法理解传入的键读取表达式内容", errNull);
            }
            IDictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in keyAbsIOFilePath.Keys)
            {
                string absIOFilePath = keyAbsIOFilePath[key];
                string json = System.IO.File.ReadAllText(absIOFilePath, )
                result[key] = ;
            }
            return ResultStatueCode.OK.To("执行完成!", success_count);
        }
    }
}
