using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YTS.CodingSupportLibrary
{
    /// <summary>
    /// 静态调用扩展方法
    /// </summary>
    public static class UseExtend
    {
        /// <summary>
        /// 注册支持代码页
        /// </summary>
        public static void SupportCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 判断得到文件的编码内容
        /// </summary>
        /// <param name="file">文件内容</param>
        /// <returns>编码结果, 如果不受支持无法找到则返回 NULL</returns>
        public static Encoding GetEncoding(this FileInfo file)
        {
            JudgeEncodingResponse response = file.GetJudgeEncodingResponse();
            return response.Encoding?.ToEncoding();
        }

        /// <summary>
        /// 判断得到文件的编码内容判断结果响应
        /// </summary>
        /// <param name="file">文件内容</param>
        /// <returns>编码内容判断结果响应</returns>
        public static JudgeEncodingResponse GetJudgeEncodingResponse(this FileInfo file)
        {
            JudgeEncodingResponse response = GetDefaultResponse();
            byte[] contentBytes = null;
            foreach (var item in GetJudgeEncodings())
            {
                bool isNullContentBytes = contentBytes == null || contentBytes.Length <= 0;
                response = isNullContentBytes ?
                    item.GetEncoding(file) :
                    item.GetEncoding(contentBytes);
                if (response.Encoding != null)
                    return response;
                if (response.IsReadFileALLContent)
                    contentBytes = response.ContentBytes;
            }
            return response;
        }

        /// <summary>
        /// 获取默认响应
        /// </summary>
        /// <returns>默认响应</returns>
        internal static JudgeEncodingResponse GetDefaultResponse()
        {
            return new JudgeEncodingResponse()
            {
                IsReadFileALLContent = false,
                ContentBytes = null,
                Encoding = null,
            };
        }

        private static IEnumerable<IJudgeEncoding> GetJudgeEncodings()
        {
            yield return new JudgeEncoding.UnicodeHeader();
            yield return new JudgeEncoding.ASCII();
            yield return new JudgeEncoding.UTF8NoBOM();
            yield return new JudgeEncoding.Chinese();
        }

        /// <summary>
        /// 疑似受支持编码空枚举转为编码抽象类
        /// </summary>
        /// <param name="enum_support_encoding">疑似受支持编码空枚举</param>
        /// <returns>编码抽象类</returns>
        public static Encoding ToEncoding(this ESupportEncoding? enum_support_encoding)
        {
            if (enum_support_encoding == null)
                throw new ArgumentNullException(nameof(enum_support_encoding), $"编码配置枚举值为空, 无法解析!");
            return enum_support_encoding?.ToEncoding();
        }

        /// <summary>
        /// 受支持编码枚举转为编码抽象类
        /// </summary>
        /// <param name="enum_support_encoding">受支持编码枚举</param>
        /// <returns>编码抽象类</returns>
        public static Encoding ToEncoding(this ESupportEncoding enum_support_encoding)
        {
            switch (enum_support_encoding)
            {
                case ESupportEncoding.ASCII: return Encoding.ASCII;
                case ESupportEncoding.UTF32_LittleEndian: return Encoding.UTF32;
                case ESupportEncoding.UTF16_LittleEndian: return Encoding.Unicode;
                case ESupportEncoding.UTF16_BigEndian: return Encoding.BigEndianUnicode;
                case ESupportEncoding.UTF8: return new UTF8Encoding(true);
                case ESupportEncoding.UTF8_NoBOM: return new UTF8Encoding(false);
                case ESupportEncoding.GBK: return Encoding.GetEncoding("GBK");
                default:
                    throw new ArgumentOutOfRangeException(nameof(enum_support_encoding), $"受支持的编码配置, 无法解析: {enum_support_encoding}");
            }
        }
    }
}
