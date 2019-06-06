using Edu.Entity.TrainBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity
{
    /// <summary>
    /// all entity base
    /// </summary>
    public abstract class DbBase: ITrainBase,ITester
    {
        public DbBase()
        {
        }
        public abstract string Id { get; set; }
        public abstract string TitleOrName { get; set; }
        public abstract DateTime MakeDay { get; set; }
        [ScaffoldColumn(false)]public abstract string Maker { get; set; }
        [MaxLength(500)]public abstract string Memo { get; set;}      
        public abstract bool IsEnabled { get; set ; }
        public abstract int? OrderCode { get ; set ; }
        public abstract bool? IsBasic { get; set; } //if the info can be updated by admin.
    }
}
