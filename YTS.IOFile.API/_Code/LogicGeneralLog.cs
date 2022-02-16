using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YTS.Logic.Log;

namespace YTS.IOFile.API
{
    public class LogicGeneralLog : FilePrintLog
    {
        public LogicGeneralLog() : base("./_logs/file.log", Encoding.UTF8)
        {
        }

        protected override void PrintLines(params string[] msglist)
        {
            base.PrintLines(msglist);
        }
    }
}
