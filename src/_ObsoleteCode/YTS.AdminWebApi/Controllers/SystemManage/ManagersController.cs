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
using Microsoft.AspNetCore.Authorization;

namespace YTS.AdminWebApi.Controllers
{
    public class ManagersController : BaseApiController
    {
        protected YTSEntityContext db;
        public ManagersController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<Managers> QueryWhereManagers(IQueryable<Managers> list,
            string Account = null,
            string Password = null,
            string NickName = null,
            string TrueName = null,
            string Phone = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            if (!string.IsNullOrEmpty(Account))
            {
                list = list.Where(m => m.Account.Contains(Account));
            }
            if (!string.IsNullOrEmpty(Password))
            {
                list = list.Where(m => m.Password.Contains(Password));
            }
            if (!string.IsNullOrEmpty(NickName))
            {
                list = list.Where(m => m.NickName.Contains(NickName));
            }
            if (!string.IsNullOrEmpty(TrueName))
            {
                list = list.Where(m => m.TrueName.Contains(TrueName));
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                list = list.Where(m => m.Phone.Contains(Phone));
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
        public object GetManagersList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            string Account = null,
            string Password = null,
            string NickName = null,
            string TrueName = null,
            string Phone = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            IQueryable<Managers> list = db.Managers.AsQueryable();
            list = QueryWhereManagers(list,
                Account: Account,
                Password: Password,
                NickName: NickName,
                TrueName: TrueName,
                Phone: Phone,
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
                    m.Account,
                    m.NickName,
                    m.TrueName,
                    m.Phone,
                    m.AddTime,
                    m.AddManagerID,
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

        [HttpGet]
        public Result<object> GetManager(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.Managers.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> EditManager(Managers model)
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
                try
                {
                    model.Password = ManageAuthentication.EncryptionPassword(model.Password);
                }
                catch (Exception ex)
                {
                    result.Code = ResultCode.BadRequest;
                    result.Message = ex.Message;
                    return result;
                }

                db.Managers.Add(model);
            }
            else
            {
                db.Managers.Attach(model);
                EntityEntry<Managers> entry = db.Entry(model);
                entry.State = EntityState.Modified;
                entry.Property(gp => gp.AddTime).IsModified = false;
                entry.Property(gp => gp.AddManagerID).IsModified = false;
                entry.Property(gp => gp.Password).IsModified = false;
            }
            db.SaveChanges();
            result.Data = model.ID;
            result.Message = (ID == 0 ? "添加" : "修改") + "成功！";
            return result;
        }

        [HttpPost]
        public Result DeleteManagers(int[] IDs)
        {
            var result = new Result();
            if (IDs == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "删除失败, IDs为空!";
                return result;
            }

            db.Managers.RemoveRange(db.Managers.Where(a => IDs.Contains(a.ID)).ToList());
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "删除成功！IDs:" + ConvertTool.ToString(IDs, ",");
            return result;
        }

        [HttpPost]
        public Result ManagerChangePassword(int ID, string Account, string Password)
        {
            var result = new Result();
            var model = GetManager(db);
            if (model.ID != ID || model.Account != Account)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "请勿伪造身份修改账户密码!";
                return result;
            }

            var encryPwd = ManageAuthentication.EncryptionPassword(Password);
            model.Password = encryPwd;
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "修改密码成功";
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult InitFirstManager()
        {
            if (db.Managers.Count() > 0)
            {
                return BadRequest("无须初始化!");
            }
            var manager = new Managers()
            {
                Account = "admin",
                Password = "123456",
                NickName = "admin",
                TrueName = "admin",
                Phone = "",
                AddManagerID = 0,
                AddTime = DateTime.Now,
            };
            manager.Password = ManageAuthentication.EncryptionPassword(manager.Password);
            db.Managers.Add(manager);
            db.SaveChanges();
            return Ok("初始化成功!");
        }
    }
}
