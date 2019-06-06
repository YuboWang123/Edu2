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
            if (!context.ConsoleTopMenus.Any(a => a.Name == "ϵͳ����"))
            {
                context.ConsoleTopMenus.AddOrUpdate(t=>t.Name,SchoolMenuSv.GetSysTop());
            }
            if (!context.ConsoleTopMenus.Any(a => a.Name == "��У�γ�"))
            {
                context.ConsoleTopMenus.AddOrUpdate(t => t.Name, SchoolMenuSv.GetSchoolLessonTop());
            }
            if (!context.ConsoleTopMenus.Any(a => a.Name == "�������"))
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

            if (!context.Roles.Any(a => a.Name == "У��"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("У��"));
            }

            if (!context.Roles.Any(a => a.Name == "��ʦ"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("��ʦ"));
            }

            if (!context.Roles.Any(a => a.Name == "Technician"))
            {
                context.Roles.AddOrUpdate(a => a.Name, su.CreateRole("Technician"));
            }

            #endregion

            //su.AddUserToRole("461527@qq.com", "У��").Wait();
            su.AddUserToRole("461527@qq.com", "��ʦ").Wait();
            su.AddUserToRole("wyb", "Technician").Wait();

        }
    }
}
