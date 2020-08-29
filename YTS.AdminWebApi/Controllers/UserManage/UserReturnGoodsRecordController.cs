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
    /// 用户退货记录
    /// </summary>
    public class UserReturnGoodsRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public UserReturnGoodsRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<UserReturnGoodsRecord> QueryWhereUserReturnGoodsRecord(IQueryable<UserReturnGoodsRecord> list,
            int? UserID = null,
            string UserName = null,
            int? UserExpensesRecordID = null,
            string UserExpensesRecordOrderNo = null,
            int? ProductID = null,
            string ProductName = null,
            int? ProductPriceWhere = null,
            double? ProductPrice = null,
            int? ReturnNumberWhere = null,
            int? ReturnNumber = null,
            int? ReturnMoneyWhere = null,
            double? ReturnMoney = null,
            int? ActualReturnMoneyWhere = null,
            double? ActualReturnMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            if (UserID != null)
            {
                list = list.Where(m => m.UserID == UserID);
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                list = list.Where(m => m.UserName.Contains(UserName));
            }
            if (UserExpensesRecordID != null)
            {
                list = list.Where(m => m.UserExpensesRecordID == UserExpensesRecordID);
            }
            if (!string.IsNullOrEmpty(UserExpensesRecordOrderNo))
            {
                list = list.Where(m => m.UserExpensesRecordOrderNo.Contains(UserExpensesRecordOrderNo));
            }
            if (ProductID != null)
            {
                list = list.Where(m => m.ProductID == ProductID);
            }
            if (!string.IsNullOrEmpty(ProductName))
            {
                list = list.Where(m => m.ProductName.Contains(ProductName));
            }
            if (ProductPriceWhere != null && ProductPriceWhere > 0 && ProductPrice != null)
            {
                switch (ProductPriceWhere)
                {
                    case 1: list = list.Where(m => m.ProductPrice < ProductPrice); break;
                    case 2: list = list.Where(m => m.ProductPrice <= ProductPrice); break;
                    case 3: list = list.Where(m => m.ProductPrice == ProductPrice); break;
                    case 4: list = list.Where(m => m.ProductPrice > ProductPrice); break;
                    case 5: list = list.Where(m => m.ProductPrice >= ProductPrice); break;
                }
            }
            if (ReturnNumberWhere != null && ReturnNumberWhere > 0 && ReturnNumber != null)
            {
                switch (ReturnNumberWhere)
                {
                    case 1: list = list.Where(m => m.ReturnNumber < ReturnNumber); break;
                    case 2: list = list.Where(m => m.ReturnNumber <= ReturnNumber); break;
                    case 3: list = list.Where(m => m.ReturnNumber == ReturnNumber); break;
                    case 4: list = list.Where(m => m.ReturnNumber > ReturnNumber); break;
                    case 5: list = list.Where(m => m.ReturnNumber >= ReturnNumber); break;
                }
            }
            if (ReturnMoneyWhere != null && ReturnMoneyWhere > 0 && ReturnMoney != null)
            {
                switch (ReturnMoneyWhere)
                {
                    case 1: list = list.Where(m => m.ReturnMoney < ReturnMoney); break;
                    case 2: list = list.Where(m => m.ReturnMoney <= ReturnMoney); break;
                    case 3: list = list.Where(m => m.ReturnMoney == ReturnMoney); break;
                    case 4: list = list.Where(m => m.ReturnMoney > ReturnMoney); break;
                    case 5: list = list.Where(m => m.ReturnMoney >= ReturnMoney); break;
                }
            }
            if (ActualReturnMoneyWhere != null && ActualReturnMoneyWhere > 0 && ActualReturnMoney != null)
            {
                switch (ActualReturnMoneyWhere)
                {
                    case 1: list = list.Where(m => m.ActualReturnMoney < ActualReturnMoney); break;
                    case 2: list = list.Where(m => m.ActualReturnMoney <= ActualReturnMoney); break;
                    case 3: list = list.Where(m => m.ActualReturnMoney == ActualReturnMoney); break;
                    case 4: list = list.Where(m => m.ActualReturnMoney > ActualReturnMoney); break;
                    case 5: list = list.Where(m => m.ActualReturnMoney >= ActualReturnMoney); break;
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
        public object GetUserReturnGoodsRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? UserID = null,
            string UserName = null,
            int? UserExpensesRecordID = null,
            string UserExpensesRecordOrderNo = null,
            int? ProductID = null,
            string ProductName = null,
            int? ProductPriceWhere = null,
            double? ProductPrice = null,
            int? ReturnNumberWhere = null,
            int? ReturnNumber = null,
            int? ReturnMoneyWhere = null,
            double? ReturnMoney = null,
            int? ActualReturnMoneyWhere = null,
            double? ActualReturnMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<UserReturnGoodsRecord> list = db.UserReturnGoodsRecord.AsQueryable();
            list = QueryWhereUserReturnGoodsRecord(list,
                UserID: UserID,
                UserName: UserName,
                UserExpensesRecordID: UserExpensesRecordID,
                UserExpensesRecordOrderNo: UserExpensesRecordOrderNo,
                ProductID: ProductID,
                ProductName: ProductName,
                ProductPriceWhere: ProductPriceWhere,
                ProductPrice: ProductPrice,
                ReturnNumberWhere: ReturnNumberWhere,
                ReturnNumber: ReturnNumber,
                ReturnMoneyWhere: ReturnMoneyWhere,
                ReturnMoney: ReturnMoney,
                ActualReturnMoneyWhere: ActualReturnMoneyWhere,
                ActualReturnMoney: ActualReturnMoney,
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
                    m.UserID,
                    m.UserName,
                    m.UserExpensesRecordID,
                    m.UserExpensesRecordOrderNo,
                    m.ProductID,
                    m.ProductName,
                    m.ProductPrice,
                    m.ReturnNumber,
                    m.ReturnMoney,
                    m.ActualReturnMoney,
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

        [HttpGet]
        public Result<object> GetUserReturnGoodsRecord(int? ID)
        {
            if ((ID ?? 0) <= 0)
            {
                return new Result<object>()
                {
                    Code = ResultCode.BadRequest,
                    Message = @"ID为空!",
                };
            }
            var model = db.UserReturnGoodsRecord.Where(m => m.ID == ID).FirstOrDefault();
            return new Result<object>()
            {
                Code = ResultCode.OK,
                Data = model,
                Message = model != null ? @"获取成功!" : @"数据获取为空!",
            };
        }

        [HttpPost]
        public Result<object> AddUserReturnGoodsRecord(UserReturnGoodsRecord model)
        {
            var result = new Result<object>();
            if (model.ID > 0)
            {
                result.Code = ResultCode.Forbidden;
                result.Message = "不能修改退货记录!";
                return result;
            }

            // 用户查询
            Users user = db.Users.Where(a => a.ID == model.UserID).FirstOrDefault();
            if (user == null)
            {
                result.Code = ResultCode.BadRequest;
                result.Message = "用户查询为空!";
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

            var userProductOperate = new UserProductOperate(db, GetManager(db));
            userProductOperate.AddUserReturnGoodsRecord(user, product, model);
            db.SaveChanges();

            result.Data = model.ID;
            result.Message = "添加成功!";
            return result;
        }
    }
}
