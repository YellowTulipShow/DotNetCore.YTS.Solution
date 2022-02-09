using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Logic.Log;

using static YTS.Logic.IO.PathFileInventory;

namespace YTS.Logic.IO
{
    public static class PathFileInventoryExtend
    {
        public static void WriteInventoryInfo(this IDictionary<string, MInventory> dicts, ILog log, Encoding encoding)
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
