using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Edu.BLL.TrainLesson;
using Edu.Entity;
using Edu.UI.Models;

namespace Edu.UI.Controllers
{
    public sealed class DownloadController : BaseController
    {

        public DownloadController():base()
        {
            
        }


        [Authorize]
        [OutputCache(Duration=1200)]
        public ActionResult VcrTestTpl()
        {
            string path = AppConfigs.templatepath;
            string fileName = "testTemplate.docx";
            byte[] fileBs = FileBytes(path);
            bool ok = false;
            if (fileBs != null)
            {
                Session[fileName] = fileBs;
                ok = true;
            }         
            return Json(new { success=ok,fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFile(string filename)
        {
            var sn = Session[filename];
            if (sn == null)
            {
                return new EmptyResult();
            }
            else
            {
                var ms = new MemoryStream((byte[])sn);
                Session[filename] = null;
                return File(ms, MimeMapping.GetMimeMapping(filename), filename);
            }
        
        }


        private byte[] FileBytes(string pth)
        {
            if (Common.Utility.FileExists(pth))
            {               
                FileStream fileStream = new FileStream(Server.MapPath(pth), FileMode.Open);
                byte[] cntByts = new byte[fileStream.Length];
                fileStream.Read(cntByts, 0, cntByts.Length);
                fileStream.Close();
                fileStream.Dispose();

                return cntByts;         
             }
            else
            {
                return null;
            }
        }

        #region front end

        /// <summary>
        /// get resource files ----ajax.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public ActionResult GetResources(string fileId)
        {
            var reslt = new OperResultModel();
            string fileName = string.Empty;

            if (!string.IsNullOrEmpty(fileId))
            {
                if (User.Identity.IsAuthenticated)
                {
                    var mdl = new VcrFileBLL().Single(fileId);
                    if (mdl != null)
                    {
                        fileName = mdl.Name;
                        byte[] fileBs = FileBytes(mdl.Path);

                        if (fileBs != null)
                        {
                            Session[fileName] = fileBs;
                            reslt.OperResult = AppConfigs.OperResult.success;
                            reslt.Message = fileName;
                        }
                    }
                }
                else
                {
                    reslt.OperResult = AppConfigs.OperResult.failDueToAuthen;
                    reslt.Message = "not authened";
                }
            }
            else
            {
                reslt.OperResult = AppConfigs.OperResult.failDueToArgu;
                reslt.Message = "argument null";
            }

            return Json( reslt, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}