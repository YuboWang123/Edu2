using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL.TrainBase
{
    public class Base_SubjectDAL:BaseDAL
    {
       
        private DbFunc dbFunc;
        private string forepart = @"select  * from Base_Subject";
        public Base_SubjectDAL()
        {
            _sb = new StringBuilder();
        }

        public DataTable Query(string whr)
        {
            _sb.Append(forepart + whr);
            dbFunc = new DbFunc();
            return dbFunc.ExecuteDataTable(_sb.ToString());
        }
    }
}
