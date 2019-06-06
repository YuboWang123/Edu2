using System;
using Edu.Entity.Live;
using Edu.UI.Service.Live;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Edu.UI.Areas.Live.Controllers
{
    [Authorize]
    public class HostBoardController : Controller
    {
        private HostShowRepository _hostSv;

        public HostBoardController()
        {
            _hostSv = new HostShowRepository();
        }

        public async Task<ActionResult> Index()
        {
            var mdl =await Task.Run(()=> _hostSv.Single(User.Identity.Name)) ;
            return View(mdl??new LiveHostShow());
        }

       
        [HttpPost]
        public ActionResult UpdateOrAdd(LiveHostShow hostShow)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                hostShow.UserId = User.Identity.Name;
                var d = _hostSv.Single(hostShow.UserId);
                if ( d!= null)
                {
                    d.IsBasic = hostShow.IsBasic;
                    d.StartDate = hostShow.StartDate;
                    d.TimeDuration = hostShow.TimeDuration;
                    d.MakeDay=DateTime.Now;
                     i=_hostSv.Update(d);
                }
                else
                {
                    i = _hostSv.Add(hostShow);
                }

            }

            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }




    }
}