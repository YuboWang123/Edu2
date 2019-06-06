using Edu.UI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edu.UI.Areas.School.Models
{
    /// <summary>
    /// school share same menu.
    /// </summary>
    public class ConsoleTopMenu : BaseConsoleMenu
    {
        [StringLength(100)]
        [Display(Name = "控制器名")]
        public string ControllerName { get; set; }
        public ICollection<Module> Modules { get; set; }
    }
  
}
