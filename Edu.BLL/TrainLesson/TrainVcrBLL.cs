
using Edu.Common;
using Edu.DAL.TrainLesson;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Text;
using Wyb.DbUtility;

namespace Edu.BLL.TrainLesson
{
    /// <summary>
    /// del not done yet .
    /// </summary>
    public class TrainVcrBLL : IVcrPath, IPager<Vcr>
    {
        private readonly VcrTestBLL vcrTestBLL;
        private readonly VcrDAL vcrDAL;
        private readonly VcrFileBLL vcrFileBLL;

        public TrainVcrBLL()
        {
            vcrTestBLL = new VcrTestBLL();
            vcrFileBLL = new VcrFileBLL();
            vcrDAL = new VcrDAL();
        }

        public int Add(Vcr mdl)
        {
            return vcrDAL.Add(mdl);
        }

        public AppConfigs.OperResult Del(string k)
        {
            if (this.FkExists(k))
            {
                return AppConfigs.OperResult.failDueToFk;
            }
            return vcrDAL.Del(k)>0 ? AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
        }

 
        /// <summary>
        /// delete vcr video only.
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        public bool DelVcrVideo(string vid)
        {
            string pth = VcrPath(vid);
            return Utility.DeleteFile(pth);
        }


        //delete video and update the vcr record.
        public int DelVideo(string id,string username)
        {
            Vcr vcr = Single(id);
            
            if (vcr!=null&&Utility.FileExists(vcr.VideoPath))
            {
                Utility.DeleteFile(vcr.VideoPath);
            }

            vcr.VideoPath = string.Empty;
            vcr.UpdateTime = DateTime.Now.ToString();
            vcr.UpdatedBy = username;
            return Update(vcr);

        }

        /// <summary>
        /// if vcr file exist with fk.
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private int FkExistsFile(string k)
        {
            return vcrFileBLL.FkExists(k);
        }


        private int FkExistsTestForVcr(string vcrId)
        {
            return vcrTestBLL.FkExists(vcrId);
        }

        public bool FkExists(string vcrId)
        {
            return FkExistsFile(vcrId) > 0 || FkExistsTestForVcr(vcrId)>0;
        }

        public List<Vcr> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            return vcrDAL.Query(whr, orderby, pg, out ttl, pgsz);
        }

        public List<Vcr> Query(string lessonid)
        {
            return vcrDAL.Query(lessonid);
        }

        public Vcr Single(string k)
        {
            return vcrDAL.Single(k);
        }

        public string GetVcrPath(string k)
        {
            return vcrDAL.GetVcrPath(k);
        }

        public int Update(Vcr mdl)
        {
            return vcrDAL.Update(mdl);
        }

    
        public IEnumerable<VcrFile> vcrFiles(string vcrId)
        {
            return vcrFileBLL.QueryList(vcrId);            
        }

        public string VcrPath(string vcrId)
        {
           
            return vcrDAL.VcrPath(vcrId);
        }

        /// <summary>
        /// get test list
        /// </summary>
        /// <param name="vcrId"></param>
        /// <returns></returns>
        public IEnumerable<VcrTest> vcrTests(string vcrId)
        {
            return vcrTestBLL.QueryList(vcrId);
        }




       

    }
}
