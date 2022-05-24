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
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace YTS.AdminWebApi.Controllers
{
    public class MenusController : BaseApiController
    {
        protected YTSEntityContext db;
        public MenusController(YTSEntityContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public object GetMenusList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            string NameSpaces = null,
            int? id = null)
        {
            var list = db.Menus.AsQueryable();
            if (!string.IsNullOrWhiteSpace(NameSpaces))
            {
                list = list.Where(a => a.NameSpaces == NameSpaces);
            }
            int total = 0;
            var result = list
                .Where(a => a.ParentID == id)
                .ToOrderBy(sort, order)
                .ToPager(page, rows, a => total = a)
                .ToList()
                .Select(m => new MenusExtend._menu()
                {
                    id = m.ID,
                    text = m.Name,
                    code = m.Code,
                    url = m.Url,
                    state = MenusExtend._menu_status[0],
                    childrenCount = list
                        .Where(a => a.ParentID == m.ID)
                        .Count(),
                });
            return result;
        }

        private IQueryable<Menus> QueryWhereMenus(IQueryable<Menus> list,
            int? ParentID = null,
            string NameSpaces = null,
            string Code = null,
            string Name = null,
            string Url = null,
            bool? IsHide = null,
            int? Ordinal = null,
            string Remark = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            if (ParentID != null)
            {
                list = list.Where(m => m.ParentID == ParentID);
            }
            if (!string.IsNullOrEmpty(NameSpaces))
            {
                list = list.Where(m => m.NameSpaces.Contains(NameSpaces));
            }
            if (!string.IsNullOrEmpty(Code))
            {
                list = list.Where(m => m.Code.Contains(Code));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                list = list.Where(m => m.Name.Contains(Name));
            }
            if (!string.IsNullOrEmpty(Url))
            {
                list = list.Where(m => m.Url.Contains(Url));
            }
            if (IsHide != null)
            {
                list = list.Where(m => m.IsHide == true == IsHide);
            }
            if (Ordinal != null)
            {
                list = list.Where(m => m.Ordinal == Ordinal);
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
        public object GetMenuInfosList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? ParentID = null,
            string NameSpaces = null,
            string Code = null,
            string Name = null,
            string Url = null,
            bool? IsHide = null,
            int? Ordinal = null,
            string Remark = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            IQueryable<Menus> list = db.Menus.AsQueryable();
            list = QueryWhereMenus(list,
                ParentID: ParentID,
                NameSpaces: NameSpaces,
                Code: Code,
                Name: Name,
                Url: Url,
                IsHide: IsHide,
                Ordinal: Ordinal,
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
                    m.ParentID,
                    m.NameSpaces,
                    m.Code,
                    m.Name,
                    m.Url,
                    m.IsHide,
                    m.Ordinal,
                    m.Remark,
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

        [HttpPost]
        public Result UploadMenus(string NameSpaces, IEnumerable<MenusExtend._menu> models)
        {
            var result = new Result();
            if (models == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "上传数据为空!";
                return result;
            }

            var menusExtend = new MenusExtend(db, GetManager(db));
            int needDBCount = menusExtend.UpdateDBDatas(NameSpaces, models);

            result.Code = ResultCode.OK;
            if (needDBCount <= 0)
                result.Message = "需要上传的菜单为空!";
            else
                result.Message = $"完成上传的菜单数量: {needDBCount}!";
            return result;
        }

        [HttpPost]
        public Result<object> EditMenus(Menus model)
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
                db.Menus.Add(model);
            }
            else
            {
                db.Menus.Attach(model);
                EntityEntry<Menus> entry = db.Entry(model);
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
        public Result DeleteMenus(int[] IDs)
        {
            var result = new Result();
            if (IDs == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "删除失败, IDs为空!";
                return result;
            }

            db.Menus.RemoveRange(db.Menus.Where(a => IDs.Contains(a.ID)).ToList());
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "删除成功！IDs:" + ConvertTool.ToString(IDs, ",");
            return result;
        }
    }
}
