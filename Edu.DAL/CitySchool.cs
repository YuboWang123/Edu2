using Edu.Entity.CitySchool;

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL
{

    /// <summary>
    /// query database with sql string.
    /// </summary>
    public class CitySchool
    {
        private string cnnStr = @"Data Source=.;database=NetEdu;Integrated Security=True";

        private DbFunc _dbFunc;

        public CitySchool()
        {
            _sb = new StringBuilder();
            _dbFunc = new DbFunc();
            _dbFunc.ConnectionString = cnnStr;
        }

        private StringBuilder _sb;

        #region school

        #endregion


        #region city
        public DataTable GetCity(string prvcId)
        {
            _sb.AppendFormat("select * from Base_City where parentid={0}" , prvcId);
            return _dbFunc.ExecuteDataTable(_sb.ToString());
        }

        public DataTable GetProvince()
        {
            _sb.Append("select * from base_city where level=1");
            return _dbFunc.ExecuteDataTable(_sb.ToString());
        }

        public DataTable GetDownCountries(string parentId)
        {
            _sb = new StringBuilder();
            _sb.Append("select  * from base_city where parentid=" + parentId);
            return _dbFunc.ExecuteDataTable(_sb.ToString());
        }

        public int GetSchoolType(string schoId)
        {
            _sb.AppendFormat("select sctype  from Base_School where id={0}", schoId);
            var bo = _dbFunc.ExecuteScalar(_sb.ToString());
            if (bo != DBNull.Value)
                return int.Parse(bo.ToString());
            else
                return 0;
        }


        public DataTable GetSchoolType()
        {
            _sb = new StringBuilder();
            _sb.Append("select * from Base_SchoolType");
            return _dbFunc.ExecuteDataTable(_sb.ToString());
        }
        #endregion


    }
}
