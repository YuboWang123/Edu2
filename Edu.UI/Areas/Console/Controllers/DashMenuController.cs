
using Edu.UI.Areas.School.Models;
using Edu.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edu.UI.Areas.Console.Controllers
{
    /// <summary>
    /// edit menu only.
    /// </summary>
    public class DashMenuController : BaseAdminController
    {
        private IEnumerable<ConsoleTopMenu> _ListConsoleTopMenu=new List<ConsoleTopMenu>();
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        public DashMenuController()
        {
            _ListConsoleTopMenu = applicationDbContext.ConsoleTopMenus.OrderBy(a => a.OrderNo);
        }

        // GET: Console/DashMenu
        public ActionResult Index()
        {
            return View();
        }

        #region TopMenu
        public ActionResult AddTopMenu()
        {
            return PartialView();
        }


        [HttpPost]
        public bool AddTopMenu(ConsoleTopMenu consoleTopMenu)
        {
            int odr = 1;
            if (!int.TryParse(consoleTopMenu.OrderNo.ToString(), out odr))
            {
                consoleTopMenu.OrderNo = odr;
            }

            if (Request.IsAjaxRequest() && ModelState.IsValid)
            {
                applicationDbContext.ConsoleTopMenus.Add(consoleTopMenu);
                return applicationDbContext.SaveChanges() > 0;

            }
            return false;
        }

        public ActionResult EditTopMenu(int topId = 1)
        {
            var top = _ListConsoleTopMenu.Where(a => a.Id == topId).SingleOrDefault();
            return PartialView(top);
        }

        [HttpPost]
        public bool EditTopMenu(ConsoleTopMenu consoleTopMenu)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(consoleTopMenu).State = System.Data.Entity.EntityState.Modified;
                return applicationDbContext.SaveChanges() > 0;
            }
            return false;
        }


        [HttpGet]
        public bool delTopMenu(int topId)
        {
            var topmnu = _ListConsoleTopMenu.Where(a => a.Id == topId).SingleOrDefault();
            if (topmnu != null)
            {
                //var sublist = applicationDbContext.ConsoleSideBar.Where(a => a.consoleTopMenu.Id == topmnu.Id);
                //applicationDbContext.ConsoleSideBar.RemoveRange(sublist);
                //applicationDbContext.ConsoleTopMenu.Remove(topmnu);
                //return applicationDbContext.SaveChanges() > 0;
            }
            return false;
        }

        [HttpGet]
        public bool delSubMenu(int Id)
        {
            var submenu = applicationDbContext.Modules.Find(Id);
            if (submenu != null)
            {
                applicationDbContext.Entry(submenu).State = System.Data.Entity.EntityState.Deleted;
                return applicationDbContext.SaveChanges() > 0;
            }

            return false;
        }
        #endregion



        #region SideBar
        public ActionResult SideBar(int topId = 1)
        {
            var list = applicationDbContext.ConsoleTopMenus.Find(topId).Modules;
            ViewBag.topId = topId;
            ViewBag.topName = _ListConsoleTopMenu.Where(a => a.Id == topId).SingleOrDefault().Name;
            return PartialView(list);
        }

        /// <summary>
        /// Add new sidebar
        /// </summary>
        /// <param name="topId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSideBar(int topId)
        {
            var top = applicationDbContext.ConsoleTopMenus.Find(topId).Modules;
            return PartialView(top);
        }

        [HttpPost]
        public bool AddSideBar([Bind(Exclude = "SideBarSubMenu")]Module sideBar)
        {
            if (ModelState.IsValid)
            {               
                applicationDbContext.Entry(sideBar).State = System.Data.Entity.EntityState.Added;
                return applicationDbContext.SaveChanges() > 0;
            }
            return false;
          
        }

        [HttpGet]
        public ActionResult EditSideBar(int sideId = 1)
        {
            var consoleSideBar = applicationDbContext.Modules.Find(sideId);
            return PartialView(consoleSideBar);
        }

        [HttpPost]
        public bool EditSideBar([Bind(Exclude = "consoleTopMenu")]Module consolesidebar)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Entry(consolesidebar).State = System.Data.Entity.EntityState.Modified;
                return applicationDbContext.SaveChanges() > 0;
            }

            return false;
        }
        #endregion


    }
}