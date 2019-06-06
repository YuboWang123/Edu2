using Edu.Entity.SchoolFinance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Edu.Entity;
using Wyb.DbUtility;


namespace Edu.DAL.SchoolFinance
{
    public class FINCardDAL :BaseDAL
    {
        private DbFunc _dbfunc;
        private readonly int _bulkInsertCount ;
        public FINCardDAL():base()
        {
            _dbfunc = new DbFunc();
        }

        public FINCardDAL(int bulkCnt):this()
        {
            _bulkInsertCount = bulkCnt;
        }

        #region Bulk Gen Card 

        public bool Add(DataTable dt)
        {
            return  _dbfunc.BulkInsert(dt, "FINCards", connstr, _bulkInsertCount);
        }

        public int Update(FINCard card)
        {
            _sb=new StringBuilder();
            _sb.Append(@"UPDATE FINCards
                                       SET
                                          Memo = @Memo
                                          ,UserId=@UserId                                 
                                          ,ActivatedDay = @ActivatedDay
                                          ,Status = @Status
                                          ,StatusDay = @StatusDay
                                          ,Password = @Password
                                          ,CardConfigId = @CardConfigId
                                          ,EndDay = @EndDay
                                          ,FailTimes = @FailTimes
                                          ,LockedEndTime = @LockedEndTime");
            _sb.Append($" where Id=@Id");
            var parmlist =TableToModel<FINCard>.FillDbParams(card, DbConfig.DbProviderType.SqlServer);

            _dbfunc.ConnectionString = connstr;
            return _dbfunc.ExecuteNonQuery(_sb.ToString(),parmlist.ToArray());
           
        }

        public int UpdateCardStatus(string configid,AppConfigs.SingleCardStatus status)
        {
            _sb=new StringBuilder();
            _sb.AppendFormat(@"Update FINCards set Status={0} where CardConfigId='{1}' ", (int)AppConfigs.SingleCardStatus.Deleted, configid);
            _dbfunc.ConnectionString = connstr;
            return _dbfunc.ExecuteNonQuery(_sb.ToString());

            //throw new NotImplementedException();
        }

        public DataTable QueryByConfig(string configid)
        {
            _sb = new StringBuilder();
            _sb.Append(@"SELECT Id
                                  ,Memo
                                  ,UserId
                                  ,ActivatedDay
                                  ,Status
                                  ,StatusDay
                                  ,CardConfigId
                                  ,Password
                              FROM FINCards");
            _sb.AppendFormat(" where CardConfigId='{0}'", configid);
            _dbfunc.ConnectionString = connstr;
            var dt = _dbfunc.ExecuteDataTable(_sb.ToString());
            if(dt!=null && dt.Rows.Count>0)
            {
                return dt;
            }

            return null;
        }



        #endregion
        /// <summary>
        /// del certain records by id list.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int BulkDel(string idString)
        {
            _sb = new StringBuilder();
            _sb.Append(@"delete from FINCards where Id in (" + idString + ")");
            _dbfunc.ConnectionString = connstr;
            return this._dbfunc.ExecuteNonQuery(_sb.ToString());
        }

        public int BulkDel(string idString,AppConfigs.SingleCardStatus status=AppConfigs.SingleCardStatus.Deleted)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat(@"UPDATE FINCards
                                          SET                                        
                                          Status = {0}
                                          ,StatusDay = getdate()
                                         ", (int)status);
            _sb.Append($" where Id in  (" + idString + ")");
            _dbfunc.ConnectionString = connstr;
            return _dbfunc.ExecuteNonQuery(_sb.ToString());

        }

        /// <summary>
        /// open window sql func get status and card count.
        /// </summary>
        /// <param name="configid"></param>
        /// <returns></returns>
        public List<KeyValuePair<AppConfigs.SingleCardStatus, int>> CountStatus(string configid)
        {
            _sb = new StringBuilder();
            _sb.Append(@"select distinct status, COUNT(id) over(partition by status) as cnt   from FINCards");
            _sb.AppendFormat(" where CardConfigId='{0}'", configid);
            _dbfunc.ConnectionString = connstr;
            var dt = _dbfunc.ExecuteDataTable(_sb.ToString());

            List<KeyValuePair<AppConfigs.SingleCardStatus,int>> list=new List<KeyValuePair<AppConfigs.SingleCardStatus, int>>();
            

            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    int i = 0;
                    if (int.TryParse(r[0].ToString(), out i))
                    {
                        var kv=new KeyValuePair<AppConfigs.SingleCardStatus,int>((AppConfigs.SingleCardStatus)i,int.Parse((r[1].ToString())));
                        list.Add(kv);
                    }
                }
            }
            return list;
       
        }

