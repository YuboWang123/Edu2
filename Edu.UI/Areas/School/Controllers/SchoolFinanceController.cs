using Edu.BLL.SchoolFinance;
using Edu.Entity;
using Edu.Entity.SchoolFinance;
using Edu.UI.Areas.School.Models.FinanceViewModels;
using Edu.UI.Areas.School.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Edu.UI.Areas.School.Controllers
{
    /// <summary>
    /// finance controller  at top menu of finance.
    /// </summary>
    public class SchoolFinanceController : SchoolBaseController
    {
        private SchoolFinanceSv _financeSv;

        public SchoolFinanceController()
        {
            _financeSv = new SchoolFinanceSv();
        }

        /// <summary>
        /// card config list page of rechargable card.
        /// </summary>
        public ActionResult Index(int state=-1,int pg=1)
        {
            ///get default list of Rechargable cards.
            ConfigListViewModel configListViewModel=_financeSv.QueryConfigList(AppConfigs.BatchType.RechargableCard, state,pg);
            
            return PartialView(configListViewModel);
        }

        /// <summary>
        /// card config page of periodcard.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult PeriodCard(int status= -1,int pg=1)
        {
            ConfigListViewModel configListViewModel = _financeSv.QueryConfigList(AppConfigs.BatchType.PeriodCard, status, pg);
            return PartialView(configListViewModel);
        }

       
        public ActionResult Sale()
        {
            string whr = "UserId is not null";
            FINCardBLL bll=new FINCardBLL();
           var list= bll.Query(whr, "statusday", 1, out int i,false, 10);
           CardListViewModel mdl=new CardListViewModel()
           {
               Cards=list,
               Pager=Common.Utility.HtmlPager(10,1,i,5),
               ttlRecord=i
           };
           return PartialView(mdl);

           
        }

        /// <summary>
        /// get config list of certain type---ajax
        /// </summary>
        /// <param name="cardType"></param>
        /// <param name="state"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult _ConfigTable(AppConfigs.BatchType cardType, int state =-1, int pg = 1)
        {
            int i;
            string whr = "BatchType="+(int)cardType??1.ToString();

            if (state != -1)
            {
                whr += " and BatchCardStatus=" + state;
            }
            var mdl = _financeSv.QueryWithCardCount(whr, null, pg, out i, 10);
            return PartialView(mdl);
        }

        #region CardConfig


        /// <summary>
        /// generate cards immediately and save the config setup.---ajax
        /// </summary>
        /// <param name="id">config id.</param>
        /// <returns></returns>
        public async Task<int> GenCards(CardGenParams genViewModel)
        {
            _financeSv = new SchoolFinanceSv(genViewModel);
            return await _financeSv.GenCards();
        }

        /// <summary>
        /// make new
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfigNew([Bind(Exclude = "Maker,MakeDay,Id")]FINCardConfig config)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                config.Maker = MyUserId;
                i = this._financeSv.ConfigAdd(config);
            }
            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ConfigEdit(string id)
        {
            var mdl=new SchoolFinanceSv().GetSingle(id);
            return PartialView(mdl);
        }

        [HttpPost]
        public ActionResult ConfigEdit(FINCardConfig config)
        {
            AppConfigs.OperResult i=AppConfigs.OperResult.failDueToExist;
            if (ModelState.IsValid)
            {
                i = _financeSv.UpdateConfig(config)>0 ?AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
            }
            return Json(new {i},JsonRequestBehavior.AllowGet);
        }



        public ActionResult DelConfig(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException();
            }
            var i= _financeSv.DelConfigVirturl(id);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Card 

        /// <summary>
        /// list card list by card status--partial page.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult statusCards(string statusStr,string configId, int pg = 1)
        {
            int i;
            if (string.IsNullOrWhiteSpace(statusStr))
            {
                statusStr = "NeverUsed";
            }
            AppConfigs.SingleCardStatus status =(AppConfigs.SingleCardStatus) Enum.Parse(typeof(AppConfigs.SingleCardStatus),statusStr,false);
            var mdl = _financeSv.ListStatusCards(status,configId, pg,out i);
            var cardlst=new CardListViewModel()
            {
                Cards=mdl,
                Pager=Common.Utility.HtmlPager(10,pg,i,5),
                ttlRecord=i,
                StatusCount = _financeSv.CountStatus(configId)
            };
            return PartialView(cardlst);
        }



        /// <summary>
        /// card list by config.
        /// </summary>
        /// <param name="id">config id</param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public ActionResult CardList(string id, int pg = 1)
        {
            int i;
            string whr = string.Format("CardConfigId='{0}'", id);
            var viewModel = new CardListViewModel()
            {
                Cards = _financeSv.QueryCards(whr, null, pg, out i, 10),
                Pager = Common.Utility.HtmlPager(10, pg, i, 5),
                ttlRecord = i,
                StatusCount = _financeSv.CountStatus(id)
            };
            return PartialView(viewModel);
        }



        public JsonResult Clear(string configId)
        {
            if (string.IsNullOrEmpty(configId))
            {
                throw new ArgumentNullException("configId  not defined");
            }

            var i =_financeSv.Clear(configId);

            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListExport(string id)
        {
            StudyCard study = new StudyCard(id);
            string ListName = "学习卡" + DateTime.Now.ToFileTime().ToString();
            Session[ListName] = study.getExcelStream(); //get excel stream
            return Json(new { success = true, ListName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetXls(string ListName)
        {
            var ms = new MemoryStream((byte[])Session[ListName]);
            if (ms == null) return new EmptyResult();
            Session[ListName] = null;
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ListName + ".xlsx");

        }



        /// <summary>
        /// del single card record.----ajax.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelCard(string id)
        {
            int i = 0;
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("argument is null");
            }
            i = _financeSv.DelSingleCard(id);
            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// del many cards by checkbox selected.---ajax.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult DelCards(IList<string> ids)
        {
            int i = _financeSv.DelManyCards(ids);
            return Json(new {i}, JsonRequestBehavior.AllowGet);
            throw new NotImplementedException();
        }

        /// <summary>
        /// admin freeze card .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FreezeCard(string id)
        {
            if (string.IsNullOrWhiteSpace(id) && id =="-1")
            {
                throw new NotImplementedException();
            }
            var i = _financeSv.FreezeSingleCard(id);
            return Json(new {i}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// unfreeze card.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UnfreezeCard(string id)
        {
            if (string.IsNullOrWhiteSpace(id) && id == "-1")
            {
                throw new NotImplementedException();
            }
            var i = _financeSv.unfreezeSingleCard(id);
            return Json(new { i }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}