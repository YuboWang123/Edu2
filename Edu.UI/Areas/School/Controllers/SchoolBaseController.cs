using Edu.BLL.TrainLesson;
using Edu.Entity.School;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Edu.UI.Controllers;
using static Edu.Entity.AppConfigs;
 
namespace Edu.UI.Areas.School.Controllers
{

    /// <summary>
    /// all kinds of service property provider. no view needed.
    /// </summary>
    public class SchoolBaseController : BaseController
    {
        #region Class Mbrs
        private readonly SchoolSv _schoolsv;
        private ApplicationSignInManager _signInManager;

        public SchoolBaseController()
        {
            _schoolsv = new SchoolSv();
            BaseSchoolId = "1";
        }

        public string BaseSchoolId { get; set; }


        public SchoolBaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public SchoolEntity School
        {
            get
            {
                return _schoolsv.GetSchoolByUid(MyUserId);
            }
        }
        #endregion



        /// <summary>
        /// _trainlist get text for the partial.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string GetYorN(bool t)
        {
            return t ? "Y" : "N";
        }

        public string GetUName(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return string.Empty;
            }
            if (UserManager.GetRoles(uid).Contains("admin"))
            {
                return "admin";
            }

            var u = UserManager.FindById(uid);
            if (u != null)
            {
                return u.UserName;
            }
            return "匿名";

        }


   
        /// <summary>
        /// base school
        /// </summary>
        /// 
        public SchoolEntity GetBase_School()
        {
            if (BaseSchoolId == null)
                throw new NullReferenceException();
            return _schoolsv.GetBaseSchoolById(BaseSchoolId);           
        }

        public SchoolEntity GetBase_School(string schoolId)
        {
            return _schoolsv.GetBaseSchoolById(schoolId);
        }

 

        /// <summary>
        /// Get file path according the userid
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ForAll">is for all site use</param>
        /// <returns></returns>
        private string GetFilePath(FileType fileType, bool ForAll = false)
        {
            string CommonPath = System.Configuration.ConfigurationManager.AppSettings["Upload"];
            string UserFileName;
            string fileName;
            switch (fileType)
            {
                case FileType.TxtFile:
                    fileName = "Txt";
                    break;
                case FileType.VideoFile:
                    fileName = "Video";
                    break;
                case FileType.ImgFile:
                    fileName = "Image";
                    break;
                default:
                    fileName = "Doc";
                    break;
            }

            if (ForAll)
            {
                UserFileName = CommonPath;
            }
            else
            {
                UserFileName = CommonPath + "/" + MyUserId + "/" +fileName +"/"+ DateTime.Now.Year + "_" + DateTime.Now.Month;
            }

            if (!Directory.Exists(Server.MapPath(UserFileName)))
            {
                Directory.CreateDirectory(Server.MapPath(UserFileName));
            }

            return UserFileName;

        }


        #region Vcr Upload.
        ///upload trainvcr
        /// <summary>
        /// ajax upload image or files, work with plug of fileupload
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetVcr(string vid)
        {
            string error = string.Empty;
            HttpPostedFileBase file = Request.Files[0];
            if (file == null)
            {
                return Json(new { error = "上传失败" }, JsonRequestBehavior.AllowGet);
            }


            if (string.IsNullOrEmpty(vid))
            {
                return Json(new { error = "没有视频ID参数" }, JsonRequestBehavior.AllowGet);
            }
           
            string[] pth = new string[1];
            string userfile = GetFilePath(FileType.VideoFile, false);
            if (userfile != null)
            {
                string part2 = DateTime.Now.ToFileTime() .ToString()+Path.GetExtension(file.FileName);

                string p = Path.Combine(Server.MapPath(userfile), part2);
                string returnPth = userfile + "/" + part2;

                try
                {
                    file.SaveAs(p);
                    pth[0] = returnPth;
                    TrainVcrSv trainVcrSv = new TrainVcrSv(BaseSchoolId);
                    var mdl =trainVcrSv.Single(vid);
                    if (mdl != null)
                    {
                        mdl.VideoPath = returnPth;
                        mdl.Duration =await trainVcrSv.VideoDuration(Common.Utility.GetMapPath(returnPth));
                        trainVcrSv.Update(mdl);
                    }
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

       
        /// <summary>
        /// Upload files of the vcr Resource.
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        public  JsonResult VcrResource(string vid)
        {
            string error = string.Empty;
            HttpPostedFileBase file = Request.Files["resource"];

            if (file == null)
            {
                return Json(new { error = "上传失败" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(vid))
            {
                return Json(new { error = "没有ID参数" }, JsonRequestBehavior.AllowGet);
            }

            string[] pth = new string[1];
            string userfile = GetFilePath(FileType.TxtFile, false);
            if (userfile != null)
            {
                string part2 = DateTime.Now.ToFileTime() + "_" + file.FileName;
                string p = Path.Combine(Server.MapPath(userfile), part2);
                string returnPth = userfile + "/" + part2;

                try
                {
                    file.SaveAs(p);
                    pth[0] = returnPth;

                    //save the model
                    IVcrFile vcrFilesSv = new VcrFilesSv(vid);

                    string fileId = Guid.NewGuid().ToString("n");
                    vcrFilesSv.Add(new VcrFile
                    {
                        Id = fileId,
                        MakeDay = DateTime.Now,
                        Maker = MyUserId,
                        Path = returnPth,
                        VcrId = vid,
                        Name = file.FileName,
                        FileOk = true,
                        FileSize = file.ContentLength
                    });

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

        public JsonResult VcrTest(string vid)
        {
            string error = string.Empty;
            HttpPostedFileBase file = Request.Files[0];
            if (file == null)
            {
                return Json(new { error = "上传失败" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(vid))
            {
                return Json(new { error = "没有ID参数" }, JsonRequestBehavior.AllowGet);
            }


            string[] pth = new string[1];
            string userfile = GetFilePath(FileType.TxtFile, false);
            if (userfile != null)
            {
                string part2 = DateTime.Now.ToFileTime() + "_" + file.FileName;
                string p = Path.Combine(Server.MapPath(userfile), part2);
                string returnPth = userfile + "/" + part2;

                try
                {
                    file.SaveAs(p);
                    pth[0] = returnPth;
                    //save the model  

                    IVcrTest vcrTest = new VcrTestBLL();
                   int i= vcrTest.BulkInsert(p, vid,MyUserId);
                    if (i == 0)
                    {
                        error = "文件无法解析,请下载'Doc模板'";
                        return Json(new { error}, JsonRequestBehavior.AllowGet);
                    }
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




    }
}