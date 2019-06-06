using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Areas.School.Models
{
    /// <summary>
    /// sidebar and single content.
    /// </summary>
    public class ConsoleSideBarContentViewModel
    {
        public IEnumerable<Module> Modules { get; set; }
        public ConsoleSideMenu ConsoleSideMenu { get; set; }
    }
}