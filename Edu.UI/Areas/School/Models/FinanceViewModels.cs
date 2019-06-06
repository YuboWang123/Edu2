using Edu.Entity.SchoolFinance;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Edu.Entity;

namespace Edu.UI.Areas.School.Models.FinanceViewModels
{
    public class ConfigListViewModel
    {
        public int state { get; set; }
        public IEnumerable<FINCardConfig> configs { get; set; }
        public int  ttlRecord { get; set; }
        public string Pager { get; set; }
    }

    public class CardListViewModel
    {
        public int ttlRecord { get; set; }
        public string Pager { get; set; }
        public IEnumerable<FINCard> Cards { get; set; }
        public IEnumerable<KeyValuePair<AppConfigs.SingleCardStatus,int>>  StatusCount { get; set; }
    }

    public class ConfigEditViewModel
    {
        public int CardCnt { get; set; }
        public FINCardConfig FinCardConfig { get; set; }
    }
    


}