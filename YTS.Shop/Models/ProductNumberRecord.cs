using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 产品数量修改记录
    /// </summary>
    public class ProductNumberRecord
    {
        /// <summary>
        /// ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

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
        /// 操作类型
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 操作产品数量
        /// </summary>
        public int OperateNumber { get; set; }

        /// <summary>
        /// 是否无限数量
        /// </summary>
        public bool IsUnlimitedNumber { get; set; }

        /// <summary>
        /// 相关标识
        /// </summary>
        public string RelatedSign { get; set; }

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
