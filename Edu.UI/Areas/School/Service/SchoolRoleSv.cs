using Edu.BLL.School;
using Edu.UI.Areas.School.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Edu.Entity;

namespace Edu.UI.Areas.School.Service
{
    using Edu.Entity.Account;
    using Edu.UI.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// role menu relationship management,role management
    /// </summary>
    public class SchoolRoleSv: RoleSv
    {
        //private string _schoolid;
        private  ApplicationDbContext _applicationdbContext;
        private RoleStore<ApplicationRole> _roleStore;


        /// <summary>
        /// change user role 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="roleId"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public AppConfigs.OperResult ChangeUserRole(string uid, string roleId,bool isAdd=true)
        {
            if (string.IsNullOrEmpty(uid) ||string.IsNullOrWhiteSpace(roleId))
            {
                return AppConfigs.OperResult.failDueToArgu;
            }

            ApplicationUser user = UserManager.FindById(uid);
            string[] roles = UserManager.GetRoles(uid).ToArray();

            if (roleId == "200" && roles.Length>0)
            {
                UserManager.RemoveFromRoles(uid, roles);
                return AppConfigs.OperResult.success;
            }

            ApplicationRole rn = RoleManager.Roles.SingleOrDefault(a => a.Id == roleId); //find user role .
            if (isAdd)
            {
                if (user.Roles.All(a => a.RoleId != roleId))
                {
                    UserManager.AddToRole(uid, rn.Name);
                    return AppConfigs.OperResult.success;
                }

                return AppConfigs.OperResult.failDueToExist;
            }
            else
            {
                UserManager.RemoveFromRole(uid, rn.Name);
                return AppConfigs.OperResult.success;
            }

        }
    
        
        /// <summary>
        /// get one user's all role names.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IEnumerable<string> GetRoleNamesUser(ApplicationUser user)
        {
            var s = user.Roles.Select(a => a.RoleId);
            var roleNames = new List<string>();
            if (s != null)
            {
                foreach (var t in s)
                {
                    roleNames.Add(RoleStore.Roles.SingleOrDefault(a=>a.Id==t).Name);
                }

                return roleNames;
            }

            return null;
        }


        public IEnumerable<string> GetSideBarRoles()
        {
            using (_applicationdbContext = new ApplicationDbContext())
            {
                return _applicationdbContext.SideBarRoles.Include(a => a.ApplicationRole)
                    .Select(a => a.ApplicationRole).Select(a=>a.Name).Distinct().ToList();
            }
        }

        /// <summary>
        /// get user roles with select items.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ChangeUserRoleViewModel GetViewModel(string uid)
        {
            ChangeUserRoleViewModel mdl;
            List<SelectListItem> selecteditemsRoles;
            string noAuth = "(无权限)";

            SelectListItem sl = new SelectListItem()
            {
                Selected = true,
                Text =noAuth,
                Value = "200"
            };

            selecteditemsRoles = GetRoleListItem().ToList();
            selecteditemsRoles.Add(sl);

            var u= UserManager.FindById(uid);
            string username=null,roleNameList=null;
           

            if (u != null)
            {
                username = u.UserName;
                var r = GetRoleNamesUser(u);
                if (r != null)
                {
                    selecteditemsRoles.RemoveAll(a=>r.Contains(a.Text));
                    if (selecteditemsRoles.Count == 0)
                    {
                        selecteditemsRoles=new List<SelectListItem>()
                        {
                            new SelectListItem(){Text=noAuth,Value = "-1"}
                        };
                    }
                    roleNameList = string.Join(",",r);
                }
                else
                {
                    roleNameList = noAuth;
                }
               
            }

             
            mdl = new ChangeUserRoleViewModel()
            {
                SelectListRoles =selecteditemsRoles,
                Uid = uid,
                UserName =username,
                RoleList =roleNameList
            };
            return mdl;

        }

        public bool AddUser(NewUserViewModel mdl)
        {
            PasswordHasher hasher=new PasswordHasher();
            
            ApplicationUser user=new ApplicationUser()
            {
                UserName=mdl.UserName,
                PasswordHash= hasher.HashPassword("123456"),
            };

           var r= UserManager.Create(user);

           if (r.Succeeded)
           {
               user = UserManager.FindByName(mdl.UserName);
               ApplicationRole rn = RoleManager.Roles.SingleOrDefault(a => a.Id == mdl.Role);
                UserManager.AddToRole(user.Id, rn.Name);
               return true;
            }
           else
           {
               return false;
           }
         
        }


        public async Task<bool> UserExist(string username)
        {
            var r= await UserManager.FindByNameAsync(username);
            return r!=null;
        }


        public IEnumerable<ApplicationRole> GetUserRoles(string uid)
        {
            using (_applicationdbContext = new ApplicationDbContext())
            {
                _roleStore = new RoleStore<ApplicationRole>(_applicationdbContext);
                return _roleStore.Roles
                    .Include(a => a.SideBarRoles)
                    .Where(a => a.Users.FirstOrDefault(b => b.UserId == uid).UserId == uid)
                    .ToList();
            }
        }

     
        public RoleAspNetUserViewModel GetViewModel(string roleid, out int ttl, int pg = 1)
        {
            IEnumerable<Aspnetuser> mdl;
            SchoolUserBLL userBll=new SchoolUserBLL();
            if (roleid=="-1")
            {
                mdl =userBll.NoRoleUser(out ttl, pg);
            }
            else
            {
                mdl =userBll.QueryByRole(roleid,null,pg,out ttl);
            }
           
            string p = Common.Utility.HtmlPager(10, pg, ttl, 5);
            return new RoleAspNetUserViewModel()
            {
                Aspnetusers=mdl,
                CurrentRole = roleid,
                Pager = p
            };
        }
     


    }
}