using Edu.BLL.School;
using Edu.Entity;
using Edu.Entity.CitySchool;
using Edu.Entity.School;
using Edu.UI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Areas.School.Service;

namespace Edu.UI.Areas.Console.Controllers
{
    /// <summary>
    ///  manage shools,create school by admin.
    /// </summary>
    public class SchoolController : BaseAdminController, IControllerAct<SchoolEntity>
    {
        public SchoolController()
        {
            // citySchoolSv = new CitySchoolSv();
            schoolSv = new SchoolSv();
        }

        //  private CitySchoolSv citySchoolSv;

        private SchoolSv schoolSv;

        public ActionResult list()
        {
            ViewData["prvc"] = base.GetProvince();
            ViewData["scType"] = base.GetSchoolType();
            return PartialView();
        }

        public ActionResult add()
        {
            ViewData["prvc"] = base.GetProvince();
            ViewData["scType"] = base.GetSchoolType();
            return PartialView();
        }


        /// <summary>
        /// ajax add new school by admin.
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult add([Bind(Include = "Memo,SchoolmasterPhone,cityId,countryId,provinceId")] SchoolEntity mdl)
        {
            if (ModelState.IsValid)
            {
                mdl.MakeDay = DateTime.Now;
                mdl.Maker = MyUserId;
                mdl.SchoolId = Guid.NewGuid().ToString("n");
                mdl.Payed = false;
                var user= addSchoolMster(mdl.SchoolmasterPhone); //add master user.
                mdl.SchoolMasterId = user.Id;
                var i = schoolSv.AddSchool(mdl);
                return Json(new { i }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { t=AppConfigs.OperResult.failUnknown }, JsonRequestBehavior.AllowGet);
        } 

        /// <summary>
        /// create user of schoolmaster by phone number.
        /// </summary>
        /// <param name="schoolmasterphone"></param>
        /// <returns></returns>
        private ApplicationUser addSchoolMster(string schoolmasterphone)
        {
            var user = new UserSv().GetUserByPhone(schoolmasterphone);
            if (user == null)
            {
                user = new ApplicationUser { Email=schoolmasterphone+"@netedu.com",PhoneNumber=schoolmasterphone,UserName=schoolmasterphone };
                var rs= UserManager.Create(user, "123456");
                if (rs.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "schoolmaster");
                }

            }


            return user;    
        }

        public ActionResult update(string key)
        {
            SchoolEntity school = schoolSv.GetBaseSchoolById(key);
            return PartialView(school);
        }

        [HttpPost]
        public ActionResult update(SchoolEntity mdl)
        {
            var i = schoolSv.UpdateSchool(mdl);
            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult del(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                var i=schoolSv.Del(key);
                return Json(new {i}, JsonRequestBehavior.AllowGet);
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// ajax get cities.
        /// </summary>
        /// <param name="pvnId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult citySelect(string pvnId)
        {
            if (string.IsNullOrEmpty(pvnId))
            {
                throw new ArgumentNullException();
            }
            var cit = new CitySchoolSv().GetCity(pvnId);
            var s = from n in cit
                    select new
                    {
                        n.id,
                        n.name
                    };
            ViewData["cities"] = new SelectList(s, "id", "name");
            return PartialView();
        }


        public ActionResult cityCountrySelect(string cityId)
        {
            if (string.IsNullOrEmpty(cityId))
            {
                throw new ArgumentException();
            }

            var cn = base.GetDownCountries(cityId);
            ViewData["cityCntry"] = cn;
            return PartialView();

        }

    }
}