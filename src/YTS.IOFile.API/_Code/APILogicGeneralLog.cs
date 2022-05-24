using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.Logging;

using YTS.Log;

namespace YTS.IOFile.API
{
    /// <summary>
    /// API 通用逻辑日志
    /// </summary>
    /// <typeparam name="TCategoryName">类别名称</typeparam>
    public class APILogicGeneralLog<TCategoryName> : ILog
    {
        private readonly ILogger<TCategoryName> _logger;
        private readonly FilePrintLog _fileLog;

        /// <summary>
        /// 初始化 - API 通用逻辑日志
        /// </summary>
        /// <param name="_logger">系统自带日志记录工具</param>
        public APILogicGeneralLog(ILogger<TCategoryName> _logger)
        {
            this._logger = _logger;
            var path = $"./_logs/{typeof(TCategoryName).Name}/{DateTime.Now:yyyy/MM-dd/HH}.log";
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            _fileLog = new FilePrintLog(path, Encoding.UTF8);
        }

        /// <inheritdoc />
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            _logger?.LogError(message, new object[] { args });
            _fileLog?.Error(message, args);
        }

        /// <inheritdoc />
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            _logger?.LogError(ex, message, new object[] { args });
            _fileLog?.Error(message, ex, args);
        }

        /// <inheritdoc />
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            _logger?.LogInformation(message, new object[] { args });
            _fileLog?.Info(message, args);
        }
    }
}
