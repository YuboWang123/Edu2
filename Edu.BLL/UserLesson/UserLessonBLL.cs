using Edu.Entity.UserLesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Edu.DAL.TrainBase;
using Edu.Entity;
using Edu.DAL.UserLesson;

namespace Edu.BLL.UserLesson
{
    public class UserLessonBLL 
    {
        private UserLessonDAL _userLsnDAL;

        public UserLessonBLL()
        {
            _userLsnDAL = new UserLessonDAL();
        }

        public IEnumerable<Edu.Entity.UserLesson.UserLesson> GetList(int pgsz, string whr, out int ttl, string orderby, bool isAsc, int pg=1)
        {
            return _userLsnDAL.GetList(pgsz,whr, out ttl, orderby, isAsc, pg);
        }

    

    }
}
