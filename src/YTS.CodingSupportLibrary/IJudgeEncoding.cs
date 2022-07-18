using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YTS.CodingSupportLibrary
{
    /// <summary>
    /// 判断文件编码
    /// </summary>
    internal interface IJudgeEncoding
    {
        /// <summary>
        /// 根据文件判断文件编码内容
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <returns>编码响应结果</returns>
        JudgeEncodingResponse GetEncoding(FileInfo file);

        /// <summary>
        /// 根据字节内容判断文件编码内容
        /// </summary>
        /// <param name="buffer">字节内容</param>
        /// <returns>编码响应结果</returns>
        JudgeEncodingResponse GetEncoding(byte[] buffer);
    }
}
