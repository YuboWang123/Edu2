using Edu.BLL.School;
using Edu.Entity.Account;
using Edu.Entity.School.SchoolUserModels;
using System.Collections.Generic;
using Edu.Entity;
using System;
using System.Web;
using Edu.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;

namespace Edu.UI.Areas.School.Service
{
    /// <summary>
    /// users in school: teachers and students
    /// </summary>
    public class SchoolUserSv
    {
        private readonly SchoolUserBLL _bll;

        private RoleSv _roleSv;

        public SchoolUserSv()
        {
            _bll = new SchoolUserBLL();
            _roleSv=new RoleSv();
        }

        public ApplicationUser CreateUser(string name)
        {
            PasswordHasher passwordHasher=new PasswordHasher();

            return new ApplicationUser()
            {
                Email = name,
                PasswordHash = passwordHasher.HashPassword("123456"),
                UserName=name,
                EmailConfirmed=true
            };
        }

        public async Task<bool> AddUserToRole(string user, string role)
        {
            var r =await _roleSv.RoleManager.FindByNameAsync(role);

           var u=await  _roleSv.UserManager.FindByNameAsync(user);

           if (r == null)
           {
               //add new role
               _roleSv.AddNewRole(role);
           }

           if (_roleSv.UserManager.GetRoles(u.Id).Contains(role))
           {
               return true;
           }

          var rs=await  _roleSv.UserManager.AddToRoleAsync(u.Id, r.Name);
          return rs == IdentityResult.Success;

        }



        public ApplicationRole CreateRole(string name)
        {
            return  new ApplicationRole()
            {
                Maker="App maker",
                Name=name
            };
        }
       
     
    }
}