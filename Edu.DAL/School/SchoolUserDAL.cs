
using Edu.Entity.Account;
using Edu.Entity.School.SchoolUserModels;
using System;
using System.Collections.Generic;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL
{
    public class SchoolUserDAL:BaseDAL
    {
        private DbFunc _dbFunc;

        public SchoolUserDAL():base()
        {
            _dbFunc=new DbFunc();
        }

        /// <summary>
        /// get general user list.
        /// </summary>
        /// <param name="whr"></param>
        /// <param name="orderby"></param>
        /// <param name="pg"></param>
        /// <param name="ttl"></param>
        /// <param name="pgsz"></param>
        /// <returns></returns>
        public List<Aspnetuser> QueryByRole(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            _sb=new StringBuilder();
            StringBuilder sb = new StringBuilder();

            sb.Append(@"select row_number() over (order by Avatar) od,
                                       u.Id,
                                    Avatar,
                                    UserName, 
                                    PhoneNumber,
                                    Email                               
                                    from AspNetUserRoles r
                                    join AspNetUsers u
                                    on r.UserId=u.Id");

            sb.Append(" where " + whr);

            ttl = GetRecordCount(sb.ToString());

            _sb.Append("with tmp as(" + sb);

            _sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);
            _dbFunc.ConnectionString = connstr;
            System.Data.DataTable dt = _dbFunc.ExecuteDataTable(_sb.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Aspnetuser>.FillModel(dt);
            }
            return null;
        
        }


        public IEnumerable<Aspnetuser> NoRoleUser(out int ttl,int pg)
        {
            _sb = new StringBuilder();
            _sb.Append(@"select row_number() over (order by Avatar) od,
                                       u.Id,
                                    Avatar,
                                    UserName, 
                                    PhoneNumber,
                                    Email
                                    from AspNetUsers u
                                    left join AspNetUserRoles ur
                                    on
                                    u.Id=ur.userid
                                    where ur.RoleId is null");
            ttl = GetRecordCount(_sb.ToString());

            StringBuilder sb = new StringBuilder();
            sb.Append("with tmp as(" + _sb);
            sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);
            _dbFunc.ConnectionString = connstr;
            System.Data.DataTable dt = _dbFunc.ExecuteDataTable(sb.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Aspnetuser>.FillModel(dt);
            }
            return null;
        }
    }
}
