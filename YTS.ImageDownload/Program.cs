using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            log.Info($"参数解析完成");
            var helper = new MainHelper(log, cache);
            ushort page = 0;
            var imgDownHelper = new ImageDownloadHelper(log, 3);
            while (page < MaxPage)
            {
                page++;
                var logArgs = new Dictionary<string, object>() { { "page", page }, };
                string html = helper.GetOfficialWebsiteHTML(page);
                if (string.IsNullOrEmpty(html))
                {
                    log.Error("官网图片信息HTML获取为空!", logArgs);
                    break;
                }
                var imgs = helper.ParseImageAddress(html);
                if (imgs == null)
                {
                    logArgs["html.Length"] = html.Length;
                    log.Error("图片解析为空!", logArgs);
                    break;
                }
                foreach (var item in imgs)
                {
                    string file_path = ToImageFilePath(path, item);
                    logArgs["file_path"] = file_path;
                    if (File.Exists(file_path))
                    {
                        log.Info("图片已存在, 跳过下载!", logArgs);
                        continue;
                    }
                    logArgs["DownloadImageURL"] = item.DownloadImageURL;
                    var statue = imgDownHelper.Download(item.DownloadImageURL, file_path);
                    logArgs["ResultStatue"] = statue;
                    log.Info("下载图片结果!", logArgs);
                    if (statue == ImageDownloadHelper.ExecuteResultStatue.Complate)
                    {
                        OutPrintImageDescription(log, path, item);
                    }
                }
            }
        }
    }
}
