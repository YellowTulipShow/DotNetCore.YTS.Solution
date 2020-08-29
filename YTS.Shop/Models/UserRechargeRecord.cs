using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 用户充值记录
    /// </summary>
    public class UserRechargeRecord
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
        [NotMapped]
        [ForeignKey("UserID")]
        public Users User { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户充值设置ID
        /// </summary>
        public int? UserRechargeSetID { get; set; }

        /// <summary>
        /// 用户充值设置
        /// </summary>
        [ForeignKey("UserRechargeSetID")]
        public UserRechargeSet UserRechargeSet { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public double RechargeMoney { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        public double? GiveAwayMoney { get; set; }

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
