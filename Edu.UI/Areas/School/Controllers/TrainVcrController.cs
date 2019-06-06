using Edu.BLL.TrainLesson;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Models.VcrViewModels;
using Edu.UI.Areas.School.Service;
using Edu.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Web.Mvc;
using Edu.Entity;
using Edu.UI.Service;
using Edu.UI.Service.VcrService;

namespace Edu.UI.Areas.School.Controllers
{
    public class TrainVcrController : SchoolBaseController
    {
        private readonly TrainVcrBLL _trainVcrBll;

        public TrainVcrController()
        {
            _trainVcrBll = new TrainVcrBLL();

        }



        /// <summary>   
        /// list lesson vcrs and func.
        /// </summary>
        /// <param name="lessonid">lsn id</param>
        /// <param name="lsn">lsn name</param>
        /// <param name="pic">pic url</param>
        /// <returns></returns>
        public ActionResult Index(string lessonid,string lsn,string pic)
        {
            if (string.IsNullOrWhiteSpace(lessonid))
            {
                throw new Exception($"param {nameof (lsn)} is null");
            }

            var mdl = new TrainVcrSv(BaseSchoolId).Query(lessonid,1); //interface not impliment the func

            int i;
            var mdlBypag=new VcrSvc().GetVcrs(lsn,1,out i);
            string pager = Common.Utility.HtmlPager(10, 1, i, 5);
            var vcrModel=new VcrListViewModel()
            {
                LessonId=lessonid,
                LessonName=lsn,
                VcrListPag = new VcrListPag()
                {
                    Pager=pager,
                    Vcrs=mdlBypag,
                },
                Vcrs = mdlBypag
                


            };

            return PartialView(vcrModel);
        }

        public ActionResult IndexLeft(string lsnId,int pg=1)
        {
            if (string.IsNullOrWhiteSpace(lsnId))
            {
                throw new Exception($"param lesson id is null");
            }
            else
            {
                int i = 0;
                var mdl = new VcrSvc().GetVcrs(lsnId, pg, out i);
                string pger = Common.Utility.HtmlPager(10, pg, i, 5);

                return PartialView("trainVcrList",new VcrListPag()
                {
                    Vcrs = mdl,
                    Pager=pger
                });
            }

        }

        /// <summary>
        /// add new vcr record .---------ajax
        /// </summary>
        /// <param name="lsnId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VcrAdd(string lsnId)
        {
            if (string.IsNullOrEmpty(lsnId))
            {
               throw new ArgumentException("参数"+nameof(lsnId)+ "是空");
            }

            return PartialView(
                new Vcr() {
                LessonId = lsnId,
                Maker = MyUserId,
                TrainerId = MyUserId
                });
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "Id,LessonId,VideoPath,IsFree,TitleOrName,IsEnabled")] Vcr mdl)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                mdl.UpdatedBy = MyUserId;
                i = new VcrSvc().UpdateVcr(mdl);
            }

            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VcrAdd([Bind(Exclude = "Maker,TrainerId,Id,UpdateTime,updatedBy,memo")] Vcr mdl)
        {
            if (ModelState.IsValid)
            {
                mdl.Maker = MyUserId;
                mdl.TrainerId = MyUserId;
                mdl.MakeDay = DateTime.Now;
                mdl.Id = Guid.NewGuid().ToString("n");
                mdl.ViewTimes = 0;
                mdl.HasTest = false;
                
                string findalId = string.Empty;
                int  r = new VcrSvc().AddVcr(mdl);

                if (r > 0)
                {
                    findalId = mdl.Id;
                }
                
                return Json(new { i = r ,id=findalId}, JsonRequestBehavior.AllowGet);
              
            }
       
            return Json(new { i = 0 }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Del(string key)
        {
            var i = _trainVcrBll.Del(key);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ajax bulk del video record.
        /// </summary>
        /// <param name="vids"></param>
        /// <returns></returns>
        public ActionResult DelMany(string[] vids)
        {
            int vcr= 0;
            int files = 0;
            int tsts = 0;
            if (vids.Count()> 0)
            {
                IEnumerable<VcrFile> mdl;
                IEnumerable<VcrTest> tst;
                for (int s = 0; s<vids.Count(); s++)
                {
                    mdl = _trainVcrBll.vcrFiles(vids[s]);

                    if(mdl!=null && mdl.Count() > 0)
                    {
                        mdl.ToList().ForEach(a => a.DelFile());
                        files = new VcrFileBLL().BulkDel(mdl.Select(a => a.Id).ToList());
                    }

                    files = mdl==null?0:mdl.Count();
                    tst = _trainVcrBll.vcrTests(vids[s]);                 

                    

                    if(tst!=null && tst.Count() > 0)
                    {
                        tsts = new VcrTestBLL().BulkDel(tst.Select(a => a.Id).ToList());
                    }

                    bool b = new TrainVcrBLL().DelVcrVideo(vids[s]);
                    vcr+= _trainVcrBll.Del(vids[s])==AppConfigs.OperResult.success ?1:0;
                }
            }

            return Json(new { v=vcr,f=files,t=tsts }, JsonRequestBehavior.AllowGet);

        }


      
        /// <summary>
        /// only get vcr record  except for video path.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult Vcredit(string key)
        {
            var mdl = _trainVcrBll.Single(key);
            if (mdl != null)
            {
                return PartialView(mdl);
            }
           throw new InstanceNotFoundException("没有记录");
          
        }

 


        /// <summary>
        /// del the video if there are not relative files.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult DelVideo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }
            string updatedBy = User.Identity.Name;
            int t= _trainVcrBll.DelVideo(id,updatedBy);
            return Json(new { i=t }, JsonRequestBehavior.AllowGet);
           
        }
       


        #region Test Resource & Vcr list
        /// <summary>
        /// get right parital including test vcr and resource in the page of index
        /// VTR=vcr test resource.
        /// will seperate from vcr files and resource CRUD
        /// </summary>
        /// <returns></returns>
        public ActionResult Vtr(string id)
        {
            var VTRModel = new Vcr_Resource_TestViewModel();
            if (string.IsNullOrEmpty(id))
            {
                return PartialView("error");
            }
            else
            {
                VTRModel.VcrData =new KeyValuePair<string, string>(id, _trainVcrBll.VcrPath(id));
                VTRModel.vcrFiles = _trainVcrBll.vcrFiles(id);
                VTRModel.vcrTests = _trainVcrBll.vcrTests(id);
            }

            return PartialView(VTRModel);
        }

        /// <summary>
        /// [
        /// {"Id":"e6c7a71fcab64e16b189e95f9227cf60"
        /// ,"VcrId":"69be581d151744df80a1b5d7a3c3ce28"
        /// ,"Name":"test.docx"
        /// ,"Path":"/Upload/0ff166e4-2d14-47bd-a870-02b4637437b7/Txt/2019_1/131927032924625707_test.docx"
        /// , "MakeDay":"\/Date(1548229692463)\/"
        /// ,"Maker":"0ff166e4-2d14-47bd-a870-02b4637437b7"
        /// ,"FileOk":true
        /// ,"FileSize":11621
        /// }
        /// ]
        /// </summary>
        /// <param name="vcrid"></param>
        /// <returns></returns>
        public ActionResult Vcr_resource(string vcrid)
        {
            var mdl = _trainVcrBll.vcrFiles(vcrid);
            return PartialView(mdl);
        }

        public ActionResult Vcr_test(string vcrid)
        {
            var mdl =_trainVcrBll.vcrTests(vcrid);
            return PartialView(mdl);
        }

      

        #endregion
    }
}