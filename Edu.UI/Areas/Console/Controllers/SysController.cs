using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edu.UI.Areas.Console.Controllers
{

   
    /// <summary>
    /// menu manage,
    /// </summary>
    [Authorize(Roles ="admin")]
    public class SysController : BaseAdminController
    {
        // GET: Console/Sys
        public ActionResult Index()
        {
            return View();
        }     
    }
}