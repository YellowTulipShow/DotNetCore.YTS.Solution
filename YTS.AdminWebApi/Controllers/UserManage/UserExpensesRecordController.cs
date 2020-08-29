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
    /// 用户消费记录
    /// </summary>
    public class UserExpensesRecordController : BaseApiController
    {
        protected YTSEntityContext db;
        public UserExpensesRecordController(YTSEntityContext db)
        {
            this.db = db;
        }

        private IQueryable<UserExpensesRecord> QueryWhereUserExpensesRecord(IQueryable<UserExpensesRecord> list,
            int? UserID = null,
            string UserName = null,
            string BatchNo = null,
            string ExpensesOrderNo = null,
            int? ProductID = null,
            string ProductName = null,
            int? ProductPriceWhere = null,
            double? ProductPrice = null,
            int? ProductBuyNumberWhere = null,
            int? ProductBuyNumber = null,
            int? ProductExpensesMoneyWhere = null,
            double? ProductExpensesMoney = null,
            int? ProductPayMoneyWhere = null,
            double? ProductPayMoney = null,
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
            if (!string.IsNullOrEmpty(BatchNo))
            {
                list = list.Where(m => m.BatchNo.Contains(BatchNo));
            }
            if (!string.IsNullOrEmpty(ExpensesOrderNo))
            {
                list = list.Where(m => m.ExpensesOrderNo.Contains(ExpensesOrderNo));
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
            if (ProductBuyNumberWhere != null && ProductBuyNumberWhere > 0 && ProductBuyNumber != null)
            {
                switch (ProductBuyNumberWhere)
                {
                    case 1: list = list.Where(m => m.ProductBuyNumber < ProductBuyNumber); break;
                    case 2: list = list.Where(m => m.ProductBuyNumber <= ProductBuyNumber); break;
                    case 3: list = list.Where(m => m.ProductBuyNumber == ProductBuyNumber); break;
                    case 4: list = list.Where(m => m.ProductBuyNumber > ProductBuyNumber); break;
                    case 5: list = list.Where(m => m.ProductBuyNumber >= ProductBuyNumber); break;
                }
            }
            if (ProductExpensesMoneyWhere != null && ProductExpensesMoneyWhere > 0 && ProductExpensesMoney != null)
            {
                switch (ProductExpensesMoneyWhere)
                {
                    case 1: list = list.Where(m => m.ProductExpensesMoney < ProductExpensesMoney); break;
                    case 2: list = list.Where(m => m.ProductExpensesMoney <= ProductExpensesMoney); break;
                    case 3: list = list.Where(m => m.ProductExpensesMoney == ProductExpensesMoney); break;
                    case 4: list = list.Where(m => m.ProductExpensesMoney > ProductExpensesMoney); break;
                    case 5: list = list.Where(m => m.ProductExpensesMoney >= ProductExpensesMoney); break;
                }
            }
            if (ProductPayMoneyWhere != null && ProductPayMoneyWhere > 0 && ProductPayMoney != null)
            {
                switch (ProductPayMoneyWhere)
                {
                    case 1: list = list.Where(m => m.ProductPayMoney < ProductPayMoney); break;
                    case 2: list = list.Where(m => m.ProductPayMoney <= ProductPayMoney); break;
                    case 3: list = list.Where(m => m.ProductPayMoney == ProductPayMoney); break;
                    case 4: list = list.Where(m => m.ProductPayMoney > ProductPayMoney); break;
                    case 5: list = list.Where(m => m.ProductPayMoney >= ProductPayMoney); break;
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
        public object GetUserExpensesRecordList(
            int? page = null, int? rows = null,
            string sort = null, string order = null,
            int? UserID = null,
            string UserName = null,
            string BatchNo = null,
            string ExpensesOrderNo = null,
            int? ProductID = null,
            string ProductName = null,
            int? ProductPriceWhere = null,
            double? ProductPrice = null,
            int? ProductBuyNumberWhere = null,
            int? ProductBuyNumber = null,
            int? ProductExpensesMoneyWhere = null,
            double? ProductExpensesMoney = null,
            int? ProductPayMoneyWhere = null,
            double? ProductPayMoney = null,
            DateTime? AddTimeStart = null,
            DateTime? AddTimeEnd = null,
            int? AddManagerID = null,
            string Remark = null)
        {
            IQueryable<UserExpensesRecord> list = db.UserExpensesRecord.AsQueryable();
            list = QueryWhereUserExpensesRecord(list,
                UserID: UserID,
                UserName: UserName,
                BatchNo: BatchNo,
                ExpensesOrderNo: ExpensesOrderNo,
                ProductID: ProductID,
                ProductName: ProductName,
                ProductPriceWhere: ProductPriceWhere,
                ProductPrice: ProductPrice,
                ProductBuyNumberWhere: ProductBuyNumberWhere,
                ProductBuyNumber: ProductBuyNumber,
                ProductExpensesMoneyWhere: ProductExpensesMoneyWhere,
                ProductExpensesMoney: ProductExpensesMoney,
                ProductPayMoneyWhere: ProductPayMoneyWhere,
                ProductPayMoney: ProductPayMoney,
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
                    m.BatchNo,
                    m.ExpensesOrderNo,
                    m.ProductID,
                    m.ProductName,
                    m.ProductPrice,
                    m.ProductBuyNumber,
                    m.ProductExpensesMoney,
                    m.ProductPayMoney,
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
        public Result AddUserExpensesRecord(UserExpensesRecord model)
        {
            return AddUserExpensesRecords(new UserExpensesRecord[] {  model, });
        }

        [HttpPost]
        public Result AddUserExpensesRecords(UserExpensesRecord[] models)
        {
            var result = new Result();
            models = models ?? new UserExpensesRecord[] {};
            var manager = GetManager(db);

            UserProductOperate userProductOperate = new UserProductOperate(db, GetManager(db));
            string BatchNo = OrderForm.CreateOrderNumber();
            foreach (var model in models)
            {
                if (model.ID > 0)
                {
                    result.Code = ResultCode.Forbidden;
                    result.Message = "不能修改消费记录!";
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

                userProductOperate.AddUserExpensesRecord(BatchNo, user, product, model);
            }
            db.SaveChanges();

            result.Message = "添加成功!";
            return result;
        }
    }
}
