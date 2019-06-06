using Edu.UI.Models;

namespace Edu.UI.Areas.School.Models
{
    public class SideBarRole
    {
        public ApplicationRole ApplicationRole { get; set; }
        public ConsoleSideMenu ConsoleSideMenu { get; set; }
        public string ApplicationRoleId { get; set; }
        public  int ConsoleSideMenuId { get; set; }
        public int SideBarId { get; set; }
    }


}