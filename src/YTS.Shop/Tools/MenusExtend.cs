using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    /// <summary>
    /// 菜单扩展
    /// </summary>
    public class MenusExtend
    {
        protected YTSEntityContext db;
        protected Managers menager;
        public MenusExtend(YTSEntityContext db, Managers menager)
        {
            this.db = db;
            this.menager = menager;
        }

        public readonly static string[] _menu_status = new string[] { "closed", "open" };

        public class _menu
        {
            public int? id { get; set; }
            public string text { get; set; }
            public string code { get; set; }
            public string url { get; set; }
            public string state { get; set; }
            public bool? ishide { get; set; }
            public int? childrenCount { get; set; }
            public IEnumerable<_menu> children { get; set; }
        }

        public int UpdateDBDatas(string NameSpaces, IEnumerable<_menu> models, int? parentID = null)
        {
            int needDBCount = 0;
            if (models == null)
                return needDBCount;
            foreach (_menu item in models)
            {
                var menu = ToDBDatas(NameSpaces, item, parentID);
                if (menu == null)
                {
                    continue;
                }
                needDBCount += 1;
                if (menu.ID <= 0)
                {
                    menu.AddTime = DateTime.Now;
                    menu.AddManagerID = menager.ID;
                    db.Menus.Add(menu);
                }
                else
                {
                    db.Menus.Attach(menu);
                    EntityEntry<Menus> entry = db.Entry(menu);
                    entry.State = EntityState.Modified;
                    entry.Property(gp => gp.AddTime).IsModified = false;
                    entry.Property(gp => gp.AddManagerID).IsModified = false;
                }
                db.SaveChanges();

                if (item.children != null)
                {
                    needDBCount += UpdateDBDatas(NameSpaces, item.children, menu.ID);
                }
            }
            return needDBCount;
        }

        public Menus ToDBDatas(string NameSpaces, _menu _menu, int? parentID = null)
        {
            if (string.IsNullOrWhiteSpace(_menu.code))
                return null;
            var model = db.Menus
                .Where(a => a.NameSpaces == NameSpaces && a.Code == _menu.code)
                .FirstOrDefault();
            if (model == null)
            {
                return new Menus()
                {
                    ID = 0,
                    NameSpaces = NameSpaces,
                    ParentID = parentID,
                    Code = _menu.code,
                    Name = _menu.text,
                    Url = _menu.url,
                    IsHide = _menu.ishide ?? false,
                    Ordinal = (db.Menus
                        .Where(a => a.NameSpaces == NameSpaces && a.ParentID == parentID)
                        .Max(a => (int?)a.Ordinal) ?? 0) + 1,
                };
            }
            else
            {
                model.IsHide = _menu.ishide ?? false;
                model.Name = _menu.text;
                model.Url = _menu.url;
                return model;
            }
        }
    }
}
