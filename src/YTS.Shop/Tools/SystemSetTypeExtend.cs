using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    /// <summary>
    /// 系统字典表扩展
    /// </summary>
    public class SystemSetTypeExtend
    {
        protected YTSEntityContext db;
        public SystemSetTypeExtend(YTSEntityContext db)
        {
            this.db = db;
        }

        public class _menu
        {
            public string Explain { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
            public IEnumerable<_menu> children { get; set; }
        }

        /// <summary>
        /// 枚举类型转为字典设置
        /// </summary>
        /// <typeparam name="E">枚举类型</typeparam>
        /// <returns>字典设置</returns>
        public void UpdateEnumSetType<E>() where E : Enum
        {
            var key = SystemSetTypeStaticFunction.GetKey<E>();
            var model = new _menu()
            {
                Explain = SystemSetTypeStaticFunction.GetExplain<E>()?.Text,
                Key = key,
                Value = null,
                children = SystemSetTypeStaticFunction.GetEnumInfos<E>()
                    .Select(m => new _menu()
                    {
                        Explain = m.Explain,
                        Key = SystemSetTypeStaticFunction.GetBelowKey(key, m.Name),
                        Value = m.IntValue.ToString(),
                    })
                    .ToList()
            };
            UpdateDBDatas(new _menu[] { model });
        }

        public int UpdateDBDatas(IEnumerable<_menu> models, int? parentID = null)
        {
            int needDBCount = 0;
            if (models == null)
                return needDBCount;
            foreach (_menu item in models)
            {
                var menu = ToDBDatas(item, parentID);
                if (menu == null)
                {
                    continue;
                }
                needDBCount += 1;
                if (menu.ID <= 0)
                {
                    db.SystemSetType.Add(menu);
                }
                else
                {
                    db.SystemSetType.Attach(menu);
                    EntityEntry<SystemSetType> entry = db.Entry(menu);
                    entry.State = EntityState.Modified;
                }
                db.SaveChanges();

                if (item.children != null)
                {
                    needDBCount += UpdateDBDatas(item.children, menu.ID);
                }
            }
            return needDBCount;
        }

        public SystemSetType ToDBDatas(_menu _menu, int? parentID = null)
        {
            if (string.IsNullOrWhiteSpace(_menu.Key))
                return null;
            var model = db.SystemSetType
                .Where(a => a.ParentID == parentID && a.Key == _menu.Key && a.Value == _menu.Value)
                .FirstOrDefault();
            if (model == null)
            {
                return new SystemSetType()
                {
                    ID = 0,
                    ParentID = parentID,
                    Explain = _menu.Explain,
                    Key = _menu.Key,
                    Value = _menu.Value,
                    Ordinal = (db.SystemSetType
                        .Where(a => a.ParentID == parentID)
                        .Max(a => (int?)a.Ordinal) ?? 0) + 1,
                    Remark = null,
                };
            }
            else
            {
                model.Explain = _menu.Explain;
                model.Key = _menu.Key;
                model.Value = _menu.Value;
                return model;
            }
        }
    }
}
