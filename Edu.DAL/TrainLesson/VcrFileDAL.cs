using Edu.Common;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Text;
using Wyb.DbUtility;

namespace Edu.DAL.TrainLesson
{
    public class VcrFileDAL:BaseDAL
    {
        private readonly DbFunc dbFunc;

        public VcrFileDAL():base()
        {
            dbFunc = new DbFunc();
        }

        public int FkExists(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("select count(Id) from vcrfiles where vcrid='{0}'", k);
            dbFunc.ConnectionString = connstr;
            return dbFunc.ExecuteNonQuery(_sb.ToString());           
        }

        public int Del(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("delete from vcrfiles where id='{0}'", k);
            dbFunc.ConnectionString = connstr;
            return dbFunc.ExecuteNonQuery(_sb.ToString());

            //throw new NotImplementedException();
        }

        public int BulkDel(string[] idlist)
        {
            if (idlist.Length > 0)
            {
                _sb = new StringBuilder();
                string lst ="'"+ string.Join("','", idlist)+"'";

                _sb.Append(@"delete from vcrfiles where id in (" + lst + ")");
                dbFunc.ConnectionString = connstr;
                return dbFunc.ExecuteNonQuery(_sb.ToString());

            }
            return 0;
        }

        public IEnumerable<VcrFile> QueryList(string vid)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"SELECT  Id 
                                                  , VcrId
                                                  , Name
                                                  , Path
                                                  , MakeDay
                                                  , Maker
                                                  , FileOk,FileSize
                                              FROM  VcrFiles  where VcrId='{0}' ", vid);
            dbFunc.ConnectionString = connstr;
            var dt=   dbFunc.ExecuteDataTable(_sb.ToString());
            if (dt != null&& dt.Rows.Count>0)
            {
                return TableToModel<VcrFile>.FillModel(dt);
            }
            return null;
        }

        public VcrFile Single(string k)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"SELECT  id 
                                                  , VcrId 
                                                  , Name 
                                                  , Path 
                                                  , MakeDay 
                                                  , Maker 
                                                  , FileOk 
                                              FROM VcrFiles  where Id='{0}'", k);
            dbFunc.ConnectionString = connstr;
            var dt = dbFunc.ExecuteDataTable(_sb.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<VcrFile>.FillSingleModel(dt.Rows[0]);
            }
            return null;

          
        }

        public int Update(VcrFile mdl)
        {
            _sb = new StringBuilder();
            _sb.Append(@"UPDATE  VcrFiles 
                                   SET 
                                       VcrId  =  VcrId, 
                                       Name  =  Name, 
                                       Path  =  Path,   
                                       FileOk  =  FileOk ");
            _sb.Append(" Where Id=@Id");

            var parmlist = Wyb.DbUtility.TableToModel<VcrFile>.FillDbParams(mdl, DbConfig.DbProviderType.SqlServer);


            dbFunc.ConnectionString = connstr;

            return dbFunc.ExecuteNonQuery(_sb.ToString(),parmlist.ToArray());
        
            //throw new NotImplementedException();
        }

      

    }
}
