using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.Logging;

using YTS.Log;

namespace YTS.WEBAPI
{
    /// <summary>
    /// API 通用逻辑日志
    /// </summary>
    /// <typeparam name="TCategoryName">类别名称</typeparam>
    public class APILogicGeneralLog<TCategoryName> : ILog
    {
        private readonly ILogger<TCategoryName> _logger;
        private readonly ILog _fileLog;

        /// <summary>
        /// 初始化 - API 通用逻辑日志
        /// </summary>
        /// <param name="_logger">系统自带日志记录工具</param>
        public APILogicGeneralLog(ILogger<TCategoryName> _logger)
        {
            this._logger = _logger;
            _fileLog = GetFilePrintLogCase();
        }

        /// <summary>
        /// 获取文件打印日志示例
        /// </summary>
        protected virtual ILog GetFilePrintLogCase()
        {
            var path = $"./_logs/{typeof(TCategoryName).Name}/{DateTime.Now:yyyy/MM-dd/HH}.log";
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            return new FilePrintLog(new FileInfo(path), Encoding.UTF8);
        }

        /// <inheritdoc />
        public void Error(string message, params IDictionary<string, object>[] args)
        {
            try { _logger?.LogError(message, new object[] { args }); } catch (Exception) { }
            try { _fileLog?.Error(message, args); } catch (Exception) { }
        }

        /// <inheritdoc />
        public void Error(string message, Exception ex, params IDictionary<string, object>[] args)
        {
            try { _logger?.LogError(ex, message, new object[] { args }); } catch (Exception) { }
            try { _fileLog?.Error(message, ex, args); } catch (Exception) { }
        }

        /// <inheritdoc />
        public void Info(string message, params IDictionary<string, object>[] args)
        {
            try { _logger?.LogInformation(message, new object[] { args }); } catch (Exception) { }
            try { _fileLog?.Info(message, args); } catch (Exception) { }
        }
    }
}
