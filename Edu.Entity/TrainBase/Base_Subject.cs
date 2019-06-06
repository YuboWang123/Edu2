using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity.TrainBase
{
    public class Base_Subject:DbBase
    {
        /// <summary>
        /// 学科主键
        /// </summary>
        [Key]
        public override string Id { get; set; }

        /// <summary>
        /// 学科编码
        /// </summary>
        public string SubjectCode { get; set; }
        /// <summary>
        /// 学科名称
        /// </summary>
       [Required]
        [MaxLength(100)]
        [Display(Name = "学科名称")]
        public override string TitleOrName { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        public override int? OrderCode { get; set; }
        /// <summary>
        /// 是否启用（1 启用 0 未启用）默认启用
        /// </summary>
     
        public override bool IsEnabled { get;set; }

        /// <summary>
        /// 请求类型:1同步2复习3试题
        /// </summary>
        // public int RequestType { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
       
        public override string Maker { get;set; }

        /// <summary>
        /// 操作时间
        /// </summary>
     
        public override DateTime MakeDay { get;set; }

        /// <summary>
        /// 备注
        /// </summary>
        
        public override string Memo { get;set; }
        public override bool? IsBasic { get;set; }
        public string SchoolId { get; set; }
    }
}
