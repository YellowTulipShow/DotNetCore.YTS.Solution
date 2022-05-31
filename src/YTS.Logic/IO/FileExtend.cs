using System.IO;

namespace YTS.Logic.IO
{
    /// <summary>
    /// 文件相关扩展
    /// </summary>
    public static class FileExtend
    {
        /// <summary>
        /// 写入全部文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void WriteAllText(string path, string content)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            File.WriteAllText(file.FullName, content);
        }
    }
}
