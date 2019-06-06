using Edu.BLL.TrainBase;
using Edu.BLL.TrainLesson;
using Edu.Entity.TrainBase;
using Edu.Entity.TrainLesson;
using Edu.UI.Models.TrainViewModels;
using System.Collections.Generic;
using Wyb.DbUtility;

namespace Edu.UI.Service
{

    /// <summary>
    /// basic lesson of the suject data service.
    /// </summary>
    public class BasicLessonSvc
    {
        private LessonBLL _lessonBLL;
        private TrainVcrBLL _vcrBLL;

        public BasicLessonSvc()
        {
            _lessonBLL = new LessonBLL();
            _vcrBLL = new TrainVcrBLL();
        }
        /// <summary>
        /// query lessons by clicked 'a'. 
        /// </summary>
        /// <param name="bindId"></param>
        /// <param name="pg"></param>
        /// <param name="ttl"></param>
        /// <param name="pgsz"></param>
        /// <returns></returns>
        public BindingLessonViewModel QueryPagedLessons(Base_DataBind datas, int pg, int pgsz)
        {
            Base_DataBindBLL bindBll=new Base_DataBindBLL();
            string whr = " schoolid=1";
            int ttl = 0;
            whr += bindBll.ArrayToWhere(datas);
           var mdl=new BindingLessonViewModel()
           {
               TrainBaseLessons=_lessonBLL.Query(whr,pg,out ttl,10),
               Pager = Common.Utility.HtmlPager(10,pg,ttl,5)
           };

           return mdl;
        }


        #region Vcr list
        public LessonVcrViewModel GetLessonVcrViewModel(string k,out int ttl, int pg=1)
        {
            var mdl = _lessonBLL.Single(k);
            var vcrs = GetVcrs(k,out ttl, pg);


            return new LessonVcrViewModel
            {
                baseLesson = mdl,
                vcrs = vcrs
            };
        }


        public IEnumerable<Vcr> GetVcrs(string k,out int ttl,int pg)
        {
            IPager<Vcr> vcrbBLL = new TrainVcrBLL();
            string whr = string.Format("LessonId='{0}' and videopath is not null", k);
            var vcrmdls = vcrbBLL.Query(whr, null, pg, out ttl);
            return vcrmdls;
        }


        #endregion



    }
}