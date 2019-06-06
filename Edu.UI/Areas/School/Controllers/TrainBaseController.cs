using Edu.BLL.TrainBase;
using Edu.Entity.TrainBase;
using Edu.UI.Areas.School.Models;
using Edu.UI.Areas.School.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Edu.Entity;
using PagedList;

namespace Edu.UI.Areas.School.Controllers
{
    using System.Net.Http;

    /// <summary>
    /// basic data controller, period,subject ,gerne,grade at top menu of 网校课程框架数据
    /// 
    /// only admin can have auth to edit basic infos.
    /// </summary>
    [Authorize]
    public class TrainBaseController : SchoolBaseController
    {
        private TrainBaseSv trainBaseSv;
     
        public TrainBaseController():base()
        {
            trainBaseSv = new TrainBaseSv();
        }

        [Authorize]
        public AppConfigs.OperResult DelTrainBase(string id,int type=0)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new HttpRequestException("参数错误");
            }

            AppConfigs.OperResult i = AppConfigs.OperResult.failUnknown;
            TrainbaseRequestType t= (TrainbaseRequestType)type;

            switch (t)
            {
                case TrainbaseRequestType.period:
                    i = this.trainBaseSv.Del<Base_Period>(id);
                    break;
                case TrainbaseRequestType.grade:
                    i = this.trainBaseSv.Del<Base_Grade>(id);
                    break;
                case TrainbaseRequestType.subject:
                    i = this.trainBaseSv.Del<Base_Subject>(id);
                    break;
                case TrainbaseRequestType.genre:
                    i = this.trainBaseSv.Del<Base_Genre>(id);
                    break;
            }

