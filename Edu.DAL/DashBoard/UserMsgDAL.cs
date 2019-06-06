using Edu.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Edu.Entity.Message;
using Wyb.DbUtility;

namespace Edu.DAL.DashBoard
{
    public class UserMsgDAL:BaseDAL
    {
        private DbFunc _dbfunc;

        public UserMsgDAL():base()
        {
            _dbfunc = new DbFunc();
        }

        public int count(string whr)
        {
            _sb = new StringBuilder();
            _sb.Append("select count(*) from UserMsgs");
            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where" + whr);
            }
            _dbfunc.ConnectionString = connstr;

            var ob = _dbfunc.ExecuteScalar(_sb.ToString());

            return int.Parse(ob.ToString());
        }

        public List<UserMessage> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            _sb = new StringBuilder();
            _sb.Append(@"SELECT ROW_NUMBER() over(order by MakeDay desc) od,Id 
                                      , Memo 
                                      , NickName 
                                      , Connects 
                                      , MakeDay 
                                  FROM  UserMsgs ");

            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append("where " + whr);
            }

            StringBuilder sb = new StringBuilder();

            ttl = base.GetRecordCount(_sb.ToString());

            sb.AppendFormat(@" with tmp as (" + _sb + ") select * from tmp where od > {0} and od < {1}", (pg - 1) * pgsz, pg * pgsz);
            _dbfunc.ConnectionString = connstr;
            var dt =this._dbfunc.ExecuteDataTable(sb.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<UserMessage>.FillModel(dt);
            }
            return null;

      
        }
    }
}
