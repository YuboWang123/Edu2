using System.ComponentModel.DataAnnotations;
using Edu.Entity;

namespace Edu.UI.Areas.School.Models
{
    public class BaseConsoleMenu:IEduMenu
    {
        [Key] public int Id { get; set; }
        [StringLength(100)]
        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        [Display(Name="顺序号")]public int OrderNo { get; set; }
        public string Maker { get; set; }
        public string SchoolId { get; set; }
    }
}