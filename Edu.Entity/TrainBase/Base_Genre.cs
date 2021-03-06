﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Edu.Entity.TrainBase
{
    public class Base_Genre: DbBase
    {        
        [Key]public override string Id { get; set; }
        [Required]
        [Display(Name = "类别名称")]       
        public override string TitleOrName { get; set; }
        public override int? OrderCode { get; set; }      
        public override bool IsEnabled { get;set; }       
        public override string Maker { get;set; }     
        public override DateTime MakeDay { get;set; }     
        public override string Memo { get;set; }
        public override bool? IsBasic { get;set; }
        public string SchoolId { get; set; }
    }
}
