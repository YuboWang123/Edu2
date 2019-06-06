using Edu.UI.Areas.School.Service;
using Edu.UI.Models;
using Microsoft.AspNet.Identity;

namespace Edu.UI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Edu.UI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {    
            #region MyRegion
            if (!context.ConsoleTopMenus.Any(a => a.Name == "系统管理"))
            {
                context.ConsoleTopMenus.AddOrUpdate(t=>t.Name,SchoolMenuSv.GetSysTop());
            }
            if (!context.ConsoleTopMenus.Any(a => a.Name == "网校课程"))
            {
                context.ConsoleTopMenus.AddOrUpdate(t => t.Name, SchoolMenuSv.GetSchoolLessonTop());
            }
            if (!context.ConsoleTopMenus.Any(a => a.Name == "财务管理"))
            {
                context.ConsoleTopMenus.AddOrUpdate(t => t.Name, SchoolMenuSv.GetFinanceTop());
            }

       
            var su = new SchoolUserSv();

            if (!context.Users.Any(a => a.UserName == "461527@qq.com"))
            {
                context.Users.AddOrUpdate(t => t.UserName, su.CreateUser("461527@qq.com"));
            }

            if (!context.Roles.Any(a => a.Name == "sys"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("sys"));
            }

            if (!context.Roles.Any(a => a.Name == "校长"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("校长"));
            }

            if (!context.Roles.Any(a => a.Name == "老师"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("老师"));
            }

            if (!context.Roles.Any(a => a.Name == "Technician"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("Technician"));
            }

            #endregion

            //su.AddUserToRole("461527@qq.com", "校长").Wait();
            su.AddUserToRole("461527@qq.com", "老师").Wait();
            su.AddUserToRole("wyb", "Technician").Wait();

        }
    }
}
