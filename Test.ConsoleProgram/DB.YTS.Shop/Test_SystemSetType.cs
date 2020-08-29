using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YTS.Shop;
using YTS.Shop.Tools;
using YTS.Tools;

namespace Test.ConsoleProgram.DB.YTS.Shop
{
    public class Test_SystemSetType : CaseModel
    {
        public Test_SystemSetType()
        {
            NameSign = "测试系统字典参数获取";
            ExeEvent = OnMethod;
        }

        public bool OnMethod()
        {
            var db = DBContextInstance.YTSEntityContext;
            var OperateTypeName = db.SystemSetType.QueryEnumValue<KeysType.UserMoneyRecordOperateType>(1);
            Console.WriteLine("OperateTypeName: ({0})", OperateTypeName);
            return OperateTypeName == "充值";
        }
    }
}
