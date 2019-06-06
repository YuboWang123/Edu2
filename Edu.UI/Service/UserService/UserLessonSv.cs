using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Edu.Entity;
using Edu.Entity.UserLesson;
using Edu.UI.Models;

namespace Edu.UI.Service.UserService
{
    public class UserLessonSv : IDbData<UserLesson>
    {
        private readonly ApplicationDbContext _dbContext;

        public UserLessonSv()
        {
            _dbContext = new ApplicationDbContext();
        }

        public int Add(UserLesson mdl)
        {
            _dbContext.UserLessons.Add(mdl);
            return _dbContext.SaveChanges();
        }

        public async Task<int> AddTask(UserLesson mdl)
        {
            return await Task.Run(() => Add(mdl));
        }
        public int Del(string id)
        {
            var mdl = _dbContext.UserLessons.Find(id);
            _dbContext.Entry(mdl).State = EntityState.Deleted;
            return _dbContext.SaveChanges();
        }

        public UserLesson Single(string k)
        {
          return _dbContext.UserLessons.Find(k);
        }

        public int Update(UserLesson mdl)
        {
            _dbContext.Entry(mdl).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }
    }
}