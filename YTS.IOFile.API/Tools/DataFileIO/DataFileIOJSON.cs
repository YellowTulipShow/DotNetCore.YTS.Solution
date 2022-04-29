using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace YTS.IOFile.API.Tools.DataFileIO
{
    /// <summary>
    /// 数据文件IO - JSON类型实现 - 泛型类型
    /// </summary>
    public class DataFileIOJSON<T> : IDataFileIO<T>
    {
        private static readonly Encoding FILE_ENCODING = Encoding.UTF8;
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// 实例化 - 数据文件IO - JSON类型实现
        /// </summary>
        public DataFileIOJSON()
        {
            serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
        }

        /// <inheritdoc />
        public T Read(string fileAbsPath)
        {
            if (!File.Exists(fileAbsPath))
                return default;
            string json = File.ReadAllText(fileAbsPath, FILE_ENCODING);
            return JsonConvert.DeserializeObject<T>(json, serializerSettings);
        }

        /// <inheritdoc />
        public void Write(string fileAbsPath, T data)
        {
            string json = JsonConvert.SerializeObject(data, serializerSettings);
            File.WriteAllText(fileAbsPath, json, FILE_ENCODING);
        }
    }

    /// <summary>
    /// 数据文件IO - JSON类型实现 - object 类型
    /// </summary>
    public class DataFileIOJSON : DataFileIOJSON<object>, IDataFileIO
    {
        /// <inheritdoc />
        public DataFileIOJSON() : base() { }
    }
}
