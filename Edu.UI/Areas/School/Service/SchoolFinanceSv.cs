using Edu.BLL.SchoolFinance;
using Edu.Entity.SchoolFinance;
using Edu.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Edu.Entity;
using Edu.UI.Areas.School.Models.FinanceViewModels;
using Wyb.DbUtility;


namespace Edu.UI.Areas.School.Service
{
    public class SchoolFinanceSv
    {
        public SchoolFinanceSv()
        {
            _finCardBll = new FINCardBLL();
        }

       
        private readonly FINCardBLL _finCardBll;
        
        private CardGenParams cardGenViewModel;

        public SchoolFinanceSv(CardGenParams cardGen) : this()
        {
            cardGenViewModel = cardGen;

        }

        #region Config

        public ConfigListViewModel QueryConfigList(AppConfigs.BatchType type, int status, int pg)
        {
                string w = "BatchType="+(int)type+(status ==-1? null: " and BatchCardStatus=" + status);
                int t=0;
                return new ConfigListViewModel()
                {
                    state = (int)status,
                    configs = QueryWithCardCount(w, null, pg, out t,20),
                    ttlRecord = t,
                    Pager = Common.Utility.HtmlPager(10,pg,t,20)
                };
        }


        private string ConfigWhrByStatus(int i)
        {
            if (i ==0)
            {
                return null;
            }
            return "CardStatus=" + i;
        }

        public List<FINCardConfig> QueryWithCardCount(string whr, string orderby, int pg, out int ttl, int pgsz)
        {
            return new FINCardConfigBLL().QueryWithCardCount(whr, orderby, pg, out ttl, pgsz);
        }


        public FINCardConfig GetSingle(string id)
        {
            using (var dbAppContext = new ApplicationDbContext())
            {
                return dbAppContext.FINCardConfigs.Include(a => a.FinCards).SingleOrDefault(a => a.Id == id);
            }
        }

        public int ConfigAdd(FINCardConfig config)
        {
            using (var dbAppContext = new ApplicationDbContext())
            {
                config.Id = Guid.NewGuid().ToString("n");
                config.MakeDay=DateTime.Now;
                config.BatchCardStatus = AppConfigs.BatchCardStatus.Created;
                config.StatusDay=DateTime.Now;
                dbAppContext.FINCardConfigs.Add(config);
                return dbAppContext.SaveChanges();
            }
        }

        public int UpdateConfig(FINCardConfig mdl)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Entry(mdl).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// really delete config.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DelConfig(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.FINCardConfigs.Any(a => a.Id == id))
                {
                    if (CardExist(id))
                    {
                        ///some card record down there.
                        return 0;
                    }
                    var mdl = db.FINCardConfigs.Find(id);
                    db.FINCardConfigs.Remove(mdl);
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public AppConfigs.OperResult DelConfigVirturl(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.FINCardConfigs.Any(a => a.Id == id))
                {
                    if (CardExist(id))
                    {
                        ///some card record down there.
                        return 0;
                    }

                    var mdl = db.FINCardConfigs.Find(id);
                    _finCardBll.UpdateCardStatus(id, AppConfigs.SingleCardStatus.Deleted);
                    mdl.BatchCardStatus = AppConfigs.BatchCardStatus.Deleted;
                    db.Entry(mdl).State = EntityState.Modified;
                    return db.SaveChanges()>0?AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
                }

                return AppConfigs.OperResult.failDueToExist;
            }
        }

        private bool CardExist(string configId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.FINCards.Any(a => a.CardConfigId == configId);
            }
        }

        #endregion

        #region cards

        public AppConfigs.OperResult Clear(string configId)
        {
            using (var db = new ApplicationDbContext())
            {
                var mdl = db.FINCardConfigs.Find(configId);
                if (mdl != null)
                {
                    mdl.BatchCardStatus = AppConfigs.BatchCardStatus.Deleted;
                    db.Entry(mdl).State = EntityState.Modified;
                    return db.SaveChanges()>0 ? AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
                }
                return AppConfigs.OperResult.failDueToExist;
            }
        }

        /// <summary>
        /// generate cards by config.
        /// </summary>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public async Task<int> GenCards()
        {
            if (cardGenViewModel != null)
            {
                var configid = cardGenViewModel.configId;
                var studycard=new StudyCard(configid);
                return await studycard.genCardListAsync(cardGenViewModel);
            }
            else
            {
                throw new ArgumentNullException("CardGenViewModel not found");
            }
           
        }


        public int DelSingleCard(string key)
        {
            return this._finCardBll.Del(key);
        }

        /// <summary>
        /// toggle freeze card.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AppConfigs.SingleCardStatus FreezeSingleCard(string key)
        {
            var mdl = Single(key);
            if (mdl!= null)
            {
              
                if (mdl.Status == AppConfigs.SingleCardStatus.AdminFreezed ||
                    mdl.Status == AppConfigs.SingleCardStatus.Freezed)
                {
                    mdl.Status = AppConfigs.SingleCardStatus.InUse;
                    mdl.StatusDay = DateTime.Now;
                }
                else
                {
                    mdl.Status = AppConfigs.SingleCardStatus.AdminFreezed;
                    mdl.StatusDay = DateTime.Now;
                }
               
                this._finCardBll.Update(mdl);
                return mdl.Status;
            }

            return 0;

        }

        public AppConfigs.SingleCardStatus unfreezeSingleCard(string key)
        {
            var mdl = Single(key);
            if (mdl != null)
            {
                if (mdl.Status == AppConfigs.SingleCardStatus.AdminFreezed || mdl.Status == AppConfigs.SingleCardStatus.Freezed)
                {
                    mdl.StatusDay = DateTime.Now;
                    mdl.Status = AppConfigs.SingleCardStatus.InUse;
                    _finCardBll.Update(mdl);
                }
              
                return mdl.Status;
            }

            return 0;

        }

        public FINCard Single(string Key)
        {
            return _finCardBll.Single(Key);
        }


        public int DelManyCards(IList<string> idList)
        {
            return new StudyCard().BulkDelCard(idList);
        }

        public List<FINCard> QueryCards(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            var cardPager = new FINCardBLL();
            return cardPager.Query(whr, orderby, pg, out ttl, pgsz);
        }



        /// <summary>
        /// count card by status.
        /// </summary>
        /// <param name="configid">fk</param>
        /// <returns></returns>
        public List<KeyValuePair<AppConfigs.SingleCardStatus, int>> CountStatus(string configid)
        {
            return new FINCardBLL().CountStatus(configid);
        }

        public List<FINCard> ListStatusCards(AppConfigs.SingleCardStatus? status,string configId, int pg, out int i)
        {
            string whr = null;
            if (status.HasValue)
            {
                whr = "Status=" + (int)status;
            }

            if (!string.IsNullOrWhiteSpace(configId))
            {
                whr += " and CardConfigId='" + configId + "'";
            }

            return QueryCards(whr, null, pg, out i);
        }

        #endregion







    }
}