using System.Linq;
using Edu.Common;
using Edu.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Areas.School.Service;
using WebGrease.Css.Extensions;
using static Edu.Entity.AppConfigs;
using System;

namespace Edu.UI.Areas.School.Controllers
{
    /// <summary>
    /// Login, logout,password,School keeper access only!.
    /// </summary>
    public class AcctController : SchoolBaseController
    {
        public AcctController():base()
        {
            
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
            byte[] bytes = utility.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");

        }

        [HttpGet]
       // [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Acct", new { area = "School" });
        }


        /// <summary>
        /// school admin login
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsRoleOf(MyUserId))
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("No authentication", "没有权限");
            }

            return View();
        }
        


        [HttpPost]
        public async Task<ActionResult> Index(Models.LoginViewModel acct)
        {
            var valc = Session["ValidateCode"];
            if (!ModelState.IsValid)
            {
                return View(acct);
            }

            if (string.IsNullOrEmpty(acct.RndNumber))
            {
                ModelState.AddModelError("RndNumber", "验证码没填!");
                Session["ValidateCode"] = null;
                return View(acct);
            }
            else
            {
                if (valc == null)
                {
                    ModelState.AddModelError("", "状态已过期");
                    return View();
                }

                if (!valc.ToString().Equals(acct.RndNumber))
                {
                    ModelState.AddModelError("RndNumber", "验证码填错了!");
                    Session["ValidateCode"] = null;
                    return View(acct);
                }
            }

            try
            {
                var s = await SignInManager.PasswordSignInAsync(acct.Email, acct.Password, acct.RememberMe, shouldLockout: true);
                if (s == SignInStatus.Success)
                {
                    var user = await GetUserByInfoAsyn(acct.Email);

                    if (IsRoleOf(user.Id))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "School" });
                    }
                    else
                    {
                        return RedirectToAction("index", "Default", new { area = "" });

                    }
                }
                else
                {
                    ModelState.AddModelError("", "登陆失败");
                }

                if (s == SignInStatus.LockedOut)
                {
                    ModelState.AddModelError("", "用户已被锁定，请5分钟后尝试登陆!");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw;
            }

            return View();
        }


        #endregion

        private bool IsRoleOf(string userId)
        {
            var rs=new SchoolRoleSv().GetSideBarRoles();

            var userR = UserManager.GetRoles(userId);

            return rs.Intersect(userR).Any();
        }
     
    }
}
