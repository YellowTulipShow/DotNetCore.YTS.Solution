using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YTS.Shop.Models
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class Menus
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
        /// 下级实体列表
        /// </summary>
        [ForeignKey("ParentID")]
        public ICollection<Menus> Belows { get; set; }

        /// <summary>
        /// 命名空间标识
        /// </summary>
        public string NameSpaces { get; set; }

        /// <summary>
        /// 代码标识
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url页面路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHide { get; set; }

        /// <summary>
        /// 排序顺序
        /// </summary>
        public int Ordinal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
