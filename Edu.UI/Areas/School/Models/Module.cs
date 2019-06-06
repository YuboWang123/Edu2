using Edu.Entity;
using Edu.UI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Edu.UI.Areas.School.Models
{

    /// <summary>
    /// sidebar models
    /// in which
    /// topmenu include sidebar
    /// sidebar include module,
    /// module include content.
    /// </summary>
    public class Module 
    {
        public Module()
        {
            
        }
        public Module(ConsoleTopMenu topMenu,string sidebarname)
        {
            consoleTopMenu = topMenu;
            Name = sidebarname;
        }

        [Key]public int Id { get; set; }
        [StringLength(100)]public string  Name { get; set; }
        public int  OrderNo { get; set; }
        public string Maker { get; set; }
        public ConsoleTopMenu consoleTopMenu { get; set; }
        public ICollection<ConsoleSideMenu> ConsoleSideMenus { get; set; }
        [Required] public int consoleTopMenuId { get; set; }
      
    }
}
 
