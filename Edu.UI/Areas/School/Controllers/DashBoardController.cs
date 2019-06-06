using Edu.BLL.DashBoard;
using Edu.Entity;
using Edu.UI.Areas.School.Models;
using Edu.UI.Areas.School.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace Edu.UI.Areas.School.Controllers
{

    /// <summary>
    /// school menu panel genretor. 
    /// </summary>
    
    public class DashBoardController : SchoolBaseController
    {
        private SchoolSv schoolSv;
        private CommonMenuSv commonMenu;


        public DashBoardController()
        {
            commonMenu = new CommonMenuSv();
            schoolSv=new SchoolSv();
        }
  

        /// <summary>
        /// for common use .
        /// </summary>
        public IEnumerable<ConsoleTopMenu> CommonTopBar
        {
            get
            {
                return commonMenu.GetUserMenus(MyUserId);
            }
        }

     

        /// <summary>
        /// get user's school Id.
        /// </summary>
        public string SchoolID
        {
            get {
                return schoolSv.GetSchoolByUid(MyUserId).SchoolId;
            }
        }



        /// <summary>
        /// entry of school master. 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            List<ConsoleTopMenu> topmenus;
            var rolelist = GetUserRoles();
            topmenus = CommonTopBar.ToList();
            if (rolelist!=null && rolelist.Contains(AppConfigs.AppRole.sys.ToString()) && !topmenus.Any(a => a.Name == "系统管理"))
            {
                topmenus.Add(SchoolMenuSv.GetSysTop());
            }
            return View(topmenus);

        }
     
        /// <summary>
        /// partial page of sidebars of a certain user
        /// </summary>
        /// <param name="topId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SideBar(int topId)
        {
            IEnumerable<Module> sib;
            if (topId == 200)
            {
                sib = SchoolMenuSv.GetSysTop().Modules;
            }
            else
            {
                //get side bars of a user in certain top menu 
                sib = commonMenu.GetUserMenus(MyUserId).SingleOrDefault(a => a.Id == topId).Modules;

            }


            return PartialView(sib);

        }


 

        public ActionResult UserMsgs(int pg=1)
        {
            var bll = new UserMsgBLL();
            int i = 0;
            var mdl= bll.Query(null, null, pg, out i, 10);
            ViewData["pager"] = Common.Utility.HtmlPager(10, pg, i, 20);
            return PartialView(mdl);            
        }


    }
}