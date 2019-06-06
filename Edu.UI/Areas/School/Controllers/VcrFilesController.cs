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

    /// <summary>
    /// files of a training vcr.
    /// </summary>
    public class VcrFilesController : SchoolBaseController, IActionDb<VcrFile>
    {
        private IDbData<VcrFile> dbData;

        public VcrFilesController()
        {
            dbData = new VcrFilesSv();
        }

        public ActionResult getList(string vid)
        {
            throw new NotImplementedException();
        }


        public ActionResult Add(VcrFile mdl)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public ActionResult Del(string id)
        {
            int r = new VcrFilesSv().DelFile(id);
            return Json(new {i=r}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Single(string key)
        {
            throw new NotImplementedException();
        }

        public ActionResult Update(VcrFile mdl)
        {
            throw new NotImplementedException();
        }
    }
}