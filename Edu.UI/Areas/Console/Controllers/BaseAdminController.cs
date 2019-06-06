
using Edu.Common;
using Edu.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Areas.School.Service;

namespace Edu.UI.Areas.Console.Controllers
{
   public interface IControllerAct<T>
    {
        ActionResult add();
        ActionResult add(T mdl);
        ActionResult del(string key);
        ActionResult update(string key);
        ActionResult update(T mdl);
       
    }


    /// <summary>
    /// base controller of console
    /// </summary>
    public class BaseAdminController : Edu.UI.Controllers.BaseController
    {
        CitySchoolSv citySchoolSv;
        public BaseAdminController()
        {
            citySchoolSv = new CitySchoolSv();
        }
        private ApplicationSignInManager _AdminLoginManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _AdminLoginManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _AdminLoginManager = value;
            }
        }

        #region Logins

        /// <summary>
        ///get Rnd Code
        /// </summary>
        public FileContentResult getRndCode()
        {
            Utility utility = new Utility();
            string code = utility.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes =utility.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home", new { area = "" });
        }


        /// <summary>
        /// Admin login
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated&&User.IsInRole("admin"))
            {
                RedirectToAction("index", "dashborad");
            }      

            return View();
        }   

        [HttpPost]
        public ActionResult Login(LoginViewModel acct)
        {
            var valc = Session["ValidateCode"];

            if (string.IsNullOrEmpty(acct.RndNumber))
            {
                ModelState.AddModelError("RndNumber", "验证码没填!");
                Session["ValidateCode"] = null;
                return View(acct);
            }
            else
            {
                if (!valc.ToString().Equals(acct.RndNumber))
                {
                    ModelState.AddModelError("RndNumber", "验证码填错了!");
                    Session["ValidateCode"] = null;
                    return View(acct);
                }

            }

            if (!ModelState.IsValid)
            {
                return View(acct);
            }

            string userName = acct.Email;
            ApplicationUser appUser=new ApplicationUser();
            if (Regex.IsMatch(userName, @"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$")) //如果是邮件地址
            {
                ApplicationDbContext db = new ApplicationDbContext();
               appUser = db.Users.Where(a => a.Email == userName).FirstOrDefault();
                if (appUser != null)
                {
                    userName = appUser.UserName;
                }

            }

            Task<SignInStatus> result = SignInManager.PasswordSignInAsync(userName, acct.Password, acct.RememberMe, shouldLockout: true);

            if (result.Result == SignInStatus.Success)
            {              
                var role = UserManager.GetRoles(appUser.Id);
                if (role.Contains("admin"))
                {
                    return RedirectToAction("index", "Dashboard");
                }

                if (User.IsInRole("schoolmaster")) ///if principal
                {
                    return Redirect("/school/acct/index");
                 //   return RedirectToAction("index", "/school/acct");
                }
            }
            if (result.Result == SignInStatus.LockedOut)
            {
                ModelState.AddModelError("RndNumber", "用户已被锁定，请5分钟后尝试登陆!");
            }


            return View();
        }
        #endregion

        #region Base Use

        public SelectList GetProvince()
        {
            var prv = citySchoolSv.GetProvince();     //get provinces      
            var f = from p in prv
                    select new
                    {
                        p.id,
                        p.name
                    };

            return new SelectList(f, "id", "name");
        }

        public SelectList GetCities(string pvnId)
        {
            var cits = citySchoolSv.GetCity(pvnId);
            var f = from c in cits
                    select new
                    {
                        c.id,
                        c.name
                    };
           return new SelectList(f, "id", "name");
        }

       public SelectList GetSchoolType()
        {
            var ty = citySchoolSv.GetSchoolType();
            return new SelectList(ty, "Id", "name");
        }

        public SelectList GetDownCountries(string parentid)
        {
            var cn = citySchoolSv.GetDownCountriesMdl(parentid);
            var f = from c in cn
                    select new
                    {
                        c.id,
                        c.name
                    };
            return new SelectList(f, "id", "name");
        }

        #endregion

    }
}