using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YTS.IOFile.API.Test
{
    public static class FileExtend
    {
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
