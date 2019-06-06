using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Edu.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Edu.UI.Models
{
    // 可以通过将更多属性添加到 ApplicationUser 类来为用户添加配置文件数据，请访问 https://go.microsoft.com/fwlink/?LinkID=317594 了解详细信息。
    public class ApplicationUser : IdentityUser,ITester
    {       
        [StringLength(300)]public string Avatar { get; set; }
        [StringLength(128)]public  string Memo { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,string> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
           var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        
            // 在此处添加自定义用户声明
            return userIdentity;
        }
       // public ICollection<ApplicationRole> UserRoles { get; set; }
    }


}