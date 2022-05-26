using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 用户消费记录
    /// </summary>
    public class UserExpensesRecord
    {
        /// <summary>
        /// ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey("UserID")]
        public Users User { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 消费单号
        /// </summary>
        public string ExpensesOrderNo { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public double ProductPrice { get; set; }

        /// <summary>
        /// 产品购买数量
        /// </summary>
        public int ProductBuyNumber { get; set; }

        /// <summary>
        /// 产品消费金额
        /// </summary>
        public double ProductExpensesMoney { get; set; }

        /// <summary>
        /// 产品实际购买支付金额
        /// </summary>
        public double ProductPayMoney { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 添加人Id
        /// </summary>
        public int? AddManagerID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
