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
    public class ManagerGroupController : BaseApiController
    {
        protected YTSEntityContext db;
        public ManagerGroupController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<ManagerGroup> QueryWhereManagerGroup(IQueryable<ManagerGroup> list,
            string GroupName = null,
            string Remark = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            if (!string.IsNullOrEmpty(GroupName))
            {
                list = list.Where(m => m.GroupName.Contains(GroupName));
            }
            if (!string.IsNullOrEmpty(Remark))
            {
                list = list.Where(m => m.Remark.Contains(Remark));
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
            return list;
        }

        [HttpGet]
        public object GetManagerGroupList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            string GroupName = null,
            string Remark = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            IQueryable<ManagerGroup> list = db.ManagerGroup.AsQueryable();
            list = QueryWhereManagerGroup(list,
                GroupName: GroupName,
                Remark: Remark,
                AddTimeStart: AddTimeStart,
                AddTimeEnd: AddTimeEnd,
                AddManagerID: AddManagerID);
            int total = 0;
            var result = list
                .ToOrderBy(sort, order)
                .ToPager(page, rows, a => total = a)
                .ToList()
                .Select(m => new
                {
                    m.ID,
                    m.GroupName,
                    m.Remark,
                    m.AddTime,
                    m.AddManagerID,
                    AddManagerName = db.Managers
                        .Where(a => a.ID == m.AddManagerID)
                        .Select(a => a.TrueName ?? a.NickName)
                        .FirstOrDefault(),
                    AddManagerGroupName = db.Managers
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

        [HttpGet]
        public Result<object> GetManagerGroup(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.ManagerGroup.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> EditManagerGroup(ManagerGroup model)
        {
            var result = new Result<object>();
            if (model == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "模型为空!";
                return result;
            }
            var ID = model.ID;
            if (ID <= 0)
            {
                model.AddTime = DateTime.Now;
                model.AddManagerID = GetManager(db).ID;
                db.ManagerGroup.Add(model);
            }
            else
            {
                db.ManagerGroup.Attach(model);
                EntityEntry<ManagerGroup> entry = db.Entry(model);
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
        public Result DeleteManagerGroup(int[] IDs)
        {
            var result = new Result();
            if (IDs == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "删除失败, IDs为空!";
                return result;
            }

            db.ManagerGroup.RemoveRange(db.ManagerGroup.Where(a => IDs.Contains(a.ID)).ToList());
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "删除成功！IDs:" + ConvertTool.ToString(IDs, ",");
            return result;
        }
    }
}
