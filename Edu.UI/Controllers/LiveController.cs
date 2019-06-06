using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Service.Live;

namespace Edu.UI.Controllers
{
    public class LiveController : Controller
    {
        private HostShowRepository _hostShowContext;

        public LiveController()
        {
            _hostShowContext = new HostShowRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// show live page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Show(string id)
        {
            var mdl = _hostShowContext.Single(id);
            if (mdl != null)
            {
                return View(mdl);
            }
            return View();
        }
    
    }
}