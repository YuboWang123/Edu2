

using Edu.Entity.TrainBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Wyb.DbUtility;


namespace Edu.DAL.TrainBase
{
    public class Base_DataBindDAL:BaseDAL
    {
        private readonly DbFunc dbFunc;
        
        public Base_DataBindDAL():base()
        {
            dbFunc = new DbFunc();
        }

        public DataTable GetDataTable(string whr, string column)
        {
            _sb = new StringBuilder();
            _sb.Append("select distinct "+column+" from Base_DataBind where "+whr);
            dbFunc.ConnectionString = connstr;
            return dbFunc.ExecuteDataTable(_sb.ToString());           
        }


        public string GetBindingId(string whr)
        {
            _sb = new StringBuilder();
            _sb.Append("select Id from Base_DataBind where " + whr);
            dbFunc.ConnectionString = connstr;
            var ob= dbFunc.ExecuteScalar(_sb.ToString());
            return ob?.ToString();
        }

        /// <summary>
        /// get binding data at top of lesson page
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="whr"></param>
        /// <returns></returns>
        public DataTable GetBindingData(string tablename,string whichId, string whr)
        {
            _sb = new StringBuilder();
            _sb.Append("select * from " + tablename);
            _sb.AppendFormat(" where IsEnabled=1 and Id in (select {0} from Base_Databind",whichId);
            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where " + whr);
            }
            _sb.AppendFormat(" group by {0})",whichId);
            _sb.Append(" order by coalesce(OrderCode ,10000),OrderCode asc");
            dbFunc.ConnectionString = connstr;
            return dbFunc.ExecuteDataTable(_sb.ToString());
        }

    }
}
