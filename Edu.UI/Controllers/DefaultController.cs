using System;
using System.Web.Mvc;
using Edu.BLL.TrainBase;
using Edu.Entity;
using Edu.UI.Models;
using Edu.UI.Service;
using IPTools.Core;
using log4net;


namespace Edu.UI.Controllers
{
    [Route("Default")]
    public class DefaultController : BaseController
    {
        private LessonBLL lessonBLL;
         
        public DefaultController()
        {
            lessonBLL = new LessonBLL();
        }

        // GET: Default
         [HttpGet]
        public ActionResult Index()
        {
            var mdl = lessonBLL.Query("imagepath is not null and imagepath <>''", "makeDay desc", 4);

            if (mdl != null)
            {
                return View(mdl);
            }
            return View();
            
        }

        //[HttpPost]
        //public ActionResult Contact([Bind(Exclude ="MakeDay,Id")] Entity.UserMsg userMsg)
        //{
        //    int i = 0;
        //    if (ModelState.IsValid)
        //    {
        //        using(var db=new ApplicationDbContext())
        //        {
        //            userMsg.MakeDay = DateTime.Now;
        //            userMsg.Id = Guid.NewGuid().ToString();
        //            db.userMsgs.Add(userMsg);
        //            i= db.SaveChanges();
        //        }
        //    }
        //    return Json(new { i }, JsonRequestBehavior.AllowGet);

        //}

    }
}