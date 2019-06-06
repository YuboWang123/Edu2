using Edu.Entity.SchoolFinance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Edu.DAL.SchoolFinance;
using Edu.DAL.TrainBase;
using Edu.Entity;
using Wyb.DbUtility;

namespace Edu.BLL.SchoolFinance
{
    public class FINCardBLL 
    {
        private FINCardConfig _config;
        private FINCardDAL _DAL;
        public FINCardBLL()
        {
            _DAL = new FINCardDAL();
          
        }

        public FINCardBLL(FINCardConfig config):this()
        {
            _config = config;
        }



        public DataTable QueryByConfig(string configid)
        {
            return _DAL.QueryByConfig(configid);
        }

        public int CountCardsExist(string configId)
        {
            return _DAL.CountCardsExist(configId);
        }

        /// <summary>
        /// get rebuilt kv list 
        /// </summary>
        /// <param name="configid"></param>
        /// <returns></returns>
        public List<KeyValuePair<AppConfigs.SingleCardStatus, int>> CountStatus(string configid)
        {
            var originals = AppConfigs.GetStatusSingleCard(); //orignal enum data.

            List<KeyValuePair<AppConfigs.SingleCardStatus,int>> dbList = _DAL.CountStatus(configid);

            List<KeyValuePair<AppConfigs.SingleCardStatus,int>> rslt=new List<KeyValuePair<AppConfigs.SingleCardStatus, int>>();

            foreach (var item in originals)
            {
                if (!dbList.Any(a => a.Key == (AppConfigs.SingleCardStatus)item.Key))
                {
                    rslt.Add(new KeyValuePair<AppConfigs.SingleCardStatus, int>((AppConfigs.SingleCardStatus)item.Key, 0));
                }
            }

            rslt.AddRange(dbList);
          
            return rslt;
             
        }



        /// <summary>
        /// has validated card and no expired.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool HasCard(string uid)
        {
            var list = Query($"userid ='{uid}' and status =1 and endday>=getdate()", null, 1, out int i, false);
            return i > 0;
        }

        public List<FINCard> Query(string whr, string orderby, int pg, out int ttl,bool isAsc, int pgsz = 10)
        {
            return _DAL.Query(whr, orderby, pg, out ttl,isAsc, pgsz);
        }

        public List<FINCard> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            return _DAL.Query(whr, orderby, pg, out ttl,true, pgsz);
        }

        public List<FinCardDto> QueryFinCardDtos(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            return _DAL.QueryFinCardDtos(whr, orderby, pg, out ttl, pgsz);
        }

        public int UpdateCardStatus(string configId,AppConfigs.SingleCardStatus status)
        {
            return _DAL.UpdateCardStatus(configId, status);
        }

        public int Add(FINCard model)
        {
            return _DAL.Add(model);
        }

    
        /// <summary>
        /// really delete the records.
        /// </summary>
        /// <param name="idString"></param>
        /// <returns></returns>
        public int BulkDel(string idString)
        {
            return _DAL.BulkDel(idString,AppConfigs.SingleCardStatus.Deleted);
        }

        public bool BulkAdd(DataTable dt)
        {
            _DAL=new FINCardDAL(dt.Rows.Count);
            return _DAL.Add(dt);
            
        }
   
        public FINCard Single(string Key)
        {
            return _DAL.Single(Key);
        }
        public int Del(string Key)
        {
            //return _DAL.Del(Key);
            var mdl = Single(Key);
            mdl.Status = AppConfigs.SingleCardStatus.Deleted;
            mdl.StatusDay = DateTime.Now;
            return Update(mdl);
        }


        public int Update(FINCard card)
        {
            return _DAL.Update(card);
        }

        public bool TryActivate(string cardid, string pwd, out int failTimes)
        {
            return _DAL.TryActivate(cardid, pwd,out failTimes);
        }

    }
}
