using Edu.Entity.TrainBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL.TrainBase
{
    public class Base_PeriodDAL:BaseDAL
    {
      
        private DbFunc dbFunc;
        private string forepart = @"select * from Base_Period";

        public Base_PeriodDAL():base()
        {
            _sb = new StringBuilder();
        }

        public int Add(Base_Period model)
        {
            throw new NotImplementedException();
        }

        public int Del(string Key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Base_Period> Query(string whr)
        {
            _sb = new StringBuilder();
            _sb.Append(forepart );
            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where" + whr );
            }

            _sb.Append(" order by coalesce(OrderCode ,10000),OrderCode asc");

            dbFunc = new DbFunc();
            dbFunc.ConnectionString = connstr;
            var dt= dbFunc.ExecuteDataTable(_sb.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Base_Period>.FillModel(dt);
            }
            else
            {
                return null;
            }

        }



        public Base_Period Single(string Key)
        {
            throw new NotImplementedException();
        }

        public int Update(Base_Period model)
        {
            throw new NotImplementedException();
        }
    }
}
