using Edu.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Areas.School.Models
{
    /// <summary>
    /// role can access menu view model
    /// </summary>
    public class RoleMenuViewModel
    {
        public ICollection<ApplicationRole> Roles { get; set; }
        public ICollection<ConsoleTopMenu> ConsoleTopMenus { get; set; }
        public IEnumerable<int> SideBarId { get; set; }
        public int  UserCount { get; set; }
    }
}