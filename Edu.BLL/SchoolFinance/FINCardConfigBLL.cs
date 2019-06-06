using Edu.DAL.SchoolFinance;
using Edu.Entity.SchoolFinance;
using System;
using System.Collections.Generic;
using Wyb.DbUtility;

namespace Edu.BLL.SchoolFinance
{
    public class FINCardConfigBLL:IPager<FINCardConfig>
    {
        private readonly FINCardConfigDAL _dal;

        public FINCardConfigBLL()
        {
            _dal = new FINCardConfigDAL();
        }

        public List<FINCardConfig> QueryWithCardCount(string whr, string orderby, int pg, out int ttl, int pgsz)
        {
            return _dal.QueryWithCardCount(whr, orderby, pg, out ttl, pgsz);
        }

        /// <summary>
        /// get all items that could sell.
        /// </summary>
        /// <returns></returns>
        public IList<FINCardConfig> GetAvailableStoreItems(int pg,out int i)
        {
              throw new NotImplementedException();
        }

        public List<FINCardConfig> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            return _dal.Query(whr, orderby, pg, out ttl);
        }

        public FINCardConfig SingleCardConfig(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }
            return _dal.SingleCardConfig(id);
        }

      

    }
}
