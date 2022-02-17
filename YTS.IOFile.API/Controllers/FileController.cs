using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using YTS.IOFile.API;
using YTS.Logic.Log;

namespace YTS.IOFile.API.Controllers
{
    /// <summary>
    /// 文件相关操作
    /// </summary>
    public class FileController : BaseApiController
    {
        public const string CONFIG_KEY_NAME_DIRECTORYHASH = "DirectoryHash";

        private readonly ILogger<FileController> _logger;
        private readonly ILog log;
        private readonly IConfiguration configuration;

        public FileController(ILogger<FileController> _logger, IConfiguration configuration)
        {
            this._logger = _logger;
            this.configuration = configuration;
        }

        public class DirectoryHashItem
        {
            public string Path { get; set; }
        }

        private IDictionary<string, DirectoryHashItem> GetDirectoryHashPairs()
        {
            var dict = configuration.GetSection(CONFIG_KEY_NAME_DIRECTORYHASH);
            var hash = dict.Get<Dictionary<string, DirectoryHashItem>>();
            return hash;
        }

        [HttpGet]
        public IEnumerable<string> GetOperableRootDirectories()
        {
            var parirs = GetDirectoryHashPairs();
            IList<string> list = new List<string>();
            foreach (string key in parirs.Keys)
            {
                list.Add(key);
            }
            if (_logger != null)
            {
                _logger.LogInformation("File.GetOperableRootDirectories execute!", list);
            }
            if (log != null)
            {
                log.Info("File.GetOperableRootDirectories execute!", new Dictionary<string, object>()
                {
                    { "list", list }
                });
            }
            return list.ToArray();
        }
    }
}
