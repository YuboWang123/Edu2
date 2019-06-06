using Edu.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using Edu.UI.Areas.School.Service;

namespace Edu.UI.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            _db = new ApplicationDbContext();
        }
    

        #region Base Mbr
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }
        public static UserStore<ApplicationUser> UserStore
        {
            get { return new UserStore<ApplicationUser>(new ApplicationDbContext()); }
        }

        public RoleManager<ApplicationRole> RoleManager
        {
            get
            {
                return new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new ApplicationDbContext()));
            }
        }

     


        public string GetUserNameByInfo(string userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo)) {
                return _db.Users.Where(a => a.Email == userInfo || a.PhoneNumber == userInfo || a.UserName == userInfo).FirstOrDefault().UserName;
            }
            return null;
        }

        public async  Task<ApplicationUser> GetUserByInfoAsyn(string userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo))
            {
                return await
                    Task.Run(() => _db.Users.SingleOrDefault(a =>
                        a.Email == userInfo || a.PhoneNumber == userInfo || a.UserName == userInfo));

            }
            return null;
        }

        /// <summary>
        /// get all user's roles
        /// </summary>
        /// <returns></returns>
        public  string[] GetUserRoles()
        {
            if(User.Identity.IsAuthenticated)
            return UserManager.GetRoles(this.User.Identity.GetUserId()).ToArray();
            else
            {
                return null;
            }
        }

        /// <summary>
        /// get user's folder when delete  all directory files
        /// </summary>
        /// <returns></returns>
        public string GetUserFolderPath()
        {
            return GetFilePath();
        }

        public string MyUserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return User.Identity.GetUserId();
                }
                return string.Empty;
                    
            }
        }



        /// <summary>
        /// Get image byte with certain size.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="h"></param>
        /// <param name="w"></param>
        public void GetThumbnail(string path, int h, int w)
        {
            if (!string.IsNullOrEmpty(path))
            {
                new WebImage(HttpContext.Server.MapPath(path))
                    .Resize(w, h, false, true) // Resizing the image to 100x100 px on the fly...
                    .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                    .Write();
            }
            // Loading a default photo for realties that don't have a Photo
            new WebImage(HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["defaultThumb"])).Write();
        }

        #endregion      

        #region  Uploads

        /// <summary>
        /// Ajax get image from kindeditor.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEditorFile()
        {
            Hashtable dic = new Hashtable();

            if (Request.Files.Count == 0)
            {
                return Json(new { error = 1, message = "error " }, JsonRequestBehavior.AllowGet);

            }
            string returnpath = FileSave(Request.Files[0]);
            return Json(new { url = returnpath, error = 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ajax save base64 image.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetBase64Avatar(string source)
        {
            KeyValuePair<bool, string> kv;
            if (string.IsNullOrEmpty(source) || !source.StartsWith("data:image/png;base64"))
            {
                kv = new KeyValuePair<bool, string>(false, "parameter error");
                return Json(new { kv }, JsonRequestBehavior.AllowGet);
            }

            int i = source.Length;
            if (i <= 10) //source :'data:,'
            {
                kv = new KeyValuePair<bool, string>(false, "parameter not integrity");
                return Json(new { kv }, JsonRequestBehavior.AllowGet);
            }
            string base64 = source.Substring(source.IndexOf(",") + 1);
            base64 = base64.Trim('\0');
            byte[] chartData = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(chartData, 0, chartData.Length);
            ms.Write(chartData, 0, chartData.Length);
            string folderName = GetFilePath(false);
            string fileName = DateTime.Now.ToFileTime().ToString() + ".png";
            string path = Path.Combine(folderName, fileName);
            Image im = Image.FromStream(ms, true);

            try
            {
                im.Save(Server.MapPath(path), ImageFormat.Png);
                var r= UserSv.UpdateUserAvatar(path, MyUserId);
                kv = new KeyValuePair<bool, string>(r, path);
                im.Dispose();
            }
            catch (Exception ex)
            {
                ms.Close();
                im.Dispose();
                kv = new KeyValuePair<bool, string>(false, ex.ToString());
                return Json(new { kv }, JsonRequestBehavior.AllowGet);

            }

            ms.Close();
            im.Dispose();


            return Json(new { kv }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// ajax upload image or files, work with plug of fileinput
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetFile()
        {
            HttpPostedFileBase file;
            if (Request.Files.Count == 0)
            {
                return Json(new { error = "上传失败" }, JsonRequestBehavior.AllowGet);
            }
            file = Request.Files[0];
            string error = string.Empty;
            string[] pth = new string[1];
            string userfile = GetFilePath(false);
            if (userfile != null)
            {
                string part2 = DateTime.Now.ToFileTime() + "_" + file.FileName;
                
                string p = Path.Combine(Server.MapPath(userfile), part2);
                string returnPth =userfile + "/" + part2;

                try
                {
                    file.SaveAs(p);
                    pth[0] = returnPth;
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                    return Json(new { error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { initialPreview = pth }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = "遇到错误...", initialPreview = "<p>失败，请重试...</p>", }, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Private Fns

        #region file upload

        /// <summary>
        /// save files serving for upload fns with upload file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string FileSave(HttpPostedFileBase file)
        {
            string error = string.Empty;
            string[] pth = new string[1];
            string userfile = GetFilePath(false);
            string part2 = file.FileName;
            string p = Path.Combine(userfile, part2);
         

            try
            {
                file.SaveAs(p);
                pth[0] = p;
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return error;
            }

            return pth[0];

        }


        /// <summary>
        /// Get file path according the userid
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ForAll">is for all site use</param>
        /// <returns></returns>
        private string GetFilePath(bool ForAll = false)
        {
            string CommonPath = System.Configuration.ConfigurationManager.AppSettings["Upload"];
            string UserFileName;

            if (ForAll)
            {
                UserFileName = CommonPath;
            }
            else
            {
                UserFileName = CommonPath+"/" + User.Identity.GetUserId()+"/"+DateTime.Now.Year+"_"+DateTime.Now.Month;
            }

            if (!Directory.Exists(Server.MapPath(UserFileName)))
            {
                Directory.CreateDirectory(Server.MapPath(UserFileName));
            }

            return UserFileName;

        }



        #endregion



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}