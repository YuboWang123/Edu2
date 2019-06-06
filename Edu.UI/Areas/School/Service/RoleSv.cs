using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edu.Entity;
using Edu.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ApplicationDbContext = Edu.UI.Models.ApplicationDbContext;

namespace Edu.UI.Areas.School.Service
{
    public class RoleSv
    {
        //private readonly UserStore<ApplicationUser> userStore;
        //private readonly UserManager<ApplicationUser> userManager;

        private Lazy<UserManager<ApplicationUser>> _instance =new Lazy<UserManager<ApplicationUser>>(()=> {return  new UserManager<ApplicationUser>(new AppUserStore<ApplicationUser>(new ApplicationDbContext()));});

        //private readonly RoleStore<ApplicationRole> roleStore;

        public RoleSv()
        {
            
        }

        public UserManager<ApplicationUser> UserManager {
            get {
                return _instance.Value;
            }
        }

        public RoleStore<ApplicationRole> RoleStore => new RoleStore<ApplicationRole>(new ApplicationDbContext());

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return new ApplicationRoleManager(RoleStore);
            }
        }

        public bool AddNewRole(string name)
        {
            if (RoleExist(name))
            {
                return true;
            }
            else
            {
                ApplicationRole r=new ApplicationRole(name);
                using (var db = new ApplicationDbContext())
                {
                    db.Roles.Add(r);
                    return db.SaveChanges() > 0;
                }
                //roleStore.Roles
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// remv by rolename.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelRole(string id)
        {
            if (!string.IsNullOrEmpty(id) && RoleStore.Roles.Any(a=>a.Name==id))
            {
                using (var db = new ApplicationDbContext())
                {
                    if (id == AppConfigs.AppRole.sys.ToString())
                    {
                        return 0;
                    }

                    var md = db.Roles.SingleOrDefault(a => a.Name == id);
                    db.Roles.Remove(md);
                    return db.SaveChanges();
                }
             
            }

            return 0;

         
        }

        private bool RoleExist(string name)
        {
            return RoleStore.Roles.Any(a => a.Name == name);
        }

        private bool RoleExist(Guid id)
        {
              return RoleStore.Roles.Any(a => a.Id == id.ToString());
        }


        /// <summary>
        /// get all the role select list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetRoleListItem()
        {
            using (var db = new ApplicationDbContext())
            {
                var x = RoleStore.Roles.ToList();
             
                return x.Select(p => new SelectListItem() {Text = p.Name, Value = p.Id});
               
            }

       
        }


    }
}