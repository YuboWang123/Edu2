using Edu.Common;
using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL.TrainLesson
{
    public class VcrTestDAL: BaseDAL,IDbFunc<VcrTest>
    {
        private DbFunc dbFunc;
        public VcrTestDAL():base()
        {
            dbFunc = new DbFunc();
        }

        public int Add(VcrTest mdl)
        {
            _sb = new StringBuilder();
            throw new NotImplementedException();        
        }

        public int Del(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("delete from vcrtests where Id='{0}'", k);
            dbFunc.ConnectionString = connstr;
            return dbFunc.ExecuteNonQuery(_sb.ToString());
        }

        public VcrTest Single(string k)
        {
            _sb = new StringBuilder();
            throw new NotImplementedException();
        }

        public int Update(VcrTest mdl)
        {
            _sb = new StringBuilder();
            throw new NotImplementedException();
        }

        public IEnumerable<TestItem> QueryAnswer(string vcrid)
        {
            _sb = new StringBuilder();
            _sb.Append(@"SELECT  Id , Answer as AnswerLetter FROM VcrTests ");
            _sb.AppendFormat("where VcrId='{0}' ", vcrid);
            dbFunc.ConnectionString = connstr;
            var dt = dbFunc.ExecuteDataTable(_sb.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<TestItem>.FillModel(dt);
            }
            return null;
        }



        public List<VcrTest> QueryList(string vcrid)
        {
            _sb = new StringBuilder();
            _sb.Append(@"SELECT  Id 
                                  , VcrId 
                                  , Qustion 
                                  , Answer 
                                  , AnswerLetter 
                                  , Analyze 
                                  , Maker 
                                  , MakeDay 
                                  , UpdateDay 
                                  , IsEnabled 
                                  , IsCorrect 
                              FROM    VcrTests ");

            _sb.AppendFormat("where VcrId='{0}' ",vcrid);
            dbFunc.ConnectionString = connstr;
            var dt=dbFunc.ExecuteDataTable(_sb.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<VcrTest>.FillModel(dt);
            }
            return null;
        }

        public int BulkInsertTest(List<VcrTest> testsList)
        {
            DataTable dt = GenTestTable();
            GenTableData(dt, testsList);
            try
            {
                var boo=new Wyb.DbUtility.DbFunc().BulkInsert(dt, "VcrTests", BaseDAL.connstr);
                return testsList.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int BulkDel(List<string> idlist)
        {
            if (idlist.Count > 0)
            {
                _sb = new StringBuilder();
                string lst ="'"+ string.Join("','", idlist)+"'";
                _sb.Append(@"delete from vcrtests where id in (" + lst + ")");
                dbFunc.ConnectionString = connstr;
                return dbFunc.ExecuteNonQuery(_sb.ToString());

            }
            return 0;
        }

        public int FkExists(string vcrId)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"select count(Id) from vcrtests where vcrid='{0}'", vcrId);
            dbFunc.ConnectionString = connstr;
            var ob = dbFunc.ExecuteScalar(_sb.ToString());
            if (ob != DBNull.Value)
            {
                return Convert.ToInt32(ob.ToString());
            }
            return 0;

            
        }
 



        public DataTable GenTestTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("VcrId", typeof(string));
            dt.Columns.Add("Qustion", typeof(string));
            dt.Columns.Add("Answer", typeof(string));          
            dt.Columns.Add("AnswerLetter", typeof(string));
            dt.Columns.Add("Analyze", typeof(string));
            dt.Columns.Add("Maker", typeof(string));
            dt.Columns.Add("MakeDay", typeof(DateTime));
            dt.Columns.Add("UpdateDay", typeof(DateTime));
            dt.Columns.Add("IsEnabled", typeof(bool));
            dt.Columns.Add("IsCorrect", typeof(bool));
            return dt;

        }
        public void GenTableData(DataTable dt,List<VcrTest> list)
        {
            if (list.Count > 0)
            {
                foreach (var rc in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = rc.Id.ToString();
                    dr["VcrId"] = rc.VcrId;
                    dr["Qustion"] = rc.Qustion;
                    dr["Answer"] = rc.Answer;
                    dr["AnswerLetter"] = rc.AnswerLetter;
                    dr["Analyze"] = rc.Analyze;
                    dr["Maker"] = rc.Maker;
                    dr["MakeDay"] = rc.MakeDay;
                    dr["UpdateDay"] = rc.UpdateDay;
                    dr["IsEnabled"] = rc.IsEnabled;
                    dr["IsCorrect"] = rc.IsCorrect;
                    dt.Rows.Add(dr);
                }
            }
        }
    }
}
