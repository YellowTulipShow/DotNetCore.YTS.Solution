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
    /// 用户退款记录
    /// </summary>
    public class UserRefundMoneyRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public UserRefundMoneyRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<UserRefundMoneyRecord> QueryWhereUserRefundMoneyRecord(IQueryable<UserRefundMoneyRecord> list,
            int? UserID = null,
            string UserName = null,
            int? RefundMoneyWhere = null,
            double? RefundMoney = null,
            string UserRechargeRecordIDs = null,
            int? SumRechargeMoneyWhere = null,
            double? SumRechargeMoney = null,
            int? SumGiveAwayMoneyWhere = null,
            double? SumGiveAwayMoney = null,
            int? ActualRefundMoneyWhere = null,
            double? ActualRefundMoney = null,
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
            if (RefundMoneyWhere != null && RefundMoneyWhere > 0 && RefundMoney != null)
            {
                switch (RefundMoneyWhere)
                {
                    case 1: list = list.Where(m => m.RefundMoney < RefundMoney); break;
                    case 2: list = list.Where(m => m.RefundMoney <= RefundMoney); break;
                    case 3: list = list.Where(m => m.RefundMoney == RefundMoney); break;
                    case 4: list = list.Where(m => m.RefundMoney > RefundMoney); break;
                    case 5: list = list.Where(m => m.RefundMoney >= RefundMoney); break;
                }
            }
            if (!string.IsNullOrEmpty(UserRechargeRecordIDs))
            {
                list = list.Where(m => m.UserRechargeRecordIDs.Contains(UserRechargeRecordIDs));
            }
            if (SumRechargeMoneyWhere != null && SumRechargeMoneyWhere > 0 && SumRechargeMoney != null)
            {
                switch (SumRechargeMoneyWhere)
                {
                    case 1: list = list.Where(m => m.SumRechargeMoney < SumRechargeMoney); break;
                    case 2: list = list.Where(m => m.SumRechargeMoney <= SumRechargeMoney); break;
                    case 3: list = list.Where(m => m.SumRechargeMoney == SumRechargeMoney); break;
                    case 4: list = list.Where(m => m.SumRechargeMoney > SumRechargeMoney); break;
                    case 5: list = list.Where(m => m.SumRechargeMoney >= SumRechargeMoney); break;
                }
            }
            if (SumGiveAwayMoneyWhere != null && SumGiveAwayMoneyWhere > 0 && SumGiveAwayMoney != null)
            {
                switch (SumGiveAwayMoneyWhere)
                {
                    case 1: list = list.Where(m => m.SumGiveAwayMoney < SumGiveAwayMoney); break;
                    case 2: list = list.Where(m => m.SumGiveAwayMoney <= SumGiveAwayMoney); break;
                    case 3: list = list.Where(m => m.SumGiveAwayMoney == SumGiveAwayMoney); break;
                    case 4: list = list.Where(m => m.SumGiveAwayMoney > SumGiveAwayMoney); break;
                    case 5: list = list.Where(m => m.SumGiveAwayMoney >= SumGiveAwayMoney); break;
                }
            }
            if (ActualRefundMoneyWhere != null && ActualRefundMoneyWhere > 0 && ActualRefundMoney != null)
            {
                switch (ActualRefundMoneyWhere)
                {
                    case 1: list = list.Where(m => m.ActualRefundMoney < ActualRefundMoney); break;
                    case 2: list = list.Where(m => m.ActualRefundMoney <= ActualRefundMoney); break;
                    case 3: list = list.Where(m => m.ActualRefundMoney == ActualRefundMoney); break;
                    case 4: list = list.Where(m => m.ActualRefundMoney > ActualRefundMoney); break;
                    case 5: list = list.Where(m => m.ActualRefundMoney >= ActualRefundMoney); break;
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
        public object GetUserRefundMoneyRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? UserID = null,
            string UserName = null,
            int? RefundMoneyWhere = null,
            double? RefundMoney = null,
            string UserRechargeSetIDs = null,
            int? SumRechargeMoneyWhere = null,
            double? SumRechargeMoney = null,
            int? SumGiveAwayMoneyWhere = null,
            double? SumGiveAwayMoney = null,
            int? ActualRefundMoneyWhere = null,
            double? ActualRefundMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<UserRefundMoneyRecord> list = db.UserRefundMoneyRecord.AsQueryable();
            list = QueryWhereUserRefundMoneyRecord(list,
                UserID: UserID,
                UserName: UserName,
                RefundMoneyWhere: RefundMoneyWhere,
                RefundMoney: RefundMoney,
                UserRechargeRecordIDs: UserRechargeSetIDs,
                SumRechargeMoneyWhere: SumRechargeMoneyWhere,
                SumRechargeMoney: SumRechargeMoney,
                SumGiveAwayMoneyWhere: SumGiveAwayMoneyWhere,
                SumGiveAwayMoney: SumGiveAwayMoney,
                ActualRefundMoneyWhere: ActualRefundMoneyWhere,
                ActualRefundMoney: ActualRefundMoney,
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
                    m.RefundMoney,
                    m.UserRechargeRecordIDs,
                    m.SumRechargeMoney,
                    m.SumGiveAwayMoney,
                    m.ActualRefundMoney,
                    m.AddTime,
                    m.AddManagerID,
                    AddManagerName = db.Managers
                        .Where(a => a.ID == m.AddManagerID)
                        .Select(a => a.TrueName ?? a.NickName)
                        .FirstOrDefault(),
                    m.Remark
                })
                .ToList();

            return new
            {
                rows = result,
                total,
            };
        }

        [HttpGet]
        public Result<object> GetUserRefundMoneyRecord(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.UserRefundMoneyRecord.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> AddUserRefundMoneyRecord(UserRefundMoneyRecord model)
        {
            var result = new Result<object>();
            if (model.ID > 0)
            {
                result.Code = ResultCode.Forbidden;
                result.Message = "不能修改报损记录!";
                return result;
            }

            // 用户查询
            Users user = db.Users.Where(a => a.ID == model.UserID).FirstOrDefault();
            if (user == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "用户查询为空!";
                return result;
            }

            var userOperate = new UserOperate(db, GetManager(db));
            userOperate.AddUserRefundMoneyRecord(user, model);
            db.SaveChanges();

            result.Data = model.ID;
            result.Message = "添加成功!";
            return result;
        }
    }
}
