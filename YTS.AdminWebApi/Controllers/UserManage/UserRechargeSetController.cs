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
    /// 用户充值设置
    /// </summary>
    public class UserRechargeSetController : BaseApiController
    {
        protected YTSEntityContext db;
        public UserRechargeSetController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<UserRechargeSet> QueryWhereUserRechargeSet(IQueryable<UserRechargeSet> list,
            string ProjectName = null,
            int? RechargeMoneyWhere = null,
            double? RechargeMoney = null,
            int? GiveAwayMoneyWhere = null,
            double? GiveAwayMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            DateTime? UpdateTimeStart = null,
            DateTime? UpdateTimeEnd = null,
            int? UpdateManagerID = null,
            string Remark = null)
        {
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
            if (UpdateTimeStart != null && UpdateTimeEnd != null)
            {
                if (UpdateTimeStart > UpdateTimeEnd)
                {
                    DateTime? temporary = UpdateTimeStart;
                    UpdateTimeStart = UpdateTimeEnd;
                    UpdateTimeEnd = temporary;
                }
            }
            if (UpdateTimeStart != null)
            {
                list = list.Where(c => c.UpdateTime >= UpdateTimeStart);
            }
            if (UpdateTimeEnd != null)
            {
                list = list.Where(c => c.UpdateTime < UpdateTimeEnd);
            }
            if (UpdateManagerID != null)
            {
                list = list.Where(m => m.UpdateManagerID == UpdateManagerID);
            }
            if (!string.IsNullOrEmpty(Remark))
            {
                list = list.Where(m => m.Remark.Contains(Remark));
            }
            return list;
        }

        [HttpGet]
        public object GetUserRechargeSetList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            string ProjectName = null,
            int? RechargeMoneyWhere = null,
            double? RechargeMoney = null,
            int? GiveAwayMoneyWhere = null,
            double? GiveAwayMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            DateTime? UpdateTimeStart = null,
            DateTime? UpdateTimeEnd = null,
            int? UpdateManagerID = null,
            string Remark = null)
        {
            IQueryable<UserRechargeSet> list = db.UserRechargeSet.AsQueryable();
            list = QueryWhereUserRechargeSet(list,
                ProjectName: ProjectName,
                RechargeMoneyWhere: RechargeMoneyWhere,
                RechargeMoney: RechargeMoney,
                GiveAwayMoneyWhere: GiveAwayMoneyWhere,
                GiveAwayMoney: GiveAwayMoney,
                AddTimeStart: AddTimeStart,
                AddTimeEnd: AddTimeEnd,
                AddManagerID: AddManagerID,
                UpdateTimeStart: UpdateTimeStart,
                UpdateTimeEnd: UpdateTimeEnd,
                UpdateManagerID: UpdateManagerID,
                Remark: Remark);

            int total = 0;
            var result = list
                .ToOrderBy(sort, order)
                .ToPager(page, rows, a => total = a)
                .ToList()
                .Select(m => new
                {
                    m.ID,
                    m.ProjectName,
                    m.RechargeMoney,
                    m.GiveAwayMoney,
                    m.AddTime,
                    m.AddManagerID,
                    AddManagerName = db.Managers
                        .Where(a => a.ID == m.AddManagerID)
                        .Select(a => a.TrueName ?? a.NickName)
                        .FirstOrDefault(),
                    m.UpdateTime,
                    m.UpdateManagerID,
                    UpdateManagerName = db.Managers
                        .Where(a => a.ID == m.UpdateManagerID)
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
        public Result<object> GetUserRechargeSet(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.UserRechargeSet.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> EditUserRechargeSet(UserRechargeSet model)
        {
            var result = new Result<object>();
            if (model == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "模型为空!";
                return result;
            }
            var ID = model.ID;
            model.UpdateTime = DateTime.Now;
            model.UpdateManagerID = GetManager(db).ID;
            if (ID <= 0)
            {
                model.AddTime = DateTime.Now;
                model.AddManagerID = GetManager(db).ID;
                db.UserRechargeSet.Add(model);
            }
            else
            {
                db.UserRechargeSet.Attach(model);
                EntityEntry<UserRechargeSet> entry = db.Entry(model);
                entry.State = EntityState.Modified;
                entry.Property(gp => gp.AddTime).IsModified = false;
                entry.Property(gp => gp.AddManagerID).IsModified = false;
            }
            db.SaveChanges();
            result.Data = model.ID;
            result.Message = (ID == 0 ? "添加" : "修改") + "成功！";
            return result;
        }

        [HttpPost]
        public Result DeleteUserRechargeSet(int[] IDs)
        {
            var result = new Result();
            if (IDs == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "删除失败, IDs为空!";
                return result;
            }

            db.UserRechargeSet.RemoveRange(db.UserRechargeSet.Where(a => IDs.Contains(a.ID)).ToList());
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "删除成功！IDs:" + ConvertTool.ToString(IDs, ",");
            return result;
        }
    }
}
