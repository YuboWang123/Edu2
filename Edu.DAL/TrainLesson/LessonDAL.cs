using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL.TrainLesson
{
    public class LessonDAL:BaseDAL
    {
        
        private DbFunc _dbfunc;
      
        public LessonDAL():base()
        {          
            _dbfunc = new DbFunc();
        }

        public bool Exist(string id)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("select count(Id) from Trainbaselessons where Id='{0}'",id);
            _dbfunc.ConnectionString = connstr;
            return _dbfunc.Exists(_sb.ToString());
        }

        /// <summary>
        /// check if lesson with bindid exist.
        /// </summary>
        /// <param name="bindId"></param>
        /// <returns></returns>
        public bool ExistByFk(int bindId)
        {
            _sb=new StringBuilder();
            _sb.AppendFormat("select count(Id) from Trainbaselessons where Base_DataBindId={0}", bindId);
            _dbfunc.ConnectionString = connstr;
            return _dbfunc.Exists(_sb.ToString());
        }

        /// <summary>
        /// get lesson by inner join sql .
        /// </summary>
        /// <param name="whr"></param>
        /// <param name="pg"></param>
        /// <param name="ttl"></param>
        /// <param name="pgsz"></param>
        /// <returns></returns>
        public DataTable Query(string whr, int pg, out int ttl, int pgsz)
        {
            _sb = new StringBuilder();
            _sb.Append(@"select ROW_NUMBER() over(order by MakeDay desc) od
                                      ,Id
                                      ,TitleOrName
                                      ,ImagePath
                                      ,Price
                                      ,DiscountPrice
                                      ,MakeDay
                                      ,Base_DataBindId
                                      ,ClickTimes      
                                      ,Maker
                                      ,Memo
                                      ,IsEnabled
                                      ,OrderCode
                                      ,IsBasic
                                      ,(select COUNT(*) from Vcrs where LessonId=Trainbaselessons.Id) VideoCount
                                 from Trainbaselessons  ");

            _sb.Append($" where Base_DataBindId=(select Id from base_databind where { whr })");
            _dbfunc.ConnectionString = connstr;
            ttl = base.GetRecordCount(_sb.ToString());

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"with tmp as(");
            stringBuilder.Append(_sb);
            stringBuilder.Append($")  select * from tmp where od>{(pg - 1) * pgsz} and od <{pg * pgsz} ");

            return _dbfunc.ExecuteDataTable(stringBuilder.ToString());
          
        }

        public IEnumerable<TrainBaseLesson> QueryByBindingId(string bindId,int pg,out int ttl,int pgsz)
        {
            _sb = new StringBuilder();
            _sb.Append(@"SELECT ROW_NUMBER() over (Order by makeday desc) od, Id 
                                      , TitleOrName 
                                      , ImagePath 
                                      , Price 
                                      , DiscountPrice 
                                      , MakeDay 
                                      , Base_DataBindId 
                                      , ClickTimes 
                                      , VideoCount 
                                      , Maker 
                                      , Memo 
                                      , IsEnabled 
                                      , OrderCode 
                                      , IsBasic 
                                  FROM  TrainBaseLessons");

            _sb.Append($" where Base_DataBindId='{bindId}'");
            _dbfunc.ConnectionString = connstr;
            ttl = base.GetRecordCount(_sb.ToString());

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"with tmp as(");
            stringBuilder.Append(_sb);
            stringBuilder.Append($")  select * from tmp where od>{(pg-1)* pgsz} and od <{pg*pgsz} ");

            var dt = _dbfunc.ExecuteDataTable(stringBuilder.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<TrainBaseLesson>.FillModel(dt);
            }
            return null;
          


            throw new NotImplementedException();
        }


        public IEnumerable<TrainBaseLesson> Query(string whr,string orderby, int? top)
        {
            _sb = new StringBuilder();
            _sb.Append("select ");
            if (top.HasValue)
            {
                _sb.Append("top "+top.ToString());
            }
            _sb.Append(" * from Trainbaselessons");
            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where " + whr);
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                _sb.Append(" order by " + orderby);
            }
            _dbfunc.ConnectionString = connstr;
            DataTable dt = _dbfunc.ExecuteDataTable(_sb.ToString());
            if(dt!=null&& dt.Rows.Count > 0)
            {
                return TableToModel<TrainBaseLesson>.FillModel(dt);
            }

            return null;

           
        }



        public TrainBaseLesson Single(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("select * from Trainbaselessons where Id='{0}'",k);
            _dbfunc.ConnectionString = connstr;
            DataTable dt = _dbfunc.ExecuteDataTable(_sb.ToString());

            if(dt!=null&& dt.Rows.Count >0)
            {
                return TableToModel<TrainBaseLesson>.FillSingleModel(dt.Rows[0]);
            }
            return null;

            throw new NotImplementedException();
        }

        public TrainBaseLesson GetByVcrId(string vcrid)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"select * from TrainBaseLessons where Id = (select LessonId from vcrs where Id = '{0}')", vcrid);
            _dbfunc.ConnectionString = connstr;
            DataTable dt = _dbfunc.ExecuteDataTable(_sb.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<TrainBaseLesson>.FillSingleModel(dt.Rows[0]);
            }
            return null;
            
        }



        public int DelLessonImage(string path)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("update TrainBaseLessons set imagepath=null where imagepath='{0}' ", path);
            _dbfunc.ConnectionString = connstr;
            return int.Parse(_dbfunc.ExecuteNonQuery(_sb.ToString()).ToString());
        }
    }
}
