using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu.Entity.Account;

namespace Edu.UI.Areas.School.Models
{
    /// <summary>
    /// school sys controller
    /// </summary>
    public class RoleAspNetUserViewModel
    {
        public string  CurrentRole { get; set; }
        public IEnumerable<Aspnetuser> Aspnetusers { get; set; }
        public string Pager { get; set; }
    }



    public class NewUserViewModel
    {
        [Display(Name="授权角色")]public string Role { get; set; }
        [Display(Name = "用户名")][Required][RegularExpression("^[A-Za-z0-9]+$")]
        public string UserName { get; set; }
    }

    /// <summary>
    /// for change user's role.
    /// </summary>
    public class ChangeUserRoleViewModel
    {
        public string UserName { get; set; }
        public string  Uid { get; set; }
        public string RoleList { get; set; }
        public string SelectedRole { get; set; }
        public IEnumerable<SelectListItem> SelectListRoles { get; set; }
        public bool  IsAdd { get; set; }
    }
}