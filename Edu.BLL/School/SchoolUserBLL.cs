using Edu.DAL;
using Edu.Entity.Account;
using Edu.Entity.School.SchoolUserModels;
using System;
using System.Collections.Generic;

namespace Edu.BLL.School
{
    public class SchoolUserBLL
    {
        private SchoolUserDAL _dal;

        public SchoolUserBLL()
        {
            _dal = new SchoolUserDAL();
        }

        public List<Aspnetuser> QueryByRole(string roleid, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            string whr=String.Format("RoleId='{0}'",roleid);
            return _dal.QueryByRole(whr, orderby, pg, out ttl, pgsz);
        }

        public IEnumerable<Aspnetuser> NoRoleUser(out int ttl, int pg)
        {
            return _dal.NoRoleUser(out ttl, pg);
        }


        #region basics
  

        public int Update(SchoolTrainer model)
        {
            return Update(model);
        } 
        #endregion
    }
}
