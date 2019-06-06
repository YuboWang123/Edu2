using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Edu.DAL.TrainLesson;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using Wyb.DbUtility;

namespace Edu.BLL.TrainBase
{
    public class LessonBLL 
    {
        private LessonDAL _Dal;

        public LessonBLL()
        {
            _Dal = new LessonDAL();
        }

  

        public bool Exist(string id)
        {
            return _Dal.Exist(id);
        }


        public bool ExistByFk(int bindid)
        {
            return _Dal.ExistByFk(bindid);
        }

        /// <summary>
        /// del prvs image and record in the db.????????????
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int DelLessonImage(string path)
        {            
            return _Dal.DelLessonImage(path);

        }

        public List<TrainBaseLesson> Query(string whr, int pg, out int ttl, int pgsz)
        {
            var mdl = _Dal.Query(whr, pg, out ttl, pgsz);
            return TableToModel<TrainBaseLesson>.FillModel(mdl);
        }

        public IEnumerable<TrainBaseLesson> QueryByBindingId(string bindId, int pg, out int ttl, int pgsz)
        {
            return _Dal.QueryByBindingId(bindId, pg, out ttl, pgsz);
        }

        /// <summary>
        /// index use.
        /// </summary>
        /// <param name="whr"></param>
        /// <param name="orderby"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IEnumerable<TrainBaseLesson> Query(string whr, string orderby, int? top)
        {
            return _Dal.Query(whr, orderby, top);
        }

        public TrainBaseLesson GetByVcrId(string vcrid)
        {
            return _Dal.GetByVcrId(vcrid);
        }

        public TrainBaseLesson Single(string k)
        {
            return _Dal.Single(k);
        }
   
    }
}
