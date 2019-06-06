using Edu.DAL.TrainLesson;
using Edu.Entity.TrainLesson;
using System.Collections.Generic;

namespace Edu.BLL.TrainLesson
{
    public class VcrFileBLL 
    {
        private readonly VcrFileDAL vcrFileDAL;
        public VcrFileBLL()
        {
            vcrFileDAL = new VcrFileDAL();
        }

   
        /// <summary>
        /// check if the related file record of a vcr.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int FkExists(string k)
        {
            return vcrFileDAL.FkExists(k);
        }

        public int Del(string k)
        {
            return vcrFileDAL.Del(k);
        }


        public int BulkDel(List<string> idlists)
        {
            return vcrFileDAL.BulkDel(idlists.ToArray());
        }
        public IEnumerable<VcrFile> QueryList(string vid)
        {
            return vcrFileDAL.QueryList(vid);
        }

        public VcrFile Single(string k)
        {
            return vcrFileDAL.Single(k);
        }

        public int Update(VcrFile mdl)
        {
            return vcrFileDAL.Update(mdl);
        }
        
    }
}
