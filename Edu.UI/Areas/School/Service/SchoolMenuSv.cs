using Edu.Entity;
using Edu.UI.Areas.School.Models;
using Edu.UI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Edu.BLL.School;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace Edu.UI.Areas.School.Service
{

    /// <summary>
    /// system management, role of principal needed.
    /// </summary>
    public class SchoolMenuSv:CommonMenuSv
    {
        private readonly RoleSv _roleStore;
        private readonly ApplicationDbContext _applicationdbContext;

        public SchoolMenuSv()
        {
            _roleStore = new RoleSv();
            _applicationdbContext=new ApplicationDbContext();

        }

        public SchoolMenuSv(string schoolId):this()
        {
            SchoolId = schoolId;
        }

        #region menu


        

        public ConsoleSideBarContentViewModel GetSideBarsWithContent(int contentId)
        {
            using (DbContext = new ApplicationDbContext())
            {
                var cnt = DbContext.ConsoleSideMenus.SingleOrDefault(a=>a.Id==contentId);

                if (cnt != null)
                {
                    ICollection<Module> sdbBars = getUpperSidebars(cnt);

                    return new ConsoleSideBarContentViewModel()
                    {
                        Modules = sdbBars.ToList(),
                        ConsoleSideMenu = cnt
                    };
                }
                else
                {
                    return null;
                }
             

            }
           
        }

        private ICollection<Module> getUpperSidebars(ConsoleSideMenu cnt)
        {
            var upperSidebar = DbContext.Modules.SingleOrDefault(a => a.Id == cnt.ModuleId);

            var sdbBars = GetSingleTopMenu(upperSidebar.consoleTopMenuId).Modules;
              
                    

            return sdbBars;
        }


        public static ConsoleTopMenu GetFinanceTop()
        {
            string MyUserId = "";
            var topMenu = new ConsoleTopMenu()
            {
                Maker = MyUserId,
                Name = "财务管理",
                OrderNo = 200,
                ControllerName = "schoolFinance"
            };
            var consoleSideBars = new List<Module>()
            {
                new Module(topMenu, "充值卡管理")
                {
                    ConsoleSideMenus = new List<ConsoleSideMenu>()
                    {
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="设置",OrderNo=1,ActionName= "index"},
                        
                    }
                },
                new Module(topMenu, "卡片列表")
                {
                    ConsoleSideMenus = new List<ConsoleSideMenu>()
                    {
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="年卡",OrderNo=1,ActionName="periodcard"},
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="充值卡",OrderNo=2,ActionName="index"}
                    }
                }
            };
            topMenu.Modules = consoleSideBars;
            return topMenu;
        }

        public static ConsoleTopMenu GetSchoolLessonTop()
        {
            string MyUserId = "";
            var topMenu = new ConsoleTopMenu()
            {
                Maker = MyUserId,
                Name = "网校课程",
                OrderNo = 200,
                ControllerName = "TrainBase"
            };
            var consoleSideBars = new List<Module>()
            {
                new Module(topMenu, "课程参数")
                {
                    ConsoleSideMenus = new List<ConsoleSideMenu>()
                    {
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="学段管理",OrderNo=1,ActionName= "period"},
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="学科管理",OrderNo=2,ActionName="subject"},
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="年级管理",OrderNo=2,ActionName="grade"},
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="类别管理",OrderNo=2,ActionName="genre"}
                    }
                },
                new Module(topMenu, "课程管理")
                {
                    ConsoleSideMenus = new List<ConsoleSideMenu>()
                    {
                        new ConsoleSideMenu(){ Maker=MyUserId,Name="课程列表",OrderNo=1,ActionName="lesson"},
                         
                    }
                }
            };
            topMenu.Modules = consoleSideBars;
            return topMenu;
        }


        public static ConsoleTopMenu GetSysTop()
        {
            string MyUserId = "";
            var topMenu = new ConsoleTopMenu()
            {
                Id = 200,
                Maker = MyUserId,
                Name = "系统管理",
                OrderNo = 200,
                ControllerName = "schoolsys"
            };
            var consoleSideBars = new List<Module>()
                {
                    new Module(topMenu, "学校账户")
                    {
                        ConsoleSideMenus = new List<ConsoleSideMenu>()
                        {
                            new ConsoleSideMenu(){ Maker=MyUserId,Name="更改密码",OrderNo=1,ActionName= "schoolPwd"},
                            new ConsoleSideMenu(){ Maker=MyUserId,Name="本校资料",OrderNo=2,ActionName="schoolprofile"}
                        }
                    },
                    new Module(topMenu, "系统管理")
                    {
                        ConsoleSideMenus = new List<ConsoleSideMenu>()
                        {
                            new ConsoleSideMenu(){ Maker=MyUserId,Name="菜单管理",OrderNo=1,ActionName="Menus"},
                            new ConsoleSideMenu(){ Maker=MyUserId,Name="用户授权",OrderNo=2,ActionName="roles"}
                        }
                    }
                };
             topMenu.Modules = consoleSideBars;
            return topMenu;
        }

        public int AddTopMenu(ConsoleTopMenu consoleTopMenu)
        {
            using (DbContext = new ApplicationDbContext())
            {
                DbContext.Entry(consoleTopMenu).State = System.Data.Entity.EntityState.Added;
                return DbContext.SaveChanges();
            }
            throw new NotImplementedException();
        }

        public int AddSideBar(Module consoleSideBar)
        {
            using (DbContext = new ApplicationDbContext())
            {
                DbContext.Entry(consoleSideBar).State = System.Data.Entity.EntityState.Added;
                return DbContext.SaveChanges();
            }
            throw new NotImplementedException();
        }


        public int AddSideBarContent(ConsoleSideMenu content)
        {
            using (DbContext = new ApplicationDbContext())
            {
                DbContext.Entry(content).State = System.Data.Entity.EntityState.Added;
                return DbContext.SaveChanges();
            }
            throw new NotImplementedException();
        }

        public int EditTopMenu(ConsoleTopMenu consoleTopMenu)
        {
            using (DbContext = new ApplicationDbContext())
            {
                DbContext.Entry(consoleTopMenu).State = System.Data.Entity.EntityState.Modified;
                return DbContext.SaveChanges();
            }
            throw new NotImplementedException();
        }

        public int EditContent(ConsoleSideMenu content)
        {
            using (DbContext = new ApplicationDbContext())
            {
                DbContext.Entry(content).State =EntityState.Modified;
                return DbContext.SaveChanges();
            }
        }

        public int EditSideBar(Module consoleSideBar)
        {
            using(DbContext=new ApplicationDbContext())
            {
                DbContext.Entry(consoleSideBar).State = EntityState.Modified;
                return DbContext.SaveChanges();
            }
            
        }


    
        /// <summary>
        /// get menu with sidebars
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConsoleTopMenu> MenuAll()
        {
            return base.GetMenus();
        }

    
        public int DelTopMenu(int id)
        {
            using(DbContext=new ApplicationDbContext())
            {
                var mdl = DbContext.ConsoleTopMenus.Find(id);
                if (mdl != null)
                {
                    DbContext.Entry(mdl).State = EntityState.Deleted;
                    return DbContext.SaveChanges();
                }
                return 0;
            }            
        }

        /// <summary>
        /// get and count user with role and ordinary user
        /// </summary>
        /// <returns></returns>
        public RoleMenuViewModel GetAllRoleMenuViewModel()
        {
            var roles = _roleStore.RoleStore.Roles.ToList();
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            int i = 0;
            foreach (var r in roles)
            {
                i += r.Users.Count;
            }

            int s=0;
            new SchoolUserBLL().NoRoleUser(out s, 1);
            return new RoleMenuViewModel()
            {
                ConsoleTopMenus=MenuAll().ToList(),
                Roles=roles,
                SideBarId=null,
                UserCount =s

            };
        }

        /// <summary>
        /// get view model for the role page.
        /// </summary>
        /// <returns></returns>
        public RoleMenuViewModel GetRoleMenuViewModel(string roleid)
        {
            var roles = _roleStore.RoleStore.Roles.ToList();

            var menus =MenuAll();
            IEnumerable<int> selectedId;
            if (string.IsNullOrEmpty(roleid))
            {
                selectedId = GetSelectSideBars(roles.First().Id);
            }
            else
            {
                selectedId = GetSelectSideBars(roleid);
            }

            var vm = new RoleMenuViewModel
            {
                ConsoleTopMenus = menus.ToList(),
                Roles = roles,
                SideBarId = selectedId ?? new List<int>()
            };

            return vm;
        }

        public IEnumerable<int> GetSelectSideBars(string roleid)
        {
            return _applicationdbContext.SideBarRoles
                .Where(a => a.ApplicationRoleId == roleid)
                .Select(a => a.ConsoleSideMenuId);
        }

 


        public int DelSideBar(int id)
        {
            using (DbContext = new ApplicationDbContext())
            {
                var mdl = DbContext.Modules.Find(id);
                if (mdl != null)
                {
                    var contents = DbContext.ConsoleSideMenus.Where(a => a.ModuleId == mdl.Id);
                    if (contents.ToList() != null)
                    {
                        DbContext.ConsoleSideMenus.RemoveRange(contents);
                    }
                  
                    DbContext.Entry(mdl).State =EntityState.Deleted;
                    return DbContext.SaveChanges();
                }
                return 0;
            }
        }

        /// <summary>
        /// del module inside a sidebar.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelSideBarContent(int id)
        {
            using (DbContext = new ApplicationDbContext())
            {
                var mdl = DbContext.ConsoleSideMenus.Find(id);
                if (mdl != null)
                {
                    DbContext.Entry(mdl).State = EntityState.Deleted;
                    return DbContext.SaveChanges();
                }
                return 0;
            }


        }
        #endregion




    }
}