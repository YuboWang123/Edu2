using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Edu.UI.Areas.School.Models;
using Edu.UI.Models;
using Module = Edu.UI.Areas.School.Models.Module;


namespace Edu.UI.Areas.School.Service
{
    public class CommonMenuSv
    {
        public ApplicationDbContext DbContext;
        private SchoolRoleSv _roleSv;

        private string _userRole;
        public string SchoolId { get; set; }
        public CommonMenuSv()
        {
            _roleSv=new SchoolRoleSv();
        }

        public CommonMenuSv(string rolename)
        {
            _userRole = rolename;
        }


        /// <summary>
        /// get side bar by top menu id. 
        /// Notice: common sidebar of 'wang xiao ke cheng' has make schoolid==1 for all school use,it'll not interupt any funcs due to it in the migration has been initlized.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Module> GetModules(int topId)
        {
            using (DbContext = new ApplicationDbContext())
            {
                return DbContext.Modules
                    .Include(a=>a.ConsoleSideMenus)
                    .Include(a=>a.consoleTopMenu)
                    .Where(a => a.consoleTopMenu.Id == topId)
                    .OrderBy(a => a.OrderNo).ToList();
            }
        }

   
        /// <summary>
        /// get top menu by school.
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        public IEnumerable<ConsoleTopMenu> GetMenus()
        {
            using (DbContext=new ApplicationDbContext())
            {
                var mdl = DbContext.ConsoleTopMenus
                    .Include(a=>a.Modules)
                    .Include("Modules.ConsoleSideMenus")
                    .OrderBy(a => a.OrderNo);


                foreach (var item in mdl.ToList())
                {

                    item.Maker = DbContext.Users.Find(item.Maker)?.UserName;
                }

                return mdl.ToList();
            }
        }

        /// <summary>
        /// get admin top menu.admin top=>(用户管理，角色管理，学校管理，财务管理)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<ConsoleTopMenu> GetAdminTopMenu(string userid)
        {
            using(DbContext=new ApplicationDbContext())
            {
                var mdl= DbContext.ConsoleTopMenus
                    .Where(a => a.Maker == userid).ToList();
                if (mdl.Count != 0)
                    mdl.FirstOrDefault().Modules?.ToList();
                return mdl;
            }
        }



        public ConsoleTopMenu GetSingleTopMenu(int id)
        {
            using (DbContext = new ApplicationDbContext())
            {
                return DbContext.ConsoleTopMenus
                    .Include(a => a.Modules)
                    .Include("Modules.ConsoleSideMenus")
                    .SingleOrDefault(a => a.Id == id);
            }
        }

        public Module GetSingleConsoleSideBar(int id)
        {
            using (DbContext = new ApplicationDbContext())
            {
                return DbContext.Modules
                    .Include(a=>a.ConsoleSideMenus)
                    .SingleOrDefault(a => a.Id == id);
                //throw  new NotImplementedException();
            }
        }

        /// <summary>
        /// delegate get menu.
        /// </summary>
        Func<string, string[], bool> funcRoleMenu = (a, b) =>
          {
              if (!string.IsNullOrEmpty(a))
              {
                  return a.Split(',').Intersect(b) != null;
              }

              //if not define then ,return all.
              return true;
          };

        #region Menu for roles
 
 

