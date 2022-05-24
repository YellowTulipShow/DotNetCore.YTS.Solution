using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 用户退货记录
    /// </summary>
    public class UserReturnGoodsRecord
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
        /// 用户消费记录ID
        /// </summary>
        public int? UserExpensesRecordID { get; set; }

        /// <summary>
        /// 用户消费记录单号
        /// </summary>
        public string UserExpensesRecordOrderNo { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int? ProductID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public double? ProductPrice { get; set; }

        /// <summary>
        /// 产品退货数量
        /// </summary>
        public int? ReturnNumber { get; set; }

        /// <summary>
        /// 退货金额
        /// </summary>
        public double? ReturnMoney { get; set; }

        /// <summary>
        /// 实际退货金额
        /// </summary>
        public double ActualReturnMoney { get; set; }

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
