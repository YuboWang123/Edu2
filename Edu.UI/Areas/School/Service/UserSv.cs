using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Edu.UI.Models;

namespace Edu.UI.Areas.School.Service
{
    public class UserSv
    {
        public UserSv()
        {

        }
        public static bool UpdateUserAvatar(string newpath, string MyUserId)
        {
            if (string.IsNullOrWhiteSpace(newpath) || string.IsNullOrWhiteSpace(MyUserId))
            {
                return false;
            }
            using (var db = new ApplicationDbContext())
            {
                var usr = db.Users.Find(MyUserId);
                if (usr != null)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(usr.Avatar)))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(usr.Avatar));
                    }
                    usr.Avatar = newpath;
                    db.Entry(usr).State = EntityState.Modified;
                    return db.SaveChanges() > 0;
                }
            }

            return false;
        }

        private ApplicationDbContext _db;
        public ApplicationUser GetUserByPhone(string phone)
        {
            using(_db=new ApplicationDbContext())
            {
                return _db.Users.Where(a => a.PhoneNumber == phone).FirstOrDefault();
            }
        }

      

        public int TempUserToAspuser()
        {
            throw new NotImplementedException();
        }

    }
}