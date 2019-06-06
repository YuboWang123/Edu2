using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity.TrainLesson
{

    /// <summary>
    /// video record of  lesson .
    /// </summary>
    [Serializable]
    public class Vcr :ITester
    {    
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required]public string LessonId { get; set; } //Fk Id       
        public string VideoPath { get; set; }    
        public  int? OrderCode { get; set; }
        [StringLength(120)]public string UpdateTime { get; set; }
        public bool IsFree { get; set; }
        public int ViewTimes { get; set; }
        public string UpdatedBy { get; set; }
        public bool? FileOk { get; set; }  //File exists.
        public bool HasTest { get; set; }
        //trainer
        public string TrainerId { get; set; } //Trainer UserId.
        [Required,Display(Name = "名称")]
        public  string TitleOrName { get; set; }
        [ScaffoldColumn(false)]
        public  string Maker { get; set; }
        [ScaffoldColumn(false)]
        public DateTime MakeDay { get; set; }
        public  string Memo { get; set; }
        public  bool IsEnabled { get; set; }
        public double Duration{ get; set; }
    }
}
