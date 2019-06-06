using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Edu.Entity.AppConfigs;

namespace Edu.Entity.SchoolFinance
{
    /// <summary>
    /// basic study card.
    /// </summary>
    public class BaseCard
    {
        [Key][StringLength(128)]
        public string Id { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// owner id
        /// </summary>
        public string UserId { get; set; }
        public SingleCardStatus Status { get; set; } //see also in cardconfig .
    }
}
