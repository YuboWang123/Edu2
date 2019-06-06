using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Edu.BLL.SchoolFinance;
using Edu.BLL.TrainBase;
using Edu.Entity;
using Edu.Entity.TrainBase;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Service;
using Edu.UI.Models;
using Edu.UI.Models.TrainViewModels;
using Edu.UI.Service;
using Edu.UI.Service.VcrService;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Vcr = Edu.Entity.TrainLesson.Vcr;

namespace Edu.UI.Controllers
{

    /// <summary>
    /// basic subject in junior school /middle school
    /// </summary>
    public class LessonController : BaseController
    {
        private BasicDataSvc trainBaseSvc;
        private BasicLessonSvc trainBaseLessonSvc;
       
        public LessonController()
        {
            trainBaseSvc = new BasicDataSvc();
            trainBaseLessonSvc = new BasicLessonSvc();
        }


        /// <summary>
        /// Train lesson Pag.
        /// </summary>
        /// <param name="id">lesson id</param>
        /// <returns></returns>
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("没有输入参数");
            }
            else
            {
                int i = 0;
               
                LessonVcrViewModel lessonVcrViewModel = trainBaseLessonSvc.GetLessonVcrViewModel(id, out i, 1);
                if (lessonVcrViewModel.baseLesson != null)
                {
                    ViewData["pager"] = Common.Utility.HtmlPager(10, 1, i, 20);
                    ViewData["vcrcnt"] = i;

                    if (User.Identity.IsAuthenticated)
                    {
                        lessonVcrViewModel.UserRole = new FINCardBLL().HasCard(MyUserId)?"vip":string.Empty;
                    }
                     

                    return View(lessonVcrViewModel);
                }
                else
                {
                    return View("error");
                }
            }

        }

        public ActionResult VcrList(string fk, int pg = 1)
        {
            int i = 0;
            var mdl = trainBaseLessonSvc.GetVcrs(fk, out i, pg);
            ViewData["pager"] = Common.Utility.HtmlPager(10, pg, i, 20);
            return PartialView(mdl);
        }


        /// <summary>
        /// Train list Header choosing panel. --ajax
        /// </summary>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult List()
        {
            var periods = trainBaseSvc.GetBase_Periods(null);
            return View(periods);
        }


        /// <summary>
        /// get Genre bindings--ajax
        /// </summary>
        /// <param name="last_a">a of the last clicked </param>
        /// <param name="upperids"></param>
        /// <returns></returns>
        [HttpGet]public JsonResult GetBindings(string last_a, string[] upperids)
        {
            var jsn = trainBaseSvc.GetBindings(last_a, upperids);  //get binding data with lesson list
            return Json(new { jsn }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// get trainbaselesson list by binded datas --ajax
        /// </summary>
        /// <param name="bindId"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult LessonList(Base_DataBind datas, int pg = 1)
        {
            var mdl = trainBaseLessonSvc.QueryPagedLessons(datas, pg, 10);
            return PartialView(mdl);
        }

        #region play page

      
       /// <summary>
       /// get vcr path -------------ajax.
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public JsonResult GetPath(string id)
        {
            var rslt = new OperResultModel();
            var vcrSv=new VcrSvc();
            Vcr mdl = vcrSv.Single(id);
            if (mdl != null)
            {
                if (mdl.IsFree)
                {
                    rslt.OperResult = AppConfigs.OperResult.success;
                    rslt.Message = new VcrSvc().GetVcrPath(id);
                }
                else
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        rslt.OperResult = AppConfigs.OperResult.failDueToAuthen;
                        rslt.Message = "not authened";
                    }
                    else
                    {
                        if (new FINCardBLL().HasCard(MyUserId))
                        {
                            rslt.OperResult = AppConfigs.OperResult.success;
                            rslt.Message = new VcrSvc().GetVcrPath(id);
                        }
                        else
                        {
                            rslt.OperResult = AppConfigs.OperResult.failVip;
                            rslt.Message = "need buy vip";
                        }
                    }
                }
            }
            else
            {
                rslt.OperResult = AppConfigs.OperResult.failDueToExist;
                rslt.Message = "not found";
            }
 

         
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// get files and test ---ajax
        /// </summary>
        /// <returns></returns>
        public ActionResult PlayContent(string vcr)
        {
            var mdl = new VcrSvc().GetPlayContent(vcr);
            return PartialView(mdl);
        }

        /// <summary>
        /// get one played.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Play(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id == "undefined")
            {
                throw new ArgumentException("id is null or undefined");
            }

            VcrPlayViewModel vcrPlayViewModel;
            vcrPlayViewModel = new VcrSvc().GetVcrPlayViewModel(id,User.Identity.IsAuthenticated&& User.IsInRole("vip"));
            if (User.Identity.IsAuthenticated)
            {
                vcrPlayViewModel.UserRole = string.Join(",", UserManager.GetRoles(MyUserId));
            }

            return View(vcrPlayViewModel);
        }

        /// <summary>
        /// get all lesson vcrs.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LessonStart(string id)
        {
            VcrPlayViewModel vcrPlayViewModel = new VcrSvc().GetVcrPlayViewModelByLesson(id);
            if (User.Identity.IsAuthenticated)
            {
                vcrPlayViewModel.UserRole = new FINCardBLL().HasCard(MyUserId)?"vip":string.Empty;
            }
            return View("play",vcrPlayViewModel);
        }


        /// <summary>
        /// submit test result.--ajax
        /// </summary>
        /// <param name="vcrid"></param>
        /// <param name="answers"></param>
        /// <returns></returns>
        public ActionResult TestSubmit(string vcrid,List<TestItem> answers)
        {
            var opers=new OperResultModel();
            if (!User.Identity.IsAuthenticated)
            {
                opers.OperResult = AppConfigs.OperResult.failDueToAuthen;
                opers.Message = "not authened";
            }
            else
            {
                if (answers.Count == 0 || string.IsNullOrEmpty(vcrid))
                {
                    opers.OperResult = AppConfigs.OperResult.failDueToArgu;
                    opers.Message = "no answer";
                }
                else
                {
                    opers.OperResult = AppConfigs.OperResult.success;
                    List<string> r = new VcrTestSv(vcrid, answers).GetScoreWithRight();
                    opers.Message = JsonConvert.SerializeObject(r);
                }

               
            }
            
   
            return Json(opers, JsonRequestBehavior.AllowGet);
               
        }


        #endregion

    }
}