using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace YTS.Logic.IO
{
    /// <summary>
    /// 静态扩展: 文件路径相关
    /// </summary>
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

public static string GetRelativePath(string currentDirectory, string targetFilePath)
    {
        // 标准化路径：替换反斜杠为斜杠，移除多余斜杠，移除尾部斜杠
        string NormalizePath(string path) =>
            string.IsNullOrEmpty(path) ? "" : path.Replace('\\', '/').Replace("//", "/").TrimEnd('/');

        currentDirectory = NormalizePath(currentDirectory);
        targetFilePath = NormalizePath(targetFilePath);

        // 如果路径完全相同，返回 "./"
        if (currentDirectory == targetFilePath)
        {
            return "./";
        }

        // 如果当前路径为空，直接返回绝对目标路径
        if (string.IsNullOrEmpty(currentDirectory))
        {
            return "/" + targetFilePath;
        }

        // 如果目标路径为空，返回 "../"
        if (string.IsNullOrEmpty(targetFilePath))
        {
            return "../";
        }

        // 分割路径为片段
        string[] currentParts = currentDirectory.Split('/');
        string[] targetParts = targetFilePath.Split('/');

        // 找到最长公共前缀
        int commonLength = 0;
        while (commonLength < currentParts.Length && commonLength < targetParts.Length &&
               string.Equals(currentParts[commonLength], targetParts[commonLength], StringComparison.OrdinalIgnoreCase))
        {
            commonLength++;
        }

        // 如果没有公共前缀，直接返回目标路径
        if (commonLength == 0 || (commonLength == 1 && currentParts[0] == ""))
        {
            return "/" + targetFilePath;
        }

        // 计算 ".." 的数量
        int backSteps = currentParts.Length - commonLength;

        // 构造返回路径
        string backPath = string.Join("/", Enumerable.Repeat("..", backSteps));
        string forwardPath = string.Join("/", targetParts.Skip(commonLength));

        // 如果没有 ".."，需要加 "./" 前缀
        if (string.IsNullOrEmpty(backPath))
        {
            return "./" + forwardPath;
        }

        // 如果没有 forwardPath，直接返回 "../"
        if (string.IsNullOrEmpty(forwardPath))
        {
            return backPath + "/";
        }

        return backPath + "/" + forwardPath;
    }



    }
}
