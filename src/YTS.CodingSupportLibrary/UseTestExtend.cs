using System;
using System.IO;
using System.Text;

namespace YTS.CodingSupportLibrary
{
    /// <summary>
    /// 静态调用扩展测试方法
    /// </summary>
    public static class UseTestExtend
    {
        private const char space = ' ';
        private const int single_line_byte_number = 20;
        private const int single_byte_length = 3;
        public static string GetWriteLogFileContentBytesContent(this FileInfo codeFile)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine($"文件: {codeFile.FullName}");
            byte[] datas = File.ReadAllBytes(codeFile.FullName);
            str.AppendLine($"名称: {codeFile.Name}");
            str.AppendLine($"\nHEX 十六进制:");
            str.Append(GetWriteLogFileContentBytesContent_StringBuilder(datas, b => b.ToString("X2")));
            str.AppendLine($"\nOCT 十进制:");
            str.Append(GetWriteLogFileContentBytesContent_StringBuilder(datas, b => b.ToString()));
            str.Append("\n");
            return str.ToString();
        }
        private static StringBuilder GetWriteLogFileContentBytesContent_StringBuilder(byte[] datas, Func<byte, string> byteToStrFunc)
        {
            StringBuilder str = new StringBuilder();
            int header_len = (datas.Length + single_line_byte_number).ToString().Length;
            string get_line_header(int line)
            {
                return $"  {line.ToString().PadLeft(header_len, space)}:  ";
            };
            str.Append($"{"".PadLeft(get_line_header(0).Length)}");
            string item_space = "".PadLeft(2, space);
            for (int i = 0; i < single_line_byte_number; i++)
            {
                string end_join = i == single_line_byte_number - 1 ? "" : item_space;
                str.Append($"{i,single_byte_length}{end_join}");
            }
            for (int i = 0; i < datas.Length; i++)
            {
                if (i % single_line_byte_number == 0)
                {
                    int line = (int)Math.Ceiling((decimal)(i / single_line_byte_number)) * single_line_byte_number;
                    str.Append($"\n{get_line_header(line)}");
                }
                string end_join = (i + 1) % single_line_byte_number == 0 ? "" : item_space;
                str.Append($"{byteToStrFunc(datas[i]),single_byte_length}{end_join}");
            }
            str.AppendLine(string.Empty);
            return str;
        }

        public static string ReadFileBytesToX2FormatString(this FileInfo file)
        {
            byte[] buffer = File.ReadAllBytes(file.FullName);
            return buffer.ToX2FormatString();
        }
        public static string ToX2FormatString(this byte[] buffer)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                byte b = buffer[i];
                string sign = ".";
                if (i == buffer.Length - 1)
                    sign = "";
                str.Append($"{b:X2}{sign}");
            }
            return str.ToString();
        }
    }
}
