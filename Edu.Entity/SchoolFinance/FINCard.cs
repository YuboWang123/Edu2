using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using static Edu.Entity.AppConfigs;

namespace Edu.Entity.SchoolFinance
{
    public class FINCard:BaseCard,ITester
    {
        public string Memo { get; set; }
        public string ActivatedDay { get; set; }
        /// <summary>
        /// card outdated day.
        /// </summary>
        public DateTime? EndDay { get; set; }
        public DateTime StatusDay { get; set; }
        [Required] public string CardConfigId { get; set; }
        public FINCardConfig CardConfig { get; set; }

        ///fail funcs:
        public int FailTimes { get; set; }
        public DateTime? LockedEndTime { get; set; }
        
    }

    public class FinCardDto : FINCard
    {
        public float UnitPrice { get; set; }
    }

}
