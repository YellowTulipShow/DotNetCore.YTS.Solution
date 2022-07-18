using System.IO;
using System.Text;

namespace YTS.CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: 中文编码类型
    /// </summary>
    internal class Chinese : IJudgeEncoding
    {
        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            byte[] datas = File.ReadAllBytes(file.FullName);
            return GetEncoding(datas);
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] buffer)
        {
            JudgeEncodingResponse response = UseExtend.GetDefaultResponse();
            response.Encoding = JudgeChineseGBK(buffer);
            response.ContentBytes = buffer;
            response.IsReadFileALLContent = true;
            return response;
        }

        private ESupportEncoding? JudgeChineseGBK(byte[] buffer)
        {
            int suspected_ASCII_byte_count = 0;
            int suspected_Chinese_byte_count = 0;
            int bufferIndex = 0;
            while (bufferIndex < buffer.Length)
            {
                byte b = buffer[bufferIndex];
                // 属于 ASCII 单字节内容
                if (b <= ASCII.MAX)
                {
                    suspected_ASCII_byte_count++;
                    bufferIndex++;
                    continue;
                }

                // 判断非 ASCII 编码 并且是最后一个没后续字节
                // 不符合规则: GBK 亦采用双字节表示
                if (bufferIndex == buffer.Length - 1)
                    return null;
                byte b2 = buffer[bufferIndex + 1];

                // 总体编码范围为 8140-FEFE
                // 首字节在 81-FE 之间
                // 尾字节在 40-FE 之间
                // 剔除 xx7F 一条线。
                if (0x81 <= b && b <= 0xFE && 0x40 <= b2 && b2 <= 0xFE)
                {
                    // 符合双字节, 单出现了 xx7F 一条线, 那就一定不是 GBK 编码
                    if (b2 == 0x7F)
                        return null;

                    // 统计中文字符编码
                    suspected_Chinese_byte_count += 2;
                    // 跳过 b2 位置的判断
                    bufferIndex += 2;
                    continue;
                }
                // 否则监测到不符合规则的字节直接退出
                return null;
            }
            // 全部字节 都是 ASCII 编码
            if (buffer.Length == suspected_ASCII_byte_count)
            {
                return ESupportEncoding.ASCII;
            }
            // 总字节数 == ASCII + 中文组合数据 即可判断是 GBK 编码格式
            if (buffer.Length == suspected_ASCII_byte_count + suspected_Chinese_byte_count)
            {
                return ESupportEncoding.GBK;
            }
            // 表示无法识别为 GBK 中文编码
            return null;
        }
    }
}
