using System;
using System.ComponentModel.DataAnnotations;

namespace Edu.Entity.SchoolClass
{
    public class ClassName
    {
        [Key]public string Id { get; set; }
        public DateTime MakeDay { get; set; }
        public string TeacherId { get; set; }


    }
}