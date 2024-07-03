using System.IO;
using System.Text;
using System.Collections.Generic;

using Newtonsoft.Json;

using YTS.Log;

namespace YTS.Command.FileRecognition
{
    public static class PathFileInventoryExtend
    {
        public static void WriteInventoryInfo(this IDictionary<string, PathFileInventory.MInventory> dicts, ILog log, Encoding encoding)
        {
            foreach (var file in dicts.Keys)
            {
                log.Info("输出清单内容", new Dictionary<string, object>()
                {
                    { "file", file },
                });
                File.WriteAllText(file, JsonConvert.SerializeObject(dicts[file]), encoding);
            }
        }
    }
}
