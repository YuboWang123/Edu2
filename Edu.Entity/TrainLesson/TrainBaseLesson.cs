using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Edu.Entity.TrainBase;

namespace Edu.Entity.TrainLesson
{
    /// <summary>
    /// a set of videos:lsn record.
    /// </summary>
    public class TrainBaseLesson : DbBase
    {
        private decimal? _disc;

        [Key]
        public override string Id { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = "课程名称")]
        public override string TitleOrName { get; set; }
        public string ImagePath { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        [Display(Name = "价格")]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "优惠价格")]
        public decimal? DiscountPrice {
            get { return _disc; }
            set
            {
                if (!value.HasValue)
                {
                    value = 0;
                }

                _disc = value;
            }
        }
        [ScaffoldColumn(false)]
        public override DateTime MakeDay { get; set; }
        public Base_DataBind BaseDataBind { get; set; }
        [ScaffoldColumn(false)]
        [Required]
        public int Base_DataBindId { get; set; } //FK from base_databind
        public int ClickTimes { get; set; }
        public int VideoCount { get; set; }
        public override string Maker { get; set; } //userid
        public override string Memo { get; set; }
        public override bool IsEnabled { get; set; }
        public override int? OrderCode { get; set; }
        public override bool? IsBasic { get; set; }
    }
}
