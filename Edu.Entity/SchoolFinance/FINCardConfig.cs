using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using static Edu.Entity.AppConfigs;

namespace Edu.Entity.SchoolFinance
{
    public class FINCardConfig:ITester
    {
        private int _validay;
        [Key] public string Id { get; set; }
        public int Count { get; set; }

        [StringLength(30)]
        [Required,Display(Name = "卡号开头")]
        public string CardPrefix { get; set; } //卡号前缀
        [StringLength(128)]public string Maker { get; set; }
        [StringLength(200)]public string Memo { get; set; }
        public float UnitPrice { get; set; } //单卡面值--month card and year card are 0;

        //开卡后使用期限 天,月，年
        public int? ValidPeriod
        {
            get { return _validay; }
            set
            {
                if (!value.HasValue)
                {
                    value = 10;
                }

                _validay = value.Value;
            }
        }

        /// <summary>
        /// Card Config status set. 
        /// </summary>
        public BatchCardStatus BatchCardStatus { get; set; }
        /// <summary>
        /// config batch type of the cards
        /// </summary>
        public BatchType BatchType { get; set; }

        //状态发生日期
        [ScaffoldColumn(false)]
        public DateTime StatusDay { get; set; }
        public DateTime MakeDay { get; set; }
        [Required, Display(Name = "有效日期")]
        [DataType(DataType.Text), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string Start { get; set; } //有效期日期

        public ICollection<FINCard> FinCards { get; set; }




    }
}
