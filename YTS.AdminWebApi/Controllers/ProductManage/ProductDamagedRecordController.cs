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
    /// 产品报损记录
    /// </summary>
    public class ProductDamagedRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public ProductDamagedRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<ProductDamagedRecord> QueryWhereProductDamagedRecord(IQueryable<ProductDamagedRecord> list,
            int? ProductID = null,
            string ProductName = null,
            string BatchNo = null,
            int? NumberWhere = null,
            int? Number = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            if (ProductID != null)
            {
                list = list.Where(m => m.ProductID == ProductID);
            }
            if (!string.IsNullOrEmpty(ProductName))
            {
                list = list.Where(m => m.ProductName.Contains(ProductName));
            }
            if (!string.IsNullOrEmpty(BatchNo))
            {
                list = list.Where(m => m.BatchNo.Contains(BatchNo));
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
        public object GetProductDamagedRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? ProductID = null,
            string ProductName = null,
            string BatchNo = null,
            int? NumberWhere = null,
            int? Number = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<ProductDamagedRecord> list = db.ProductDamagedRecord.AsQueryable();
            list = QueryWhereProductDamagedRecord(list,
                ProductID: ProductID,
                ProductName: ProductName,
                BatchNo: BatchNo,
                NumberWhere: NumberWhere,
                Number: Number,
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
                    m.ProductID,
                    m.ProductName,
                    m.BatchNo,
                    m.Number,
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
        public Result<object> AddProductDamagedRecord(ProductDamagedRecord model)
        {
            var result = new Result<object>();
            if (model.ID > 0)
            {
                result.Code = ResultCode.Forbidden;
                result.Message = "不能修改报损记录!";
                return result;
            }

            // 产品查询
            Product product = db.Product.Where(a => a.ID == model.ProductID).FirstOrDefault();
            if (product == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "产品查询为空!";
                return result;
            }

            var productOperate = new ProductOperate(db, GetManager(db));
            productOperate.AddProductDamagedRecord(product, model);
            db.SaveChanges();

            result.Data = model.ID;
            result.Message = "添加成功!";
            return result;
        }
    }
}
