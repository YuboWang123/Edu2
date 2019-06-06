using Edu.BLL.TrainLesson;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using Edu.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Areas.School.Service
{
    public class VcrFilesSv: IVcrFile
    {
        public VcrFilesSv()
        {
          //  vcrTestBLL = new VcrTestBLL();
            _dbFunc = new VcrFileBLL();
        }
        public VcrFilesSv(string vcrid)
        {
            VcrId = vcrid;
        }

        private ApplicationDbContext _dbContext;
      
        private VcrFileBLL _dbFunc;

        public string VcrId { get; }

        public int Add(VcrFile mdl)
        {
            using (_dbContext=new ApplicationDbContext())
            {
                _dbContext.TrainVcrFiles.Add(mdl);
                return _dbContext.SaveChanges();
            }         
        }

        public int Del(string id)
        {
            return _dbFunc.Del(id);
        }

        public int DelFile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return 0;
            }
            VcrFile vcrFile = Single(id);
            if (Common.Utility.FileExists(vcrFile.Path))
            {
                Common.Utility.DeleteFile(vcrFile.Path);
            }
 
            return Del(id);
        }

        public VcrFile Single(string k)
        {
            return _dbFunc.Single(k);
        }

        public int Update(VcrFile mdl)
        {
            return _dbFunc.Update(mdl);
        }

        public IEnumerable<VcrFile> QueryList(string vid)
        {
            throw new NotImplementedException();
        }

        public int FkExists(string k)
        {
            return _dbFunc.FkExists(k);
        }
    }
}