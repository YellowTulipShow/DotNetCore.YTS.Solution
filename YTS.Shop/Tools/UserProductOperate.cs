using System;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    public class UserProductOperate
    {
        protected YTSEntityContext db;
        protected Managers manager;
        public UserProductOperate(YTSEntityContext db, Managers manager)
        {
            this.db = db;
            this.manager = manager;
        }

        /// <summary>
        /// 添加用户消费记录
        /// </summary>
        /// <param name="BatchNo">批次号</param>
        /// <param name="user">用户</param>
        /// <param name="product">产品</param>
        /// <param name="model">用户消费记录</param>
        public void AddUserExpensesRecord(string BatchNo, Users user, Product product, UserExpensesRecord model)
        {
            model.BatchNo = BatchNo;
            model.ExpensesOrderNo = OrderForm.CreateOrderNumber();
            model.AddManagerID = model.AddManagerID ?? manager.ID;
            model.AddTime = DateTime.Now;
            model.UserID = user.ID;
            model.UserName = user.Name ?? user.NickName;
            model.ProductID = product.ID;
            model.ProductName = product.Name;
            model.ProductPrice = product.Price;
            db.UserExpensesRecord.Add(model);

            double ProductPayMoney = model.ProductPayMoney * -1;
            // 用户金额修改
            user.Money += ProductPayMoney;
            // 增加用户金额变动记录
            db.UserMoneyRecord.Add(new UserMoneyRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                OperateType = (int)KeysType.UserMoneyRecordOperateType.Consumption,
                RelatedSign = "",
                UserID = user.ID,
                UserName = user.Name ?? user.NickName,
                OperateMoney = ProductPayMoney,
                Remark = $"用户({model.UserName})产品消费, 消费金额:{ProductPayMoney}",
            });

            int ProductBuyNumber = 0;
            // 产品数量修改
            if (!product.IsUnlimitedNumber)
            {
                ProductBuyNumber = model.ProductBuyNumber * -1;
                product.Number += ProductBuyNumber;
            }
            // 增加产品数量变动记录
            db.ProductNumberRecord.Add(new ProductNumberRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                IsUnlimitedNumber = product.IsUnlimitedNumber,
                OperateNumber = ProductBuyNumber,
                OperateType = (int)KeysType.ProductNumberRecordOperateType.Sold,
                ProductID = model.ProductID,
                ProductName = model.ProductName,
                RelatedSign = model.BatchNo,
                Remark = $"用户({model.UserName})产品消费, 产品({model.ProductName})售出扣除:{ProductBuyNumber}",
            });
        }

        public void AddUserReturnGoodsRecord(Users user, Product product, UserReturnGoodsRecord model)
        {
            model.AddManagerID = manager.ID;
            model.AddTime = DateTime.Now;
            model.UserID = user.ID;
            model.UserName = user.Name ?? user.NickName;
            model.ProductID = product.ID;
            model.ProductName = product.Name;
            model.ProductPrice = product.Price;
            db.UserReturnGoodsRecord.Add(model);

            // 用户金额修改
            user.Money += model.ActualReturnMoney;
            // 增加用户金额变动记录
            db.UserMoneyRecord.Add(new UserMoneyRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                OperateType = (int)KeysType.UserMoneyRecordOperateType.ProductReturnGoods,
                RelatedSign = "",
                UserID = user.ID,
                UserName = user.Name ?? user.NickName,
                OperateMoney = model.ActualReturnMoney,
                Remark = $"用户({model.UserName})产品退货, 账户回退金额:{model.ActualReturnMoney}",
            });

            int ReturnNumber = 0;
            // 产品数量修改
            if (!product.IsUnlimitedNumber)
            {
                ReturnNumber = model.ReturnNumber ?? 0;
                product.Number += ReturnNumber;
            }
            // 增加产品数量变动记录
            db.ProductNumberRecord.Add(new ProductNumberRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                IsUnlimitedNumber = product.IsUnlimitedNumber,
                OperateNumber = ReturnNumber,
                OperateType = (int)KeysType.ProductNumberRecordOperateType.EnterWarehouse,
                ProductID = model.ProductID ?? 0,
                ProductName = model.ProductName,
                RelatedSign = $"消费记录单号:{model.UserExpensesRecordOrderNo ?? string.Empty}",
                Remark = $"用户({model.UserName})产品退货, 产品({model.ProductName})回退数量:{ReturnNumber}",
            });
        }
    }
}
