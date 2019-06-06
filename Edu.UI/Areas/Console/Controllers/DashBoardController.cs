
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Areas.Console.Services;
using Edu.UI.Areas.School.Models;
using Edu.UI.Areas.School.Service;
using Edu.UI.Models;

namespace Edu.UI.Areas.Console.Controllers
{

    /// <summary>
    /// show menus.
    /// </summary>
    public class DashBoardController:BaseAdminController
    {
        public DashBoardController()
        {

        }

        public IEnumerable<ConsoleTopMenu> TopMenu
        {
            get
            {
                var r = UserManager.GetRolesAsync(MyUserId);

                return new CommonMenuSv().GetAdminTopMenu(MyUserId);
            }
        }

     
        // GET: Console/DashBoard
        public ActionResult Index()
        {
            return PartialView(TopMenu);
        }


        public ActionResult MainContent()
        {
            return PartialView();
        }



    }
}