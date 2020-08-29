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
    /// 用户充值记录
    /// </summary>
    public class UserRechargeRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public UserRechargeRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<UserRechargeRecord> QueryWhereUserRechargeRecord(IQueryable<UserRechargeRecord> list,
            int? UserID = null,
            string UserName = null,
            int? UserRechargeSetID = null,
            string ProjectName = null,
            int? RechargeMoneyWhere = null,
            double? RechargeMoney = null,
            int? GiveAwayMoneyWhere = null,
            double? GiveAwayMoney = null,
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
            if (UserRechargeSetID != null)
            {
                list = list.Where(m => m.UserRechargeSetID == UserRechargeSetID);
            }
            if (!string.IsNullOrEmpty(ProjectName))
            {
                list = list.Where(m => m.ProjectName.Contains(ProjectName));
            }
            if (RechargeMoneyWhere != null && RechargeMoneyWhere > 0 && RechargeMoney != null)
            {
                switch (RechargeMoneyWhere)
                {
                    case 1: list = list.Where(m => m.RechargeMoney < RechargeMoney); break;
                    case 2: list = list.Where(m => m.RechargeMoney <= RechargeMoney); break;
                    case 3: list = list.Where(m => m.RechargeMoney == RechargeMoney); break;
                    case 4: list = list.Where(m => m.RechargeMoney > RechargeMoney); break;
                    case 5: list = list.Where(m => m.RechargeMoney >= RechargeMoney); break;
                }
            }
            if (GiveAwayMoneyWhere != null && GiveAwayMoneyWhere > 0 && GiveAwayMoney != null)
            {
                switch (GiveAwayMoneyWhere)
                {
                    case 1: list = list.Where(m => m.GiveAwayMoney < GiveAwayMoney); break;
                    case 2: list = list.Where(m => m.GiveAwayMoney <= GiveAwayMoney); break;
                    case 3: list = list.Where(m => m.GiveAwayMoney == GiveAwayMoney); break;
                    case 4: list = list.Where(m => m.GiveAwayMoney > GiveAwayMoney); break;
                    case 5: list = list.Where(m => m.GiveAwayMoney >= GiveAwayMoney); break;
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
        public object GetUserRechargeRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? UserID = null,
            string UserName = null,
            int? UserRechargeSetID = null,
            string ProjectName = null,
            int? RechargeMoneyWhere = null,
            double? RechargeMoney = null,
            int? GiveAwayMoneyWhere = null,
            double? GiveAwayMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<UserRechargeRecord> list = db.UserRechargeRecord.AsQueryable();
            list = QueryWhereUserRechargeRecord(list,
                UserID: UserID,
                UserName: UserName,
                UserRechargeSetID: UserRechargeSetID,
                ProjectName: ProjectName,
                RechargeMoneyWhere: RechargeMoneyWhere,
                RechargeMoney: RechargeMoney,
                GiveAwayMoneyWhere: GiveAwayMoneyWhere,
                GiveAwayMoney: GiveAwayMoney,
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
                    m.UserRechargeSetID,
                    m.ProjectName,
                    m.RechargeMoney,
                    m.GiveAwayMoney,
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

        [HttpPost]
        public Result<object> AddUserRechargeRecord(UserRechargeRecord model)
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

            // 充值设置
            UserRechargeSet userRechargeSet = db.UserRechargeSet.Where(a => a.ID == model.UserRechargeSetID).FirstOrDefault();
            if (userRechargeSet == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "充值设置查询为空!";
                return result;
            }

            var userOperate = new UserOperate(db, GetManager(db));
            userOperate.AddUserRechargeRecord(user, userRechargeSet, model);
            db.SaveChanges();

            result.Data = model.ID;
            result.Message = "添加成功!";
            return result;
        }
    }
}