            return i;
        }

        /// <summary>
        /// get reordered list of period,subject,genre,grade.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pg"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public ActionResult _pagelist(TrainbaseRequestType t, int pg, bool asc = true)
        {
            var mdl = trainBaseSv.OrderList((int)t, pg, asc);
            return PartialView(new BaseDataViewModel()
            {
                Datas = mdl,
                IsAsc=asc,
                Type=t
            });
        }

        public ActionResult _trainlist(TrainbaseRequestType t, int pg, bool asc = true)
        {
            var mdl = trainBaseSv.OrderList((int)t, pg, asc);
            return PartialView(new BaseDataViewModel()
            {
                Datas = mdl,
                IsAsc = asc,
                Type = t
            });
        }

        #region period
        public ActionResult period(int pg=1,bool asc=true)
        {
            var list = trainBaseSv.QueryPeriod(pg,asc);

            return PartialView(new BaseDataViewModel()
            {
                Datas = list,
                IsAsc = asc,
                StrColumn=string.Empty,
                Type = TrainbaseRequestType.period
            });
        }

        public ActionResult periodAdd()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult periodAdd(Base_Period mdl)
        {
            mdl.Id = Guid.NewGuid().ToString("n");
            bool t = false;
            if (ModelState.IsValid)
            {
                mdl.IsBasic = false;
                mdl.MakeDay = DateTime.Now;
                mdl.Maker = MyUserId;
                mdl.SchoolId = base.BaseSchoolId;
                int rsl = this.trainBaseSv.AddPeriod(mdl);
                t = rsl > 0;                
            }
            return Json(new { t }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult periodEdit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var m = trainBaseSv.GetBaseT<Base_Period>(id);
                return PartialView("_trainEdit",m);
            }
            else
            {
                throw new ArgumentNullException();
            }
            
        }

        [HttpPost]
        public ActionResult periodEdit(Base_Period mdl)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = trainBaseSv.EditPeriod(mdl);
            }
            return Json(new{i},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult periodDel(string id) {
          
           var t= trainBaseSv.Del<Base_Period>(id);
            return Json(new { t }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region subject
        public ActionResult subject(int pg=1,bool asc=true)
        {
            var list = trainBaseSv.QuerySubject(pg,asc);
            return PartialView(new BaseDataViewModel()
            {
                Datas = list,
                IsAsc = asc,
                StrColumn = string.Empty,
                Type = TrainbaseRequestType.subject
            });
        }
        public ActionResult subjectAdd()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult subjectAdd(Base_Subject mdl) {
            bool t = false;
            mdl.Id = Guid.NewGuid().ToString("n");
            if (ModelState.IsValid)
            {
                mdl.IsBasic = false;
                mdl.MakeDay = DateTime.Now;
                mdl.Maker = MyUserId;
                mdl.SchoolId = base.BaseSchoolId;
                int s = trainBaseSv.AddSubject(mdl);
                t = s > 0;
            }
            return Json(new { t }, JsonRequestBehavior.AllowGet);         
        }
        public ActionResult subjectEdit(string id)
        {
            if (!string.IsNullOrEmpty(id)) {
                var m = trainBaseSv.GetBaseT<Base_Subject>(id);
                return PartialView("_trainEdit",m);
            }
            else
            {
                throw new ArgumentException();
            }
            
        }
        [HttpPost]
        public ActionResult subjectEdit(Base_Subject mdl)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = trainBaseSv.EditSubject(mdl);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult subjectDel(string id)
        {
            
            var t = trainBaseSv.Del<Base_Subject>(id);
            return Json(new { t }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region grade
        public ActionResult grade(int pg=1,bool asc=true)
        {
            var list = trainBaseSv.QueryGrade(pg,asc);
            return PartialView(new BaseDataViewModel()
            {
                Datas = list,
                IsAsc = asc,
                StrColumn = string.Empty,
                Type = TrainbaseRequestType.grade
            });
        }
        public ActionResult gradeAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult gradeAdd(Base_Grade mdl)
        {
            bool t = false;
            mdl.Id = Guid.NewGuid().ToString("n");
            if (ModelState.IsValid)
            {
                mdl.IsBasic = false;
                mdl.MakeDay = DateTime.Now;
                mdl.Maker = MyUserId;
                mdl.SchoolId = base.BaseSchoolId;
                int s = trainBaseSv.AddGrade(mdl);
                t = s > 0;
            }
            return Json(new { t }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult gradeEdit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var m = trainBaseSv.GetBaseT<Base_Grade>(id);
                return PartialView("_trainEdit",m);
            }
            else
            {
                throw new ArgumentNullException();
            }
           
        }

        [HttpPost]
        public ActionResult gradeEdit(Base_Grade mdl)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = trainBaseSv.EditGrade(mdl);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult gradeDel(string id)
        {
           var  t = trainBaseSv.Del<Base_Grade>(id);
            return Json(new { t }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region genre
        public ActionResult genre(int pg = 1,bool asc=true)
        {
            var list = trainBaseSv.QueryGenre(pg,asc);
            return PartialView(new BaseDataViewModel()
            {
                Datas = list,
                IsAsc = asc,
                StrColumn = string.Empty,
                Type = TrainbaseRequestType.genre
            });
        }
        public ActionResult genreAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult genreAdd(Base_Genre mdl)
        {
            bool t = false;
            mdl.Id = Guid.NewGuid().ToString("n");
            if (ModelState.IsValid)
            {
                mdl.IsBasic = false;
                mdl.MakeDay = DateTime.Now;
                mdl.Maker = MyUserId;
                mdl.SchoolId = base.BaseSchoolId;
                int s = trainBaseSv.AddGenre(mdl);
                t = s > 0;
            }
            return Json(new { t }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult genreEdit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var m = trainBaseSv.GetBaseT<Base_Genre>(id);
                return PartialView("_trainEdit",m);
            }
            else
            {
                throw new ArgumentNullException();
            }
           
        }

        [HttpPost]
        public ActionResult genreEdit(Base_Genre mdl)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = trainBaseSv.EditGenre(mdl);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult genreDel(string id)
        {
            var t = trainBaseSv.Del<Base_Genre>(id);
            return Json(new { t }, JsonRequestBehavior.AllowGet);
        }

        #endregion
   
        #region bind
        public ActionResult dataBind()
        {
            DataBindViewModel dataBindViewModel = new DataBindViewModel();
            dataBindViewModel = trainBaseSv.DataBindRaw();
            return PartialView(dataBindViewModel);
        }
        
        /// <summary>
        /// ajax update and add new relation of databinding of the school.
        /// </summary>       
        public ActionResult saveBinding(Base_DataBindViewModel bindingModel)
        {      
            int i = trainBaseSv.SaveBindingData(bindingModel);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// count record deleted.
        /// </summary>
        /// <param name="bindingModel"></param>
        /// <returns></returns>
        private int delBinding(Base_DataBindViewModel bindingModel)
        {          
            int svd = trainBaseSv.DelBinding(bindingModel);           
            return svd;
        }


        /// <summary>
        /// ajax del the binding data when no fk exists.
        /// </summary>
        /// <param name="clickedId"></param>
        /// <param name="upperIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult delBinding(string[] clickedId,string[] upperIds)
        {
            int s = 0;//record count deled.
            string error = string.Empty;
             
            if (clickedId.Length > 0 && upperIds.Length ==3)
            {
                Base_DataBindViewModel baseBinding = new Base_DataBindViewModel
                {
                    PeriodId = upperIds[0],
                    GradeId = upperIds[1],
                    SubjectId = upperIds[2],
                    SchoolId = 1.ToString(),
                    GenreId = clickedId
                };
               s= this.delBinding(baseBinding);
            }
            else
            {
                error = "parameters error";
            }

            return Json(new { t = s ,msg=error }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///  load lower data of one level--in data binding page.
        /// </summary>
        /// <param name="clickedId">last id of 'a' clicked: nj_942ce7a6dab347f8a24b4e3ff6003268</param>
        /// <param name="upperIds">upper a's ids:["abe98d00a0d04854902ac168e010da49"]</param>
        /// <returns></returns>
        public ActionResult loadBinding(string clickedId,string[] upperIds)
        {
            KeyValuePair<string, List<string>> result=new KeyValuePair<string, List<string>>("error",new List<string>());

            if (!string.IsNullOrEmpty(clickedId))  
            {
                var dataBindBLL = new Base_DataBindBLL();
                result = dataBindBLL.GetDownLevel(clickedId, upperIds);
            }     
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region lesson
        public ActionResult lesson() {
            string schoolid = base.BaseSchoolId; //school id
            var bindTop = trainBaseSv.GetDataBindTop();
            return PartialView(bindTop);
        }

        /// <summary>
        /// get all down grade binding datas in the lesson partial page.
        /// with binding.js
        /// </summary>
        /// <param name="last_a">xd_1</param>
        /// <param name="upperids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult lessonBindingData(string last_a,string[] upperids)
        {
           var bindBll = new  Base_DataBindBLL(BaseSchoolId);
            var jsn = bindBll.GetDownLevels(last_a, upperids);       
            return Json(new { jsn }, JsonRequestBehavior.AllowGet);
        }
       
        #endregion



        
    }
 
}