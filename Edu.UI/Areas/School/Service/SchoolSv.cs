using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Edu.Entity;
using Edu.Entity.School;
using Edu.UI.Models;

namespace Edu.UI.Areas.School.Service
{
    public class SchoolSv 
    {
        private ApplicationDbContext _db;

        public SchoolSv()
        {

        }
       
        public SchoolSv(string roleId)
        {
            RoleId = roleId;
        }

        public SchoolSv(string roleId,string schoolId):this(roleId)
        {
            SchoolId = schoolId;
        }

        #region common  

        /// <summary>
        /// full text query user,with information of email ,phone number,username
        /// </summary>
        /// <param name="info">email ,phone number or username</param>
        /// <returns></returns>
        public ApplicationUser FindUserByString(string info)
        {
            using (_db = new ApplicationDbContext())
            {
                return  _db.Users.Where(a => a.PhoneNumber == info || a.UserName==info || a.Email==info).FirstOrDefault(); 
            }
        }



        public AppConfigs.OperResult UpdateSchool(SchoolEntity mdl)
        {
            using(_db=new ApplicationDbContext())
            {
                _db.Entry(mdl).State = EntityState.Modified;
                return _db.SaveChanges()>0?AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
            }
        }


        public AppConfigs.OperResult Del(string key)
        {
            using (_db = new ApplicationDbContext())
            {
                var mdl = _db.Schools.Find(key);
                if (mdl != null)
                {
                    _db.Entry(mdl).State = EntityState.Deleted;
                    return _db.SaveChanges() > 0 ? AppConfigs.OperResult.success : AppConfigs.OperResult.failUnknown;
                }
                else
                {
                    return AppConfigs.OperResult.failDueToExist;
                }
            }

        }

        public  AppConfigs.OperResult AddSchool(SchoolEntity mdl)
        {            
            using (_db=new ApplicationDbContext())
            {
                _db.Entry(mdl).State = System.Data.Entity.EntityState.Added;
                return _db.SaveChanges()>0?AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
            }
        }

        /// <summary>
        /// get school by maker.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public SchoolEntity GetSchool(string uid)
        {
            using (_db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(uid))
                {
                    return _db.Schools.SingleOrDefault(a => a.Maker == uid);
                }

                return null;
            }
        }

        /// <summary>
        /// get a school entity,shandong has no school
        /// </summary>
        /// <returns></returns>
        public SchoolEntity GetSchoolByUid(string masterId)
        {
            using (_db=new ApplicationDbContext())
            {
                if (masterId == "1")
                {
                    return null;
                }

                return _db.Schools.SingleOrDefault( a=> a.SchoolMasterId == masterId);
            }          
        }


        public SchoolEntity GetBaseSchoolById(string key)
        {
           using (_db=new ApplicationDbContext())
            {
                return _db.Schools.Find(key);
            }
        }

        /// <summary>
        /// a bit of slow.
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public SchoolEntity GetSchoolByPhone(string phone)
        {
            using(_db=new ApplicationDbContext())
            {
                var u = _db.Users.Where(a => a.PhoneNumber == phone).FirstOrDefault();
                if (u != null)
                {
                    return _db.Schools.Where(a => a.SchoolMasterId == u.Id).FirstOrDefault();
                }
                return null;
            }
        }      

        #endregion

        public string RoleId { get; set; }
        public IList<string> CanAccessRouteId { get;set; }
        public string CandoContent { get;set; }
        public string SchoolId { get; set; }
 
    }
}