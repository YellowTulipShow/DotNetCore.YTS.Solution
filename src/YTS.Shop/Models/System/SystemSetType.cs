using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class SystemSetType
    {
        /// <summary>
        /// ID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public int? ParentID { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值内容
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 解释
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
