using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.CommandLine;

using Microsoft.Extensions.Configuration;

using YTS.Logic.Internet;
using YTS.Logic.IO;
using YTS.Logic.Log;

namespace YTS.ImageDownload
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ILog log = new ConsolePrintLog();
            IConfiguration configuration = args.BuildCreateConfiguration();
            var logArgs = new Dictionary<string, object>();
            Encoding encoding = configuration.ExplainConfigurationEncoding();
            string urls_path = configuration.GetConfigKey("urls-path", "urls-path");
            urls_path = FilePathExtend.ToAbsolutePath(urls_path);
            logArgs["--urls-path"] = urls_path;
            if (string.IsNullOrEmpty(urls_path))
            {
                log.Error("请输入参数: --urls-path=\"XXX\" 下载图片列表存储文件路径!", logArgs);
                return;
            }
            string is_override_argString = configuration.GetConfigKey("is-override", "is-override");
            if (!bool.TryParse(is_override_argString, out bool is_override))
            {
                logArgs["--is-override"] = is_override_argString;
                is_override = false;
                log.Info("参数 --is-override=\"{True|False}\" 无法取到正常值, 已默认为 False 执行后续逻辑!", logArgs);
            }
            logArgs["--is-override"] = is_override;
            log.Info($"参数解析完成", logArgs);
            if (!File.Exists(urls_path))
            {
                log.Error("下载图片列表存储文件不存在!", logArgs);
                return;
            }
            var imgDownHelper = new ImageDownloadHelper(log, 3);
            string[] lines = File.ReadAllLines(urls_path, encoding);
            lines = lines.Distinct().ToArray();
            Regex imgUrlRegex = new Regex(@"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.[a-zA-Z]{2,4})(\:[0-9]+)?(/[^/][a-zA-Z0-9\.\,\?\'\\/\+&%\$#\=~_\-@]*)*$");
            IList<Task> tasks = new List<Task>();
            foreach (string line in lines)
            {
                if (!imgUrlRegex.IsMatch(line))
                {
                    continue;
                }
                string imageUrl = line;
                var match = imgUrlRegex.Match(line);
                string imageSavePath = match.Groups[2].Value;
                var task = imgDownHelper.DownloadAsync(imageUrl, imageSavePath);
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}
