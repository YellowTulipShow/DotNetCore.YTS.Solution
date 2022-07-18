using System;
using System.Collections.Generic;
using System.IO;

namespace YTS.CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: Unicode 头部判断 类型
    /// </summary>
    internal class UnicodeHeader : IJudgeEncoding
    {
        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] buffer = br.ReadBytes(4);
                    JudgeEncodingResponse response = UseExtend.GetDefaultResponse();
                    response.Encoding = JudgeHeader(buffer);
                    return response;
                }
            }
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] buffer)
        {
            JudgeEncodingResponse response = UseExtend.GetDefaultResponse();
            response.Encoding = JudgeHeader(buffer);
            response.ContentBytes = buffer;
            response.IsReadFileALLContent = true;
            return response;
        }

        private static IEnumerable<(byte[] bom, Func<ESupportEncoding> getFunc)> HeaderRule()
        {
            // UTF-32 格式
            yield return (new byte[] { 0xFF, 0xFE, 0x00, 0x00 }, () => ESupportEncoding.UTF32_LittleEndian);
            // UTF-8 BOM 格式头部必带标识
            yield return (new byte[] { 0xEF, 0xBB, 0xBF }, () => ESupportEncoding.UTF8);
            // UTF-16 格式 标识
            yield return (new byte[] { 0xFE, 0xFF }, () => ESupportEncoding.UTF16_BigEndian);
            yield return (new byte[] { 0xFF, 0xFE }, () => ESupportEncoding.UTF16_LittleEndian);
        }

        private ESupportEncoding? JudgeHeader(byte[] buffer)
        {
            if (buffer == null || buffer.Length <= 0)
            {
                return null;
            }
            // 判断头部字符
            foreach (var (bom, getFunc) in HeaderRule())
            {
                if (buffer.Length < bom.Length)
                    continue;
                bool is_all_equal = true;
                for (int i = 0; i < bom.Length; i++)
                {
                    if (buffer[i] != bom[i])
                    {
                        is_all_equal = false;
                        break;
                    }
                }
                if (is_all_equal)
                {
                    return getFunc();
                }
            }
            return null;
        }
    }
}
