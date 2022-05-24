using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 店铺管理员组
    /// </summary>
    public class ManagerGroup
    {
        /// <summary>
        /// ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 添加人ID
        /// </summary>
        public int? AddManagerID { get; set; }
    }
}
