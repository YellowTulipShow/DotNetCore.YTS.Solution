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
    /// 用户
    /// </summary>
    public class UsersController : BaseApiController
    {
        protected YTSEntityContext db;
        public UsersController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<Users> QueryWhereUsers(IQueryable<Users> list,
            string Account = null,
            string Password = null,
            string Name = null,
            string NickName = null,
            string Phone = null,
            string WeChatNumber = null,
            string QQ = null,
            string Address = null,
            int? MoneyWhere = null,
            double? Money = null,
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
            if (!string.IsNullOrEmpty(Name))
            {
                list = list.Where(m => m.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(NickName))
            {
                list = list.Where(m => m.NickName.Contains(NickName));
            }
            if (!string.IsNullOrEmpty(Phone))
            {
                list = list.Where(m => m.Phone.Contains(Phone));
            }
            if (!string.IsNullOrEmpty(WeChatNumber))
            {
                list = list.Where(m => m.WeChatNumber.Contains(WeChatNumber));
            }
            if (!string.IsNullOrEmpty(QQ))
            {
                list = list.Where(m => m.QQ.Contains(QQ));
            }
            if (!string.IsNullOrEmpty(Address))
            {
                list = list.Where(m => m.Address.Contains(Address));
            }
            if (MoneyWhere != null && MoneyWhere > 0 && Money != null)
            {
                switch (MoneyWhere)
                {
                    case 1: list = list.Where(m => m.Money < Money); break;
                    case 2: list = list.Where(m => m.Money <= Money); break;
                    case 3: list = list.Where(m => m.Money == Money); break;
                    case 4: list = list.Where(m => m.Money > Money); break;
                    case 5: list = list.Where(m => m.Money >= Money); break;
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
            return list;
        }

        [HttpGet]
        public object GetUsersList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            string Account = null,
            string Password = null,
            string Name = null,
            string NickName = null,
            string Phone = null,
            string WeChatNumber = null,
            string QQ = null,
            string Address = null,
            int? MoneyWhere = null,
            double? Money = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            IQueryable<Users> list = db.Users.AsQueryable();
            list = QueryWhereUsers(list,
                Account: Account,
                Password: Password,
                Name: Name,
                NickName: NickName,
                Phone: Phone,
                WeChatNumber: WeChatNumber,
                QQ: QQ,
                Address: Address,
                MoneyWhere: MoneyWhere,
                Money: Money,
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
                    m.Password,
                    m.Name,
                    m.NickName,
                    m.Phone,
                    m.WeChatNumber,
                    m.QQ,
                    m.Address,
                    m.Money,
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
        public Result<object> GetUsers(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.Users.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> EditUsers(Users model)
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
                db.Users.Add(model);
            }
            else
            {
                db.Users.Attach(model);
                EntityEntry<Users> entry = db.Entry(model);
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
        public Result DeleteUsers(int[] IDs)
        {
            var result = new Result();
            if (IDs == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "删除失败, IDs为空!";
                return result;
            }

            db.Users.RemoveRange(db.Users.Where(a => IDs.Contains(a.ID)).ToList());
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "删除成功！IDs:" + ConvertTool.ToString(IDs, ",");
            return result;
        }
    }
}
