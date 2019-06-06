using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Models.LiveViewModels;
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
            var liveshows = _hostShowContext.GetShows(out int i);
            string pgr = Common.Utility.HtmlPager(12, 1, i, 5);
            return View(new PagedLiveShow()
            {
                LiveHostShows=liveshows,
                pager = pgr
            });
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