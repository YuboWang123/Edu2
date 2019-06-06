using System;
using System.Collections.Generic;
using System.Text;
using Edu.Entity.SchoolFinance;
using Wyb.DbUtility;

namespace Edu.DAL.SchoolFinance
{
    public class FINCardConfigDAL: BaseDAL,IPager<FINCardConfig>
    {
        private DbFunc _dbFunc;

        public FINCardConfigDAL():base()
        {
            _dbFunc=new DbFunc();
        }


        public int Add(FINCardConfig config)
        {
            throw  new NotImplementedException();
        }

        public FINCardConfig SingleCardConfig(string id)
        {
            _sb=new  StringBuilder();
            _sb.Append(@"SELECT Id
                , Count
                , CardPrefix
                , Maker
                , Memo
                , UnitPrice
                , ValidPeriod                
                , BatchCardStatus
                , MakeDay
                , Start
               
            FROM FINCardConfigs");
            _sb.AppendFormat(" where Id='{0}'", id);
            _dbFunc.ConnectionString = connstr;
            var dt = _dbFunc.ExecuteDataTable(_sb.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<FINCardConfig>.FillSingleModel(dt.Rows[0]);
            }
            return null;

        }

        public List<FINCardConfig> QueryWithCardCount(string whr, string orderby, int pg, out int ttl, int pgsz)
        {
            _sb = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT ROW_NUMBER() over (Order by c.MakeDay desc) od, c.Id       
                                  ,CardPrefix
                                  ,u.UserName as Maker
                                  ,c.Memo
                                  ,c.UnitPrice
                                  ,ValidPeriod     
                                  ,BatchCardStatus
                                  ,MakeDay
                                  ,Start      
                                  ,c.StatusDay
                                   ,COUNT(d.Id) Count
                                  FROM FINCardConfigs c left
                                  join FINCards d  
                                  on c.Id=d.CardConfigId
                                  join aspnetusers u
                                  on u.Id=c.maker
                                ");
            if (!string.IsNullOrEmpty(whr))
            {
                sb.AppendFormat(" where {0}", whr);
            }

            sb.Append(@"  group by 
                 c.Id
                , CardPrefix
                ,  u.username
                , c.Memo
                , c.UnitPrice
                , ValidPeriod              
                , BatchCardStatus
                , MakeDay
                , Start              
                ,c.StatusDay");

            ttl = base.GetRecordCount(sb.ToString());
            _sb.Append("with tmp as(" + sb);
            _sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);

            _dbFunc.ConnectionString = connstr;
            var dt = _dbFunc.ExecuteDataTable(_sb.ToString());


            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<FINCardConfig>.FillModel(dt);
            }
            return null;

        }

        public List<FINCardConfig> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            _sb=new   StringBuilder();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT ROW_NUMBER() over (Order by MakeDay desc) od, Id
                                  ,Count
                                  ,CardPrefix
                                  ,Maker
                                  ,Memo
                                    ,MakeDay
                                    ,UnitPrice
                                  ,ValidPeriod
                                  ,Start                                                           
                                  ,BatchCardStatus                                
                              FROM FINCardConfigs");

            if (!string.IsNullOrEmpty(whr))
            {
                sb.Append(" where " + whr);
            }

            ttl = base.GetRecordCount(sb.ToString());
            _sb.Append("with tmp as(" + sb);
            _sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);

            _dbFunc.ConnectionString = connstr;
            var dt = _dbFunc.ExecuteDataTable(_sb.ToString());


            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<FINCardConfig>.FillModel(dt);
            }
            return null;
 
        }
    }
}
