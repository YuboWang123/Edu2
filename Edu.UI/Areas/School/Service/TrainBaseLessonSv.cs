using Edu.BLL.TrainBase;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Models;
using Edu.UI.Models;
using System;
using System.Data.Entity;
using System.Linq;
using Edu.Entity;

namespace Edu.UI.Areas.School.Service
{

    /// <summary>
    /// trainbase lesson service.
    /// </summary>
    public class TrainBaseLessonSv
    {
        private ApplicationDbContext _db;
        public TrainBaseLessonSv()
        {
            
        }

      
        public AppConfigs.OperResult  AddLesson(TrainBaseLesson trainbaseLesson)
        {
            using (_db = new ApplicationDbContext())
            {
                _db.Entry(trainbaseLesson).State = EntityState.Added;
                return _db.SaveChanges()>0?AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
            }
        }

        /// <summary>
        /// get bindedid by 4 parameters.
        /// </summary>
        /// <param name="whr"></param>
        /// <returns></returns>
        public int GetBindId(string whr)
        {
            string bindid = new Base_DataBindBLL().GetBindingId(whr);
            if (bindid != null)
            {
                return int.Parse(bindid);
            }

            return -1;

        }

        public TrainBaseLesson GetLesson(string k)
        {
            using(_db=new ApplicationDbContext())
            {
                return _db.TrainBaseLessons.Find(k);
            }
        }


        public int Update(TrainBaseLesson trainBaseLesson)
        {
            using(_db=new ApplicationDbContext())
            {
                _db.Entry(trainBaseLesson).State = System.Data.Entity.EntityState.Modified;
                return _db.SaveChanges();
            }
        }

        /// <summary>
        /// del lesson phyically.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int DelLesson(string[] key)
        {
            using (_db = new ApplicationDbContext())
            {
                foreach (var item in key)
                {
                    var hasAny = _db.TrainVcrs.Any(a => a.LessonId == item);
                    if (!hasAny)
                    {
                        var mdl = _db.TrainBaseLessons.Find(item);
                        if (mdl != null)
                        {
                            _db.Entry(mdl).State = EntityState.Deleted;
                        }
                    }
                }
                return _db.SaveChanges();

            }
          

           
            
          
        }
    }
}