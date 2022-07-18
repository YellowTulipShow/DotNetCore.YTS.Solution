using System.IO;
using System.Text;

namespace YTS.CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: UTF8 无BOM 编码
    /// </summary>
    internal class UTF8NoBOM : IJudgeEncoding
    {
        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            byte[] buffer = File.ReadAllBytes(file.FullName);
            return GetEncoding(buffer);
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] buffer)
        {
            JudgeEncodingResponse response = UseExtend.GetDefaultResponse();
            response.Encoding = JudgeUTF8NoBOM(buffer);
            response.ContentBytes = buffer;
            response.IsReadFileALLContent = true;
            return response;
        }

        private ESupportEncoding? JudgeUTF8NoBOM(byte[] buffer)
        {
            const uint rank_item_left = 0b_1000_0000;
            const uint rank_item_right = 0b_1100_0000;
            uint[] ranks = {
                0b_1100_0000,
                0b_1110_0000,
                0b_1111_0000,
                0b_1111_1000,
                0b_1111_1100,
                0b_1111_1110,
            };
            // 获取后续字节数量
            int get_follow_byte_count(byte b)
            {
                for (int rankIndex = 1; rankIndex < ranks.Length; rankIndex++)
                {
                    uint l = ranks[rankIndex - 1];
                    uint r = ranks[rankIndex];
                    // 在指定的范围内
                    if (l <= b && b < r)
                    {
                        return rankIndex;
                    }
                }
                return -1;
            };

            int suspected_ASCII_byte_count = 0;
            int suspected_UTF8_byte_count = 0;
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
                int follow_byte_count = get_follow_byte_count(b);
                // 不符合 UTF8 编码规则: 第一个字节表示: 字节总数规则
                if (follow_byte_count <= 0)
                    return null;
                suspected_UTF8_byte_count++;
                while (follow_byte_count > 0)
                {
                    bufferIndex++;
                    // 超出范围也判定无法识别规则
                    if (bufferIndex >= buffer.Length)
                        return null;
                    b = buffer[bufferIndex];
                    if (rank_item_left <= b && b < rank_item_right)
                    {
                        suspected_UTF8_byte_count++;
                        follow_byte_count--;
                        continue;
                    }
                    // 不符合 UTF8 编码规则: 后续字节符合规范 10xx xxxx
                    return null;
                }
                bufferIndex++;
            }
            // 全部字节 都是 ASCII 编码
            if (buffer.Length == suspected_ASCII_byte_count)
            {
                return ESupportEncoding.ASCII;
            }
            // 总字节数 == ASCII单字节数 + UTF8组合字节组数 即可判断是 UTF8 无BOM 编码格式
            if (buffer.Length == suspected_ASCII_byte_count + suspected_UTF8_byte_count)
            {
                return ESupportEncoding.UTF8_NoBOM;
            }
            // 表示无法识别为 UTF8 无BOM 编码
            return null;
        }
    }
}
