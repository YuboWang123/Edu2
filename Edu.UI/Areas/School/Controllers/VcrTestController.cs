using Edu.BLL.TrainLesson;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edu.UI.Areas.School.Controllers
{
    public class VcrTestController : SchoolBaseController
    {
        private readonly VcrTestBLL dbData;
        public VcrTestController()
        {
            dbData = new VcrTestBLL();
        }


        /// <summary>
        /// get test list of a vcr.
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        public ActionResult getList(string vid)
        {
            var mdl =dbData.QueryList(vid);
            return PartialView("vcr_test", mdl);       
        }

        public ActionResult TemplateDoc()
        {
            string doc = AppConfigs.templatepath;

            return View();
        }



        public ActionResult Add(VcrTest mdl)
        {
           int i= dbData.Add(mdl);
           return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Del(string id)
        {
            int r=dbData.Del(id);
            return Json(new { i=r }, JsonRequestBehavior.AllowGet);
        }

        // GET: School/VcrTest
        public ActionResult Index()
        {
            return View();
        }

   
    }
}