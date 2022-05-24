using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product
    {
        /// <summary>
        /// ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 产品数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 是否无限数量
        /// </summary>
        public bool IsUnlimitedNumber { get; set; }

        /// <summary>
        /// 是否实物产品
        /// </summary>
        public bool IsPhysicalEntity { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 添加人Id
        /// </summary>
        public int? AddManagerID { get; set; }
    }
}
