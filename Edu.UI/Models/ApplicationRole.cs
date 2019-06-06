using Edu.UI.Areas.School.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Models
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole():base()
        {

        }
        public ApplicationRole(string name) : base(name) { }

        /// <summary>
        /// role maker
        /// </summary>
        public string Maker { get; set; }
        public ICollection<SideBarRole> SideBarRoles { get; set; } = new List<SideBarRole>();

    }
} 