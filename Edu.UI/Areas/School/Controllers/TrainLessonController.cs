using Edu.BLL.TrainBase;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Models;
using Edu.UI.Areas.School.Service;
using System;
using System.Web.Mvc;
using Edu.Entity.TrainBase;
using Edu.UI.Areas.School.Models.TrainLessonViewModels;
using Edu.UI.Models;

namespace Edu.UI.Areas.School.Controllers
{

    /// <summary>
    /// lesson program related.
    /// </summary>
    public class TrainLessonController : SchoolBaseController
    {
         
        private readonly LessonBLL _lessonBLL;
        private readonly TrainBaseLessonSv _lessonSv;

        public TrainLessonController()
        {
            _lessonBLL = new LessonBLL();
            _lessonSv = new TrainBaseLessonSv();
        }

        #region Lesson   
        /// <summary>
        /// ipartial page of lesson Tbl
        /// get trainlesson list by 4 arguments.--ajax. 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
       [HttpPost]
        public ActionResult Index(Base_DataBind model,int pg=1)
        {
            if (model == null)
            {
                return Json(new { err = "params null" }, JsonRequestBehavior.AllowGet);
            }

            if (model.GenreId == null || model.GradeId == null || model.PeriodId == null || model.SubjectId == null)
            {
                return PartialView();
            }
            string whr = DataBindToWhere(model);
            
            int i;
            var mdl = _lessonBLL.Query(whr, pg, out i, 10);
            ViewData["pager"] = Common.Utility.HtmlPager(10, pg, i, 5);
            return PartialView("lessonTable",mdl);

        }

        /// <summary>
        /// add new lesson page
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddStart(Base_DataBind model, string info)
        {
            string whr = DataBindToWhere(model);
            int i = _lessonSv.GetBindId(whr);
            if (i == -1)
            {
                throw new Exception("Bind id not found");
            }

            
            var mdl = new TrainLessonViewModel
            {
                IsEdit = false,
                TrainBaseLesson = new TrainBaseLesson()
                {
                    Id=Guid.NewGuid().ToString("n"),
                    Base_DataBindId =i
                },
                LessonInfo = info
            };

            return PartialView("edit" ,mdl);
        }


        /// <summary>
        /// ajax add new lesson.
        /// </summary>
        /// <param name="trainBaseLesson"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Exclude = "Maker,MakeDay,ClickTimes,VideoCount,OrderCode")]TrainBaseLesson trainBaseLesson)
        {
            var result = new OperResultModel();
            if (ModelState.IsValid)
            {               
                trainBaseLesson.MakeDay = DateTime.Now;
                trainBaseLesson.Maker = MyUserId;
                trainBaseLesson.ClickTimes = 0;
                trainBaseLesson.VideoCount = 0;           
                result.OperResult= _lessonSv.AddLesson(trainBaseLesson);
                result.Message = "success";
            }

            if( !_lessonBLL.Exist(trainBaseLesson.Id))
            {
                result.OperResult = AppConfigs.OperResult.failDueToExist;
                result.Message = "record not found ";
            }

            return Json( result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// edit lesson inf.page.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string key)
        {            
                ViewData["id"] = key;
                var m = _lessonSv.GetLesson(key);
                var mdl=new TrainLessonViewModel()
                {
                    LessonInfo="",
                    IsEdit=true,
                    TrainBaseLesson=m
                };
                return PartialView(mdl);
           
        }



        [HttpPost]
        public ActionResult Edit(TrainBaseLesson trainBaseLesson)
        {
            if (trainBaseLesson.Id == null)
            {
                throw new Exception("no id found.");
            }

            if (ModelState.IsValid)
            {
                if (trainBaseLesson.ImagePath == null)
                {
                    trainBaseLesson.ImagePath = AppConfigs.defaultImagePath;
                }
                int i = _lessonSv.Update(trainBaseLesson);
                return Json(new { i }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error="model error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Del()
        {
            return PartialView();
        }

        public ActionResult DelLessonImage(string path)
        {
            int i = 0;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.IndexOf("/Content") != -1)
                {
                    i = 1;
                }
                else
                {
                 
                        Common.Utility.DeleteFile(path); //Delete image file.
                        i = new LessonBLL().DelLessonImage(path);
                
                   
                }
               
            }
            return Json(new { result=i }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ajax bulk del lesson record.
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public ActionResult DelMany(string[] idList)
        {
            int i = 0;
            
             i = _lessonSv.DelLesson(idList);
         
            return Json(new { i }, JsonRequestBehavior.AllowGet);
           
        }

        #endregion




        #region helpers

        public string DataBindToWhere(Base_DataBind mdl)
        {
            string whr = " schoolid=1";
            if (mdl.GradeId != null && mdl.PeriodId != null && mdl.SubjectId != null)
            {
                whr += " and periodid='" + mdl.PeriodId + "'";
                whr += " and gradeid='" + mdl.GradeId + "'";
                whr += " and subjectid='" + mdl.SubjectId + "'";
                whr += " and genreId='" + mdl.GenreId + "'";
            }


            return whr;
        }
        #endregion
    }
}