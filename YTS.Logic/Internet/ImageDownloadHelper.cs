using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using YTS.Logic.Log;

namespace YTS.Logic.Internet
{
    /// <summary>
    /// 图片下载帮助类
    /// </summary>
    public class ImageDownloadHelper
    {
        private readonly ILog log;
        private readonly ushort maxReSize;

        /// <summary>
        /// 初始化图片下载帮助类, 定义最大重复下载次数设置
        /// </summary>
        /// <param name="log">执行日志接口</param>
        /// <param name="maxReSize">最大重复下载次数设置</param>
        public ImageDownloadHelper(ILog log, ushort maxReSize = 3)
        {
            this.log = log;
            this.maxReSize = maxReSize;
        }

        /// <summary>
        /// 执行结果状态
        /// </summary>
        public enum ExecuteResultStatue
        {
            /// <summary>
            /// 执行完成
            /// </summary>
            Complate = 0,
            /// <summary>
            /// 重复下载N次依然失败
            /// </summary>
            ReSizeError = 1,
            /// <summary>
            /// 网络资源不存在
            /// </summary>
            NotFound404 = 2,
            /// <summary>
            /// 参数为空
            /// </summary>
            ArgumentNull = 3,
        }

        /// <summary>
        /// 下载图片执行
        /// </summary>
        /// <param name="imageUrl">图片下载地址</param>
        /// <param name="imageSavePath">图片保存地址</param>
        /// <returns>执行结果状态</returns>
        public ExecuteResultStatue Download(string imageUrl, string imageSavePath)
        {
            var task = DownloadAsync(imageUrl, imageSavePath);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 异步下载图片执行
        /// </summary>
        /// <param name="imageUrl">图片下载地址</param>
        /// <param name="imageSavePath">图片保存地址</param>
        /// <returns>执行结果状态</returns>
        public async Task<ExecuteResultStatue> DownloadAsync(string imageUrl, string imageSavePath)
        {
            return await DownloadAsync(imageUrl, imageSavePath, 0);
        }

        private async Task<ExecuteResultStatue> DownloadAsync(string imageUrl, string imageSavePath, ushort reSize)
        {
            var logArgs = new Dictionary<string, object>()
            {
                { "imageUrl", imageUrl },
                { "imageSavePath", imageSavePath },
            };
            if (string.IsNullOrEmpty(imageUrl))
            {
                log.Error("图片下载地址为空!", logArgs);
                return ExecuteResultStatue.ArgumentNull;
            }
            if (string.IsNullOrEmpty(imageSavePath))
            {
                log.Error("图片保存地址为空!", logArgs);
                return ExecuteResultStatue.ArgumentNull;
            }
            try
            {
                using WebClient client = new WebClient();
                await client.DownloadFileTaskAsync(imageUrl, imageSavePath);
                log.Info("下载图片完成!", logArgs);
                return ExecuteResultStatue.Complate;
            }
            catch (Exception ex)
            {
                if (ex is WebException webException)
                {
                    var resp = (HttpWebResponse)webException.Response;
                    if (resp?.StatusCode == HttpStatusCode.NotFound)
                    {
                        log.Error("下载图片404不存在!", logArgs);
                        if (File.Exists(imageSavePath))
                            File.Delete(imageSavePath);
                        return ExecuteResultStatue.NotFound404;
                    }
                }
                reSize++;
                if (reSize >= maxReSize)
                {
                    log.Error($"下载图片文件重复{reSize}次下载失败!", ex, logArgs);
                    if (File.Exists(imageSavePath))
                        File.Delete(imageSavePath);
                    return ExecuteResultStatue.ReSizeError;
                }
                log.Error($"文件重复{reSize}次下载失败!", ex, logArgs);
                return await DownloadAsync(imageUrl, imageSavePath, reSize);
            }
        }
    }
}
