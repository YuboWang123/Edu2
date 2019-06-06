using System;
using System.Collections.Generic;
using System.Text;
using Edu.Common;
using Edu.Entity.SchoolFinance;
using Wyb.DbUtility;

namespace Edu.BLL.SchoolFinance
{
    public abstract class FINCardBase<T> : IPager<FINCard>
    {
        public FINCardBase()
        {
            
        }

        public List<FINCard> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            throw new NotImplementedException();
        }
       
    

      
    }
}
