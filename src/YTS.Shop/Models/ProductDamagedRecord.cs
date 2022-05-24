using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 产品报损记录
    /// </summary>
    public class ProductDamagedRecord
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
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 报损数量
        /// </summary>
        public int Number { get; set; }

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
