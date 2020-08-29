using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YTS.Shop;
using YTS.Tools;

namespace Test.ConsoleProgram.DB.YTS.Shop
{
    public static class DBContextInstance
    {
        private static YTSEntityContext _YTSEntityContext = null;
        public static YTSEntityContext YTSEntityContext
        {
            get
            {
                if (_YTSEntityContext == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<YTSEntityContext>();
                    optionsBuilder.UseSqlite("Data Source=D:\\wwwroot\\DataBaseSource\\YTSEntityDB.db");
                    _YTSEntityContext = new YTSEntityContext(optionsBuilder.Options);
                }
                return _YTSEntityContext;
            }
        }
    }
}
