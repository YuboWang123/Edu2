using System;
using System.Collections.Generic;
using System.Linq;
using Edu.BLL.TrainBase;
using Edu.BLL.TrainLesson;
using Edu.Entity.TrainLesson;
using Edu.UI.Models;
using Edu.UI.Models.TrainViewModels;

namespace Edu.UI.Service.VcrService
{
    public class VcrSvc
    {
        private TrainVcrBLL trainVcrBLL;
        private LessonBLL lessonBLL;

        public VcrSvc()
        {
            trainVcrBLL = new TrainVcrBLL();
            lessonBLL = new LessonBLL();
        }

  

        public VcrPlayViewModel GetVcrPlayViewModel(string vcrid,bool authend)
        {
            VcrPlayViewModel vcrPlayViewModel;

            var lsnMdl = lessonBLL.GetByVcrId(vcrid);
            var vcrMdls = GetVcrs(lsnMdl.Id);
       
            var playcontent = GetPlayContent(vcrid);
            var selec = trainVcrBLL.Single(vcrid);
            if (selec.IsFree || authend)
            {
                vcrPlayViewModel = new VcrPlayViewModel
                {
                    LessonInfo = lsnMdl,
                    vcrs = vcrMdls,
                    VcrPlayContent = playcontent,
                    SelectedVcr = selec
                };
            }
            else
            {
                vcrPlayViewModel = new VcrPlayViewModel
                {
                    LessonInfo = lsnMdl,
                    vcrs = vcrMdls,
                    VcrPlayContent = playcontent,
                    SelectedVcr = new Entity.TrainLesson.Vcr()
                };
            }

          
           
            return vcrPlayViewModel;
          
        }

        public VcrPlayContent GetPlayContent(string vcrid)
        {
            var fileMdls = GetVcrFiles(vcrid);
            var testMdls = GetVcrTests(vcrid);

            return new VcrPlayContent
            {
                vcrTests = testMdls,
                vcrFiles = fileMdls
            };
        }

        public int AddVcr(Vcr model)
        {
            using (var t = new ApplicationDbContext())
            {
                t.Entry(model).State = System.Data.Entity.EntityState.Added;
                return t.SaveChanges();
            }
        }

        public int UpdateVcr(Vcr model)
        {
            using (var t = new ApplicationDbContext())
            {
                Entity.TrainLesson.Vcr mdl = t.TrainVcrs.Find(model.Id);
                mdl.TitleOrName = model.TitleOrName;
                mdl.IsEnabled = model.IsEnabled;
                mdl.IsFree = model.IsFree;
                mdl.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd");
                mdl.UpdatedBy = model.UpdatedBy;

                t.Entry(mdl).State = System.Data.Entity.EntityState.Modified;
                return t.SaveChanges();
            }

        }

        public VcrPlayViewModel GetVcrPlayViewModelByLesson(string lessonid)
        {
            var lsnMdl = lessonBLL.Single(lessonid);
            var vcrMdls = GetVcrs(lessonid);
            var testMdls = new List<VcrTest>();
            var fileMdls = new List<VcrFile>();
          
            if (vcrMdls != null&&vcrMdls.Count()>0)
            {
                testMdls = GetVcrTests(vcrMdls.FirstOrDefault().Id).ToList(); 
                fileMdls = GetVcrFiles(vcrMdls.FirstOrDefault().Id).ToList();
            }

            var playcontent = new VcrPlayContent()
            {
                vcrFiles = fileMdls,
                vcrTests = testMdls
            };

            return new VcrPlayViewModel
            {
                LessonInfo = lsnMdl,                
                vcrs = vcrMdls,
                VcrPlayContent=playcontent,
                SelectedVcr = new Entity.TrainLesson.Vcr()
               
            };             
        }

        public string GetVcrPath(string k)
        {
            return trainVcrBLL.GetVcrPath(k);
        }

        public Vcr Single(string id)
        {
            return trainVcrBLL.Single(id);
        }


        /// <summary>
        /// get all vcr list of the certain lesson.
        /// </summary>
        /// <param name="lessonid"></param>
        /// <param name="pg"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        public IEnumerable<Vcr> GetVcrs(string lessonid,int pg,out int ttl)
        {
            string whr = string.Format(" LessonId='{0}' ", lessonid);
            return trainVcrBLL.Query(whr, null, pg, out ttl);           
        }

        public IEnumerable<Vcr> GetVcrs(string lessonid)
        {
            return trainVcrBLL.Query(lessonid)??new List<Vcr>();
        }

        public IEnumerable<VcrTest> GetVcrTests(string vcrId) {

            return new VcrTestBLL().QueryList(vcrId)??new List<VcrTest>();      
        }

        public IEnumerable<VcrFile> GetVcrFiles(string vcrId)
        {
            string whr = string.Format(" vcrid='{0}' ", vcrId);
            return new VcrFileBLL().QueryList(vcrId)??new List<VcrFile>();        
        }


 




    }
}