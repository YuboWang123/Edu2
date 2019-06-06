using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Edu.UI.Areas.School.Models;

public class ConsoleSideMenu : BaseConsoleMenu
{
    public virtual Module Module { get; set; }
    [Required]public int ModuleId { get; set; }

    [Display(Name = "方法名")]
    [StringLength(100)]public string  ActionName { get; set; }
    public ICollection<SideBarRole> SideBarRoles { get; set; } = new List<SideBarRole>();
}
 
