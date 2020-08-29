using System;
using System.IO;
using YTS.Tools;

namespace Test.ConsoleProgram.Base
{
    public class Test_FilePath : CaseModel
    {
        public Test_FilePath()
        {
            this.NameSign = "测试日志";
            this.ExeEvent = Method;
        }

        public void IsError(bool isSuccess)
        {
            if (!isSuccess)
                throw new Exception("两个路径不想等!");
        }
        public bool Method()
        {
            const string ThumbnailImgKey = "_thumbnailImg";
            const string sourceImgUrl = "http://localhost:60793/upload/zhaoruiqing/image/20200403/6372150821320114829587451.jpg";
            Uri sourceImgUri = new Uri(sourceImgUrl);
            string sourceImgLocalPath = sourceImgUri.LocalPath;

            Assert.IsEqual(
                PathHelp.ToAbsolute("/upload/zhaoruiqing/image/20200403/6372150821320114829587451_thumbnailImg.jpg"),
                CreateThumImgPath(sourceImgLocalPath, ThumbnailImgKey)
            );

            const string ThumImgSaveDirectory = @"D:\wwwroot\ErTuiShengShi.ShopWebApi";
            Assert.IsEqual(
                @"D:\wwwroot\ErTuiShengShi.ShopWebApi\upload\zhaoruiqing\image\20200403\6372150821320114829587451_thumbnailImg.jpg",
                CreateThumImgPath(sourceImgLocalPath, ThumbnailImgKey, ThumImgSaveDirectory)
            );

            const string thumbnailImgUrl = "http://localhost:60793/upload/zhaoruiqing/image/20200403/6372150821320114829587451_thumbnailImg.jpg";
            string thumbnailImgLocalPath = new Uri(thumbnailImgUrl).LocalPath;
            Assert.IsEqual(IsSourceImgThumbnail(
                sourceImgLocalPath,
                thumbnailImgLocalPath,
                ThumbnailImgKey
            ), true);

            return true;
        }

        public bool IsSourceImgThumbnail(string sourceImgPath, string thumbnailImgPath, string ThumbnailImgKey)
        {
            string source_new_name = Path.GetFileNameWithoutExtension(sourceImgPath) + ThumbnailImgKey;
            string thumbnail_name = Path.GetFileNameWithoutExtension(thumbnailImgPath);
            return source_new_name == thumbnail_name;
        }

        public string CreateThumImgPath(string sourceImgPath, string ThumbnailImgKey, string ThumImgSaveDirectory = null)
        {
            string dirName = Path.GetDirectoryName(sourceImgPath);
            string fileName = Path.GetFileNameWithoutExtension(sourceImgPath) + ThumbnailImgKey;
            string extensionName = Path.GetExtension(sourceImgPath);
            string thumbnailPath = string.Join("\\", new string[] { dirName, fileName + extensionName });
            thumbnailPath = (thumbnailPath ?? "").Replace('/', '\\').Trim('\\');
            string directory = (ThumImgSaveDirectory ?? "").Replace('/', '\\').Trim('\\');
            if (string.IsNullOrWhiteSpace(directory))
            {
                return PathHelp.ToAbsolute(thumbnailPath);
            }
            return directory + "\\" + thumbnailPath;
        }
    }
}
