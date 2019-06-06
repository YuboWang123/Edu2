using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using Wyb.DbUtility;

namespace Edu.DAL.TrainLesson
{
    public class VcrDAL :BaseDAL, IVcrPath
    {
        private readonly DbFunc _dbFun;
        public VcrDAL():base()
        {
            _dbFun = new DbFunc();
        }

        public int Add(Vcr mdl)
        {
            throw new NotImplementedException();
        }

        public int Del(string id)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("delete from vcrs where Id='{0}'",id);
            _dbFun.ConnectionString = connstr;


            return _dbFun.ExecuteNonQuery(_sb.ToString());
           
        }

        public Vcr Single(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("select * from Vcrs where Id='{0}'", k);
            _dbFun.ConnectionString = connstr;
            var dt = _dbFun.ExecuteDataTable(_sb.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Vcr>.FillSingleModel(dt.Rows[0]);
            }
            return null;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public string GetVcrPath(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"select VideoPath from vcrs where Id='{0}'", k);
            _dbFun.ConnectionString = connstr;
            var ob=_dbFun.ExecuteScalar(_sb.ToString());
            if (ob != DBNull.Value)
            {
                return ob.ToString();
            }
            return null;
        }



        public List<Vcr> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            _sb = new StringBuilder();
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = "coalesce(OrderCode ,10000) asc,MakeDay  ";
            }
            _sb.AppendFormat(@"  SELECT ROW_NUMBER() over (order by {0} ) od,Id 
                                              , LessonId 
                                              , VideoPath 
                                              , OrderCode 
                                              , MakeDay 
                                              , UpdateTime 
                                              , IsFree 
                                              , ViewTimes 
                                              , UpdatedBy 
                                              , FileOk 
                                              , HasTest                                               
                                              , TrainerId 
                                              , TitleOrName 
                                              , Maker 
                                              , Memo 
                                              , IsEnabled                                               
                                          FROM  Vcrs  ", orderby);

            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where " + whr);
            }
            ttl = base.GetRecordCount(_sb.ToString());

            StringBuilder stringBuilder2 = new StringBuilder();

            stringBuilder2.Append("with tmp as(");
            stringBuilder2.Append(_sb);
            stringBuilder2.Append($")select * from tmp where od>{(pg - 1) * pgsz} and od<={ pg * pgsz }");
            _dbFun.ConnectionString = connstr;
            var dt = _dbFun.ExecuteDataTable(stringBuilder2.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Vcr>.FillModel(dt);
            }
            return null;
          
        }


        public List<Vcr> Query(string lessonid) {
            _sb = new StringBuilder();
            _sb.Append(@"select Id, LessonId 
                                              , VideoPath 
                                              , OrderCode 
                                              , MakeDay 
                                              , UpdateTime 
                                              , IsFree 
                                              , ViewTimes 
                                              , UpdatedBy 
                                              , FileOk 
                                              , HasTest                                              
                                              , TrainerId 
                                              , TitleOrName 
                                              , Maker 
                                              , Memo 
                                              , IsEnabled                                             
                                          FROM  Vcrs  ");

            if (!string.IsNullOrEmpty(lessonid))
            {
                _sb.AppendFormat(" where LessonId='{0}'", lessonid);
            }

            _sb.Append(" order by  coalesce(OrderCode ,10000) asc ,MakeDay asc");
            _dbFun.ConnectionString = connstr;

            var dt = _dbFun.ExecuteDataTable(_sb.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Vcr>.FillModel(dt);
            }
            return null;

        }

        public int Exists(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("select count(id) from vcrs where id='{0}'", k);
            _dbFun.ConnectionString = connstr;
            var ob = _dbFun.ExecuteScalar(_sb.ToString());
            if (ob != DBNull.Value)
            {
                return Convert.ToInt32(ob.ToString());
            }
            return 0;

        }

        /// <summary>
        /// update vcr model.
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        public int Update(Vcr mdl)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"UPDATE Vcrs 
                                                       SET  
      	                                                     LessonId  =  @LessonId,  
      	                                                     VideoPath  =  @VideoPath, 
      	                                                     OrderCode  =  @OrderCode,
      	                                                     UpdateTime  =  @UpdateTime, 
      	                                                     IsFree  =  @IsFree, 
      	                                                     ViewTimes  =  @ViewTimes,
      	                                                     UpdatedBy  =  @UpdatedBy,  
      	                                                     FileOk  =  @FileOk,
      	                                                     HasTest  =  @HasTest,      	                                                   
      	                                                     TrainerId  =  @TrainerId,  
      	                                                     TitleOrName  =  @TitleOrName,       	                                                   
      	                                                     Memo  =  @Memo, 
      	                                                     IsEnabled  =  @IsEnabled      	                                                  
                                                             WHERE  Id=@Id");

            var parmlist = Wyb.DbUtility.TableToModel<Vcr>.FillDbParams(mdl, DbConfig.DbProviderType.SqlServer);
            _dbFun.ConnectionString = connstr;
            return  _dbFun.ExecuteNonQuery(_sb.ToString(),parmlist.ToArray());

            // throw new NotImplementedException();
            //throw new NotImplementedException();
        }

 
        /// <summary>
        /// get video path string from vcr table
        /// </summary>
        /// <param name="vcrId"></param>
        /// <returns></returns>
        public string VcrPath(string vcrId)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("select VideoPath from vcrs where Id='{0}'", vcrId);
            _dbFun.ConnectionString = connstr;
            var b =_dbFun.ExecuteScalar(_sb.ToString());
            if (b != DBNull.Value)
            {
                return b.ToString();
            }
            else
            {
                return null;
            }

            
        }

    
    }
}
