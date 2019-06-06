
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity.TrainBase
{
    public class Base_DataBindBaseModel
    {
        /// <summary>
        /// 学段编号
        /// </summary>
        public string PeriodId { get; set; }
        /// <summary>
        /// 学科编号
        /// </summary>
        public string SubjectId { get; set; }
        /// <summary>
        /// 年级编号
        /// </summary>
        public string GradeId { get; set; }
        public string GenreId { get; set; }
        public string SchoolId { get; set; }

    }

    public class Base_DataBind: Base_DataBindBaseModel,ITester
    {
        /// <summary>
        /// 数据关联主键
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Memo { get; set; }
    }
}
