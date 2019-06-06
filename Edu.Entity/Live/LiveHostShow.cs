using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity.Live
{
    public class LiveHostShow 
    {
        [Key]public string Id { get;set; }
        [Display(Name="课程题目")]public string TitleOrName { get;set; }
        public DateTime MakeDay { get;set; }
        [Display(Name = "时间")] public string StartDate { get; set; }
        [Display(Name = "时间范围")] public string TimeDuration { get; set; }
        public string UserId { get;set; }
        [Display(Name = "简述")]public string Memo { get;set; } //description
        public bool IsEnabled { get;set; }
        [Display(Name="普通教育学科")]public bool? IsBasic { get;set; }
        
    }
}