        /// <summary>
        /// get top menu with side bar by user id.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IEnumerable<ConsoleTopMenu> GetUserMenus(string userid)
        {
            if (string.IsNullOrEmpty(userid))
            {
                throw new NotImplementedException();
            }

            using (DbContext = new ApplicationDbContext())
            {
                var topMenus = new List<ConsoleTopMenu>();
                var roles = _roleSv.GetUserRoles(userid);

                var sideList = new List<ConsoleSideMenu>();
                if (roles != null)
                {
                    //get all sidebar content of the role
                    foreach (var item in roles)
                    {
                        //得到所有该用户的授权导航。
                        var userSideBarRoles = item.SideBarRoles;
                        if (userSideBarRoles != null)
                        {
                            foreach (var srole in userSideBarRoles)
                            {
                                //get content.
                                var sideMenu = DbContext.ConsoleSideMenus
                                    .Include(a => a.Module.consoleTopMenu)
                                    .Include(a => a.Module)
                                    .SingleOrDefault(a => a.Id == srole.ConsoleSideMenuId);

                                if (sideMenu != null && !sideList.Contains(sideMenu))
                                {
                                    sideList.Add(sideMenu);
                                }
                               
                            }
                        }
                    }


                    if (sideList.Count > 0)
                    {
                        foreach (var item in sideList)
                        {
                            if (!topMenus.Any(a => a.Id == item.Module.consoleTopMenu.Id))
                            {
                                var thisTop = item.Module.consoleTopMenu;
                                var sidebars = sideList.Where(a => a.Module.consoleTopMenu.Id == thisTop.Id).Select(a=>a.Module);

                                foreach (var sideBar in sidebars)
                                {
                                    if (!thisTop.Modules.Any(a => a.Id == sideBar.Id))
                                    {
                                        thisTop.Modules.Add(sideBar);
                                    }
                                }

                                foreach (var insideBar in thisTop.Modules)
                                {
                                    insideBar.ConsoleSideMenus = sideList.Where(a => a.Module.Id == insideBar.Id).ToList();
                                }

                                topMenus.Add(thisTop);  //rebuild top menu
                            }
                        }


                    }


                }
                

                return topMenus;

            }

        }

        /// <summary>
        /// get upper level sidebar list by content ids.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="contentInts"></param>
        /// <returns>{ Key:contentid,Value:sidebarid }</returns>
        private List<KeyValuePair<int ,int>> GetSideBarByContentIds(ApplicationDbContext dbContext,List<int> contentInts)
        {
            List<KeyValuePair<int,int>> sideBarList=new List<KeyValuePair<int,int>>();
            
            foreach (var item in contentInts)
            {
                var cnt = dbContext.ConsoleSideMenus.Find(item);

                var sidebar = dbContext.Modules.SingleOrDefault(a =>
                    a.ConsoleSideMenus.FirstOrDefault(x => x.Id == cnt.Id) != null);
                    
                if (cnt != null &&sidebar!=null)
                {
                    sideBarList.Add(new KeyValuePair<int, int>(item,sidebar.Id));
                }

            }
            return sideBarList;
        }

        /// <summary>
        /// change menu list for a role
        /// </summary>
        /// <param name="contentList"></param>
        /// <returns></returns>
        public int UpdateMenuForRole(IList<string> contentList,string roleid)
        {
            using(DbContext=new ApplicationDbContext())
            {
                var currentRole = DbContext.Roles.Find(roleid);
                if (currentRole == null)
                {
                   throw new ArgumentNullException("no role was found by id: "+nameof(roleid));
                }

                List<int> int_content=new List<int>();
                foreach (var str in contentList)
                {
                    if (str.StartsWith("content") && str.IndexOf("_")!=-1)
                    {
                        int x=Convert.ToInt32(str.Split('_')[1]);
                        int_content.Add(x);
                    }
                }

                //get all the content list by the roleid which existed.
                var list = DbContext.SideBarRoles.Include(a=>a.ConsoleSideMenu).Where(a => a.ApplicationRoleId == roleid);
                if (list != null)
                {
                    //remove all the old record.
                    DbContext.SideBarRoles.RemoveRange(list);
                }

                //add to the db.
                foreach (var kv in GetSideBarByContentIds(DbContext, int_content))
                {
                    DbContext.SideBarRoles.Add(new SideBarRole()
                    {
                        ApplicationRoleId = roleid,
                        ConsoleSideMenuId = kv.Key,
                        SideBarId = kv.Value
                    });
                }

                return DbContext.SaveChanges();


            }

         
        }

        #endregion
    }

}