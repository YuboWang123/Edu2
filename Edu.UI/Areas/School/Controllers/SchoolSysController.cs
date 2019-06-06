
using Edu.UI.Areas.School.Models;
using Edu.UI.Areas.School.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Edu.Entity;
using Edu.Entity.School;
using Newtonsoft.Json;


namespace Edu.UI.Areas.School.Controllers
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public class SchoolSysController : SchoolBaseController
    {
        private readonly SchoolMenuSv _sysSv;
        private readonly SchoolRoleSv _roleSv;

        //private RoleSv _baseRoleSv;

        public SchoolSysController()
        {
            _sysSv = new SchoolMenuSv("1"); //school id
                                           
            _roleSv = new SchoolRoleSv();
        }


        #region Role Pages

        /// <summary>
        /// partial page for all roles in menu of sys config
        /// </summary>
        /// <returns></returns>
        public ActionResult Roles()
        {
            var r = _sysSv.GetAllRoleMenuViewModel();
            ViewData["userCount"] = UserStore.Users.Count(); 
            return PartialView(r);
        }

        public int DelRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("参数"+nameof(id)+"是空.");
            }

            return _roleSv.DelRole(id);
            throw new NotImplementedException();
        }

        public int AddRole(string rolename)
        {
            return _roleSv.AddNewRole(rolename) ?1:0;
        }

        /// <summary>
        /// get role sidebars when clicked.--- json
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public JsonResult RoleSidebars(string roleid)
        {
            var list = _sysSv.GetSelectSideBars(roleid);
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// get role users 
        /// </summary>
        /// <param name="roleid">roleid,-1 is general users.</param>
        /// <returns></returns>
        public ActionResult RoleUsers(string roleId,int pg=1)
        {
            int i = 0;
            var mdl = _roleSv.GetViewModel(roleId, out i, pg);
            return PartialView(mdl);
        }

        /// <summary>
        /// user table with certain role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult roleUserTable(string roleId, int pg = 1)
        {
            int i = 0;
            var mdl = _roleSv.GetViewModel(roleId, out i, pg);
            return PartialView(mdl);
        }




        /// <summary>
        /// add new user with user username and role id.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AddUser(NewUserViewModel mdl)
        {
            AppConfigs.OperResult i=AppConfigs.OperResult.failDueToArgu;

            if (ModelState.IsValid)
            {
                //if user already exists.
                if (await _roleSv.UserExist(mdl.UserName))
                {
                    return Json(new { i = AppConfigs.OperResult.failDueToExist }, JsonRequestBehavior.AllowGet);
                }

                i = _roleSv.AddUser(mdl)? AppConfigs.OperResult.success :AppConfigs.OperResult.failUnknown;
                return Json(new { i }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { i },JsonRequestBehavior.AllowGet);
        }


  
        /// <summary>
        /// submit role of the user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserRole(ChangeUserRoleViewModel model)
        {
            var i =_roleSv.ChangeUserRole(model.Uid, model.SelectedRole,model.IsAdd);
            return  Json(new {i},JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get user roles.----ajax
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUserRole(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                throw new ArgumentNullException("没有参数");
            }

            var mdl = _roleSv.GetViewModel(uid);
            return PartialView(mdl);
        }


        /// <summary>
        /// update role's menu---ajax.
        /// </summary>
        /// <param name="sidebar">content id list</param>
        /// <param name="role">role id</param>
        /// <returns></returns>
        public ActionResult UpdateRoleMenus(IList<string> sidebar,string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException(nameof(role)+"  is empty");
            }

            if (sidebar==null)
            {
                throw new ArgumentException(nameof(sidebar) + "  is empty");
            }

            int i = new CommonMenuSv().UpdateMenuForRole(sidebar, role);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
            
        }

        #endregion




        /// <summary>
        /// school change pwd.
        /// login pwd see user center.
        /// </summary>
        /// <returns></returns>
        public ActionResult SchoolPwd()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SchoolPwd(ChangeSchoolPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ///not authened 
            if (!User.IsInRole(AppConfigs.AppRole.sys.ToString())
                &&!User.IsInRole(AppConfigs.AppRole.schoolmaster.ToString()))
            {
                return Json(new { t = AppConfigs.OperResult.failDueToAuthen }, JsonRequestBehavior.AllowGet);
            }

            if (model.OldPassword == model.NewPassword)
            {
                return Json(new { t = AppConfigs.OperResult.failDueToExist }, JsonRequestBehavior.AllowGet);
            }

            var result = await UserManager.ChangePasswordAsync(MyUserId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(MyUserId);
                if (user != null)
                {
                   await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return Json(new { t = AppConfigs.OperResult.success }, JsonRequestBehavior.AllowGet);
              
            }
            AddErrors(result);
            return Json(new {t = AppConfigs.OperResult.failUnknown}, JsonRequestBehavior.AllowGet);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        
        public AppConfigs.OperResult AddSchool(SchoolEntity mdl)
        {
            if (User.IsInRole(AppConfigs.AppRole.sys.ToString()) || User.IsInRole(AppConfigs.AppRole.schoolmaster.ToString()))
            {
                var ss=new SchoolSv();
                var se = ss.GetSchool(MyUserId);
                if (se != null) //already has a school
                {
                    se.MakeDay = DateTime.Now;
                    se.SchoolMasterId = MyUserId;
                    se.SchoolUrl = mdl.SchoolUrl;
                    se.Memo = mdl.Memo;
                    return ss.UpdateSchool(se);
                }
                else
                {
                    mdl.SchoolId = Guid.NewGuid().ToString("n");
                    mdl.Maker = MyUserId;
                    mdl.MakeDay = DateTime.Now;
                    mdl.SchoolMasterId = MyUserId;
                    
                    return ss.AddSchool(mdl);
                }

              
            }

            return AppConfigs.OperResult.failDueToAuthen;
        }

        /// <summary>
        /// ajax update school profile.
        /// </summary>
        /// <returns></returns>
        public ActionResult SchoolProfile()
        {
            var sc=new SchoolSv().GetSchool(User.Identity.GetUserId());
            if (sc != null)
            {
                return PartialView(sc);
            }

            return PartialView(new SchoolEntity(){ Maker= 0.ToString() });
        }



        #region Menus

        #region Add

        [HttpPost]
        public ActionResult AddModule([Bind(Exclude = "Id,Maker")]Module consoleSideBar)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                consoleSideBar.Maker = MyUserId;
                i = _sysSv.AddSideBar(consoleSideBar);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

  
        [HttpPost]
        public ActionResult AddTopMenu([Bind(Exclude = "Maker")]ConsoleTopMenu consoleTopMenu)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                consoleTopMenu.Maker = MyUserId;
                i = _sysSv.AddTopMenu(consoleTopMenu);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSideBarContent([Bind(Exclude = "Maker")]ConsoleSideBarContentViewModel content )
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                content.ConsoleSideMenu.Maker = MyUserId;
                i = _sysSv.AddSideBarContent(content.ConsoleSideMenu);
            }

            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Del

        /// <summary>
        /// del top menu ---ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelTopMenu(int id)
        { 
            int i = _sysSv.DelTopMenu(id);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// del side bar and all of its content navs---ajax.
        /// </summary>
        /// <param name="id">side bar id</param>
        /// <returns></returns>
        public ActionResult DelSideBar(int id)
        {
            int i = _sysSv.DelSideBar(id);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelSideBarContent(int id)
        {
            int i = _sysSv.DelSideBarContent(id);
            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Edit
        /// <summary>
        /// get side barContents  by id---ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditContent(int id)
        {
            if (id == 0)
            {
                throw new ArgumentOutOfRangeException("id 不能为0");
            }
            else
            {
                var mdl = _sysSv.GetSideBarsWithContent(id) ?? new ConsoleSideBarContentViewModel();
                return PartialView(mdl);
            }
        }

        [HttpPost]
        public ActionResult EditContentPost(ConsoleSideMenu ConsoleSideMenu)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = _sysSv.EditContent(ConsoleSideMenu);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get sidebar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditSidebarModule(int id)
        {
            var mdl = _sysSv.GetSingleConsoleSideBar(id);
            return PartialView(mdl);
        }

        [HttpPost]
        public ActionResult EditSidebarModule(Module consoleSideBar)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = _sysSv.EditSideBar(consoleSideBar);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult EditTopMenu(int id)
        {
            var mdl = _sysSv.GetSingleTopMenu(id);
            return PartialView(mdl);
        }

        [HttpPost]
        public ActionResult EditTopMenu([Bind(Exclude = "Maker")]ConsoleTopMenu consoleTopMenu)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                consoleTopMenu.Maker = MyUserId;
                i = _sysSv.EditTopMenu(consoleTopMenu);
            }
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// get js Tree --ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult sideBarTree(int id)
        {
            return PartialView(getModuleSideBars(id));
        }
     

        /// <summary>
        /// pincipal only
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus()
        {
            return PartialView(_sysSv.MenuAll());
        }

        
        /// <summary>
        /// edit consolesidebars and its contents-----------ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult menuSidebarTable(int id)
        {
            //get all module under sidebar.
            return PartialView(getModuleSideBars(id));
        }

        private List<Module> getModuleSideBars(int topid)
        {
            var mdl = _sysSv.GetModules(topid).ToList();
            if (mdl.Count() == 0)
            {
                var top = _sysSv.GetSingleTopMenu(topid);
                mdl = new List<Module>()
                {
                    new Module(){ consoleTopMenu=top }
                };
            }

            return mdl;
        }

        #endregion






    }
}