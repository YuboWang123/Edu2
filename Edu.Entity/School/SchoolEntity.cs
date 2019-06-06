using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Edu.Entity.School
{
    /// <summary>
    ///不按学校 估计用不到了。
    /// </summary>
    public class SchoolEntity:ITester
    {
        [Key]public string SchoolId { get; set; } //with table base_school : 1 to 1
        public string SchoolMasterId { get; set; } //aspnetuser id.
        [StringLength(100)][Required][Display(Name = "学校名称")]public string SchoolName { get; set; }
        [StringLength(50)] [Required][Display(Name = "地址/简介")]public string Memo { get; set; }
      
        [StringLength(8)][RegularExpression("/^[0-9]{6}$/ ")]public string PostCode { get; set; }
        public string Maker { get; set; } //maker id
        public string SchoolUrl { get; set; } //main page of the school      
        public bool Payed { get; set; }
        public decimal PayTtl { get; set; }
        public DateTime MakeDay { get; set; }
        public DateTime? PayDay { get; set; }
        public DateTime? EndDay { get; set; }
        public bool IsEnabled { get; set; }
        //public CitySchool.CitySchool CitySchool { get; set; }
        //public string CitySchoolId { get; set; }
        public int sctype { get; set; }
        [StringLength(15)]public string SchoolmasterPhone { get; set; }

       
    }
}
