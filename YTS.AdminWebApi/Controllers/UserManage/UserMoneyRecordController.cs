using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using YTS.Tools;
using YTS.WebApi;
using YTS.Shop;
using YTS.Shop.Models;
using YTS.Shop.Tools;

namespace YTS.AdminWebApi.Controllers
{
    /// <summary>
    /// 用户金额变动记录
    /// </summary>
    public class UserMoneyRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public UserMoneyRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<UserMoneyRecord> QueryWhereUserMoneyRecord(IQueryable<UserMoneyRecord> list,
            int? UserID = null,
            string UserName = null,
            int? OperateType = null,
            string RelatedSign = null,
            int? OperateMoneyWhere = null,
            double? OperateMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            if (UserID != null)
            {
                list = list.Where(m => m.UserID == UserID);
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                list = list.Where(m => m.UserName.Contains(UserName));
            }
            if (OperateType != null)
            {
                list = list.Where(m => m.OperateType == OperateType);
            }
            if (!string.IsNullOrEmpty(RelatedSign))
            {
                list = list.Where(m => m.RelatedSign.Contains(RelatedSign));
            }
            if (OperateMoneyWhere != null && OperateMoneyWhere > 0 && OperateMoney != null)
            {
                switch (OperateMoneyWhere)
                {
                    case 1: list = list.Where(m => m.OperateMoney < OperateMoney); break;
                    case 2: list = list.Where(m => m.OperateMoney <= OperateMoney); break;
                    case 3: list = list.Where(m => m.OperateMoney == OperateMoney); break;
                    case 4: list = list.Where(m => m.OperateMoney > OperateMoney); break;
                    case 5: list = list.Where(m => m.OperateMoney >= OperateMoney); break;
                }
            }
            if (AddTimeStart != null && AddTimeEnd != null)
            {
                if (AddTimeStart > AddTimeEnd)
                {
                    DateTime? temporary = AddTimeStart;
                    AddTimeStart = AddTimeEnd;
                    AddTimeEnd = temporary;
                }
            }
            if (AddTimeStart != null)
            {
                list = list.Where(c => c.AddTime >= AddTimeStart);
            }
            if (AddTimeEnd != null)
            {
                list = list.Where(c => c.AddTime < AddTimeEnd);
            }
            if (AddManagerID != null)
            {
                list = list.Where(m => m.AddManagerID == AddManagerID);
            }
            if (!string.IsNullOrEmpty(Remark))
            {
                list = list.Where(m => m.Remark.Contains(Remark));
            }
            return list;
        }

        [HttpGet]
        public object GetUserMoneyRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? UserID = null,
            string UserName = null,
            int? OperateType = null,
            string RelatedSign = null,
            int? OperateMoneyWhere = null,
            double? OperateMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<UserMoneyRecord> list = db.UserMoneyRecord.AsQueryable();
            list = QueryWhereUserMoneyRecord(list,
                UserID: UserID,
                UserName: UserName,
                OperateType: OperateType,
                RelatedSign: RelatedSign,
                OperateMoneyWhere: OperateMoneyWhere,
                OperateMoney: OperateMoney,
                AddTimeStart: AddTimeStart,
                AddTimeEnd: AddTimeEnd,
                AddManagerID: AddManagerID,
                Remark: Remark);

            int total = 0;
            var result = list
                .ToOrderBy(sort, order)
                .ToPager(page, rows, a => total = a)
                .ToList()
                .Select(m => new
                {
                    m.ID,
                    m.UserID,
                    m.UserName,
                    m.OperateType,
                    m.RelatedSign,
                    m.OperateMoney,
                    m.AddTime,
                    m.AddManagerID,
                    m.Remark,

                    OperateTypeName = db.SystemSetType.QueryEnumValue<KeysType.UserMoneyRecordOperateType>(m.OperateType),
                    AddManagerName = db.Managers
                        .Where(a => a.ID == m.AddManagerID)
                        .Select(a => a.TrueName ?? a.NickName)
                        .FirstOrDefault(),
                })
                .ToList();

            return new
            {
                rows = result,
                total,
            };
        }
    }
}