        public List<FinCardDto> QueryFinCardDtos(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            _sb = new StringBuilder();

            StringBuilder sb = new StringBuilder();

            _sb.Append(@"SELECT ROW_NUMBER() over (Order by ActivatedDay desc) od, Id
                                  ,(select unitprice from FINCardConfigs where Id=CardConfigId) UnitPrice
                                  ,(select username from AspNetUsers where Id=UserId) as UserId
                                  ,ActivatedDay
                                  ,Status,Password
                                  ,StatusDay
                                  ,CardConfigId
                                  ,EndDay
                              FROM FINCards");

            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where " + whr);
            }

            ttl = base.GetRecordCount(_sb.ToString());
            sb.Append("with tmp as(" + _sb);
            sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);

            _dbfunc.ConnectionString = connstr;
            var dt = _dbfunc.ExecuteDataTable(sb.ToString());


            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<FinCardDto>.FillModel(dt);
            }
            return null;

        }


        public List<FINCard> Query(string whr, string orderby, int pg, out int ttl,bool isAsc, int pgsz = 10)
        {
            orderby = string.IsNullOrWhiteSpace(orderby) ? "endday" : orderby;
            _sb = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            _sb.Append(@"SELECT ROW_NUMBER() over (Order by ActivatedDay desc) od, Id
                                  ,Memo
                                  ,(select username from AspNetUsers where Id=UserId) as UserId
                                  ,ActivatedDay
                                  ,Status,Password
                                  ,StatusDay
                                  ,CardConfigId
                                  ,EndDay
                              FROM FINCards");

            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where " + whr);
            }
            ttl = base.GetRecordCount(_sb.ToString());
            sb.Append("with tmp as(" + _sb);
            sb.AppendFormat(") select * from tmp where od > {0} and od <= {1}", (pg - 1) * 10, pg * 10);
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                sb.Append(" order by " + orderby);
            }

            if (!isAsc)
            {
                sb.Append(" desc");
            }

            _dbfunc.ConnectionString = connstr;
            var dt = _dbfunc.ExecuteDataTable(sb.ToString());


            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<FINCard>.FillModel(dt);
            }
            return null;

        }
        
        public int GenCards(int genCount)
        {
            _sb = new StringBuilder();
            throw new NotImplementedException();
        }

        public int Add(FINCard model)
        {
            _sb = new StringBuilder();
            throw new NotImplementedException();
        }

        public int Del(string Key)
        {
            _sb = new StringBuilder();
            _sb.AppendFormat("delete from Fincards where id='{0}' ", Key);
            _dbfunc.ConnectionString = connstr;
            return _dbfunc.ExecuteNonQuery(_sb.ToString());
            throw new NotImplementedException();
        }

      
        /// <summary>
        /// cound the card of the congid.
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public int CountCardsExist(string configId)
        {
            _sb=new  StringBuilder();
            _sb.AppendFormat("select COUNT(*) from FinCards where CardConfigId='{0}'", configId);
            _dbfunc.ConnectionString = connstr;
            return int.Parse(_dbfunc.ExecuteScalar(_sb.ToString()).ToString());
        }

        public FINCard Single(string Key)
        {
            _sb = new StringBuilder();
            _sb.Append(@"SELECT Id
                                    , Memo
                                    , UserId
                                    , ActivatedDay
                                    , Status
                                    , StatusDay
                                    , Password
                                    , CardConfigId
                                    , EndDay
                                    , FailTimes
                                    , LockedEndTime
                                FROM FINCards");
            _sb.AppendFormat(" where Id='{0}'", Key);
            _dbfunc.ConnectionString = connstr;
            var dt= _dbfunc.ExecuteDataTable(_sb.ToString());
            if (dt != null && dt.Rows.Count>0)
            {
                var mdl = TableToModel<FINCard>.FillSingleModel(dt.Rows[0]);
                return mdl;
            }
            return null;
       
        }

        /// <summary>
        /// activate a card.
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool TryActivate(string cardid, string pwd,out int failTimes)
        {
            var mdl = Single(cardid);
            if (mdl != null)
            {
                failTimes = mdl.FailTimes;
                if (mdl.Password == pwd)
                {
                    return true;
                }
                else
                {
                    failTimes += 1;
                    return false;
                }
            }

            failTimes = 10;
            return false;
        }

    }
}
