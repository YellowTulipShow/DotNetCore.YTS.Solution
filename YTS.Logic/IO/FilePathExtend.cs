using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace YTS.Logic.IO
{
    public static class FilePathExtend
    {
        /// <summary>
        /// 文件相对路径转绝对路径
        /// </summary>
        /// <param name="relative">相对路径</param>
        /// <returns>绝对路径</returns>
        public static string ToAbsolutePath(string relative)
        {
            relative ??= string.Empty;
            if (Regex.IsMatch(relative, @"^([a-zA-Z]:\\){1}[^\/\:\*\?\""\<\>\|\,]*$"))
                return relative;
            relative = relative.Trim('/');
            if (string.IsNullOrEmpty(relative))
                return relative;
            relative = Regex.Replace(relative, @"/{2,}", @"/");
            relative = relative.Replace(@"/", @"\");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relative);
            FileInfo fi = new FileInfo(path);
            var di = fi.Directory;
            if (!di.Exists)
                di.Create();
            return path;
        }
    }
}
