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
    /// 产品
    /// </summary>
    public class ProductController : BaseApiController
    {
        protected YTSEntityContext db;
        public ProductController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<Product> QueryWhereProduct(IQueryable<Product> list,
            string Name = null,
            int? PriceWhere = null,
            double? Price = null,
            int? NumberWhere = null,
            int? Number = null,
            bool? IsUnlimitedNumber = null,
            bool? IsPhysicalEntity = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                list = list.Where(m => m.Name.Contains(Name));
            }
            if (PriceWhere != null && PriceWhere > 0 && Price != null)
            {
                switch (PriceWhere)
                {
                    case 1: list = list.Where(m => m.Price < Price); break;
                    case 2: list = list.Where(m => m.Price <= Price); break;
                    case 3: list = list.Where(m => m.Price == Price); break;
                    case 4: list = list.Where(m => m.Price > Price); break;
                    case 5: list = list.Where(m => m.Price >= Price); break;
                }
            }
            if (NumberWhere != null && NumberWhere > 0 && Number != null)
            {
                switch (NumberWhere)
                {
                    case 1: list = list.Where(m => m.Number < Number); break;
                    case 2: list = list.Where(m => m.Number <= Number); break;
                    case 3: list = list.Where(m => m.Number == Number); break;
                    case 4: list = list.Where(m => m.Number > Number); break;
                    case 5: list = list.Where(m => m.Number >= Number); break;
                }
            }
            if (IsUnlimitedNumber != null)
            {
                list = list.Where(m => m.IsUnlimitedNumber == true == IsUnlimitedNumber);
            }
            if (IsPhysicalEntity != null)
            {
                list = list.Where(m => m.IsPhysicalEntity == true == IsPhysicalEntity);
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
        public object GetProductList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            string Name = null,
            int? PriceWhere = null,
            double? Price = null,
            int? NumberWhere = null,
            int? Number = null,
            bool? IsUnlimitedNumber = null,
            bool? IsPhysicalEntity = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null)
        {
            IQueryable<Product> list = db.Product.AsQueryable();
            list = QueryWhereProduct(list,
                Name: Name,
                PriceWhere: PriceWhere,
                Price: Price,
                NumberWhere: NumberWhere,
                Number: Number,
                IsUnlimitedNumber: IsUnlimitedNumber,
                IsPhysicalEntity: IsPhysicalEntity,
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
                    m.Name,
                    m.Price,
                    m.Number,
                    m.IsUnlimitedNumber,
                    m.IsPhysicalEntity,
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
        public Result<object> GetProduct(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.Product.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> EditProduct(Product model)
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
                db.Product.Add(model);
            }
            else
            {
                db.Product.Attach(model);
                EntityEntry<Product> entry = db.Entry(model);
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
        public Result DeleteProduct(int[] IDs)
        {
            var result = new Result();
            if (IDs == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "删除失败, IDs为空!";
                return result;
            }

            db.Product.RemoveRange(db.Product.Where(a => IDs.Contains(a.ID)).ToList());
            db.SaveChanges();

            result.Code = ResultCode.OK;
            result.Message = "删除成功！IDs:" + ConvertTool.ToString(IDs, ",");
            return result;
        }
    }
}
