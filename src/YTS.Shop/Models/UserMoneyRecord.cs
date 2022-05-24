using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 用户金额变动记录
    /// </summary>
    public class UserMoneyRecord
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
        /// 操作类型
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 相关标识
        /// </summary>
        public string RelatedSign { get; set; }

        /// <summary>
        /// 金额变动值
        /// </summary>
        public double OperateMoney { get; set; }

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
