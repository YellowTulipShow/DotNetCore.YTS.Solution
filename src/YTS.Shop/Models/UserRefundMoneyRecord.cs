using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 用户退款记录
    /// </summary>
    public class UserRefundMoneyRecord
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
        /// 客户要退款金额
        /// </summary>
        public double RefundMoney { get; set; }

        /// <summary>
        /// 用户充值记录ID列表逗号间隔
        /// </summary>
        public string UserRechargeRecordIDs { get; set; }

        /// <summary>
        /// 总充值金额
        /// </summary>
        public double? SumRechargeMoney { get; set; }

        /// <summary>
        /// 总赠送金额
        /// </summary>
        public double? SumGiveAwayMoney { get; set; }

        /// <summary>
        /// 实际退款金额
        /// </summary>
        public double ActualRefundMoney { get; set; }

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
