namespace YTS.CodingSupportLibrary
{
    /// <summary>
    /// 枚举: 支持的编码队列
    /// </summary>
    public enum ESupportEncoding
    {
        /// <summary>
        /// ASCII 基础的英文编码, 任何编码都支持, 如果判断结果为此, 那么说明此文件认可为任何编码文件都可以
        /// </summary>
        ASCII,
        /// <summary>
        /// 使用 Unicode 字符编码表, 固定使用4个字节存储数据的编码方式, Little Endian 排序方式
        /// </summary>
        UTF32_LittleEndian,
        /// <summary>
        /// 使用 Unicode 字符编码表, 固定使用2个字节存储数据的编码方式, Little Endian 排序方式
        /// </summary>
        UTF16_LittleEndian,
        /// <summary>
        /// 使用 Unicode 字符编码表, 固定使用4个字节存储数据的编码方式, Big Endian 排序方式
        /// </summary>
        UTF16_BigEndian,
        /// <summary>
        /// .Net 微软研发独特的文件表头增加 三位BOM 可直接表示此文件为 UTF8 方式编码
        /// </summary>
        UTF8,
        /// <summary>
        /// 最通用的编码规则: 使用 Unicode 字符编码表, 动态使用1~4个字节存储数据的编码方式
        /// </summary>
        UTF8_NoBOM,
        /// <summary>
        /// 中文国标的大编码集合, 向下兼容 GB2312 等编码
        /// </summary>
        GBK,
    }
}
