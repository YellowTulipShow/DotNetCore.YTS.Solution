using System;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    public class ProductOperate
    {
        protected YTSEntityContext db;
        protected Managers manager;
        public ProductOperate(YTSEntityContext db, Managers manager)
        {
            this.db = db;
            this.manager = manager;
        }

        /// <summary>
        /// 增加产品报损记录
        /// </summary>
        /// <param name="product">产品信息</param>
        /// <param name="model">需要增加的产品报损记录</param>
        public void AddProductDamagedRecord(Product product, ProductDamagedRecord model)
        {
            // 固定数据格式设置
            model.BatchNo = OrderForm.CreateOrderNumber();
            model.ProductID = product.ID;
            model.ProductName = model.ProductName ?? product.Name;
            model.AddTime = DateTime.Now;
            model.AddManagerID = manager.ID;
            db.ProductDamagedRecord.Add(model);

            int OperateNumber = 0;

            // 产品数量修改
            if (!product.IsUnlimitedNumber)
            {
                product.Number -= model.Number;
                OperateNumber = model.Number * -1;
            }

            // 增加产品数量变动记录
            db.ProductNumberRecord.Add(new ProductNumberRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                IsUnlimitedNumber = product.IsUnlimitedNumber,
                OperateNumber = OperateNumber,
                OperateType = (int)KeysType.ProductNumberRecordOperateType.Damaged,
                ProductID = model.ProductID,
                ProductName = model.ProductName,
                RelatedSign = model.BatchNo,
                Remark = string.Format("产品({0})报损:{1}", product.Name, OperateNumber),
            });
        }

        /// <summary>
        /// 增加产品入库记录
        /// </summary>
        /// <param name="product">产品信息</param>
        /// <param name="model">需要增加的产品入库记录</param>
        public void AddProductEnterWarehouseRecord(Product product, ProductEnterWarehouseRecord model)
        {
            // 固定数据格式设置
            model.BatchNo = OrderForm.CreateOrderNumber();
            model.ProductID = product.ID;
            model.ProductName = model.ProductName ?? product.Name;
            model.AddTime = DateTime.Now;
            model.AddManagerID = manager.ID;
            db.ProductEnterWarehouseRecord.Add(model);

            int OperateNumber = 0;

            // 产品数量修改
            if (!product.IsUnlimitedNumber)
            {
                product.Number += model.Number;
                OperateNumber = model.Number;
            }

            // 增加产品数量变动记录
            db.ProductNumberRecord.Add(new ProductNumberRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                IsUnlimitedNumber = product.IsUnlimitedNumber,
                OperateNumber = OperateNumber,
                OperateType = (int)KeysType.ProductNumberRecordOperateType.EnterWarehouse,
                ProductID = model.ProductID,
                ProductName = model.ProductName,
                RelatedSign = model.BatchNo,
                Remark = string.Format("产品({0})入库:{1}", product.Name, OperateNumber),
            });
        }
    }
}
