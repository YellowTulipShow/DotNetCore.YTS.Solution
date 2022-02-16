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
        private readonly ILogger<FileController> _logger;
        private readonly ILog log;
        private readonly IConfiguration configuration;

        public FileController(ILogger<FileController> _logger, IConfiguration configuration)
        {
            this._logger = _logger;
            this.configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<string> GetOperableRootDirectories()
        {
            IList<string> list = new List<string>();
            var dict = configuration.GetSection("DirectoryHash");
            list.Add(dict.Value);
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
