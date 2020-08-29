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
    /// 产品数量修改记录
    /// </summary>
    public class ProductNumberRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public ProductNumberRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<ProductNumberRecord> QueryWhereProductNumberRecord(IQueryable<ProductNumberRecord> list,
            int? ProductID = null,
            string ProductName = null,
            int? OperateType = null,
            int? OperateNumberWhere = null,
            int? OperateNumber = null,
            bool? IsUnlimitedNumber = null,
            string RelatedSign = null,
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
            if (OperateType != null)
            {
                list = list.Where(m => m.OperateType == OperateType);
            }
            if (OperateNumberWhere != null && OperateNumberWhere > 0 && OperateNumber != null)
            {
                switch (OperateNumberWhere)
                {
                    case 1: list = list.Where(m => m.OperateNumber < OperateNumber); break;
                    case 2: list = list.Where(m => m.OperateNumber <= OperateNumber); break;
                    case 3: list = list.Where(m => m.OperateNumber == OperateNumber); break;
                    case 4: list = list.Where(m => m.OperateNumber > OperateNumber); break;
                    case 5: list = list.Where(m => m.OperateNumber >= OperateNumber); break;
                }
            }
            if (IsUnlimitedNumber != null)
            {
                list = list.Where(m => m.IsUnlimitedNumber == true == IsUnlimitedNumber);
            }
            if (!string.IsNullOrEmpty(RelatedSign))
            {
                list = list.Where(m => m.RelatedSign.Contains(RelatedSign));
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
        public object GetProductNumberRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? ProductID = null,
            string ProductName = null,
            int? OperateType = null,
            int? OperateNumberWhere = null,
            int? OperateNumber = null,
            bool? IsUnlimitedNumber = null,
            string RelatedSign = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<ProductNumberRecord> list = db.ProductNumberRecord.AsQueryable();
            list = QueryWhereProductNumberRecord(list,
                ProductID: ProductID,
                ProductName: ProductName,
                OperateType: OperateType,
                OperateNumberWhere: OperateNumberWhere,
                OperateNumber: OperateNumber,
                IsUnlimitedNumber: IsUnlimitedNumber,
                RelatedSign: RelatedSign,
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
                    m.OperateType,
                    m.OperateNumber,
                    m.IsUnlimitedNumber,
                    m.RelatedSign,
                    m.AddTime,
                    m.AddManagerID,
                    m.Remark,

                    OperateTypeName = db.SystemSetType.QueryEnumValue<KeysType.ProductNumberRecordOperateType>(m.OperateType),
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
    }
}
