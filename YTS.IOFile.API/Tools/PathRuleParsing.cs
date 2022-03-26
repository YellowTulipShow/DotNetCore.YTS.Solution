﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YTS.IOFile.API.Tools
{
    /// <summary>
    /// 路径解析
    /// </summary>
    public class PathRuleParsing
    {
        /// <summary>
        /// 转为写入的路径地址
        /// </summary>
        /// <param name="root">根目录地址</param>
        /// <param name="key">键</param>
        /// <returns>绝对地址路径</returns>
        public string ToWriteIOPath(string root, string key)
        {
            root = FilterHazardousContent(root);
            key = FilterHazardousContent(key);
            return null;
        }

        /// <summary>
        /// 转为读取的路径地址队列
        /// </summary>
        /// <param name="root">根目录地址</param>
        /// <param name="keyExpression">键匹配表达式</param>
        /// <returns>绝对地址路径队列(键,地址)</returns>
        public IDictionary<string, string> ToReadIOPath(string root, string keyExpression)
        {
            root = FilterHazardousContent(root);
            keyExpression = FilterHazardousContent(keyExpression);
            return null;
        }

        /// <summary>
        /// 过滤危险内容
        /// </summary>
        private string FilterHazardousContent(string content) {
            content = content?.Trim();
            if (string.IsNullOrEmpty(content))
                return content;
            content = Regex.Replace(content, @"\.+", "");
            content = Regex.Replace(content, @"\/+", "");
            content = Regex.Replace(content, @"\\+", "");
            return content;
        }
    }
}
