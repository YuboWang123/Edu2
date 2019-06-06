using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common; 
using System.Text;
using Edu.Entity.TrainBase;
using Wyb.DbUtility;

namespace Edu.DAL
{
    public class BaseDAL
    {

        public string GetConnDecrypt(string userid,string pwd)
        {
            string pwd_R = "", user_R = "";
            return $"Server = 101.201.109.100;Database = NetTrain_2;User ID = {user_R};Password = {pwd_R};Trusted_Connection = False;";
        }


        public static string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
   
        protected StringBuilder _sb;
        public BaseDAL()
        {          
           
        }

        public int GetRecordCount(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                DbFunc dbFunc = new DbFunc();
                dbFunc.ConnectionString = connstr;
                var c =dbFunc.ExecuteScalar("Select Count(*) from (" + sql + ") as T");
                return Convert.ToInt32(c);
            }
            return 0;
        }


        public IEnumerable<TTrain> OrderTrainList<TTrain>(TTrain tp, string column,int pg,out int ttl,bool asc=true) where TTrain : ITrainBase
        {
            Type typ = tp.GetType();
            switch (typ.ToString())
            {
                case "Base_Period":
                    break;
                case "Base_Subject":
                    break;
                case "Base_Grade":
                    break;
                case "Base_Genre":
                    break;
            }

             string ord = asc ? "asc" : "desc";

            DbFunc dbFunc=new DbFunc();
            
            dbFunc.ConnectionString = connstr;
            _sb = new StringBuilder();

          

            _sb.Append($"SELECT ROW_NUMBER() over (Order by {column}  { ord  }) od, *   FROM "+typ.ToString());
            ttl = GetRecordCount(_sb.ToString());
            _sb.Append("with tmp as(" + _sb);
            _sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);
            var dt = dbFunc.ExecuteDataTable(_sb.ToString());
            if (dt != null)
            {
           
            }
            throw new NotImplementedException();
        }


    }
}
