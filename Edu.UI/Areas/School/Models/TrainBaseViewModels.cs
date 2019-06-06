using Edu.Entity.TrainBase;
using Edu.UI.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Edu.Entity;

namespace Edu.UI.Areas.School.Models
{
    public class BaseDataViewModel
    {
        public IPagedList<DbBase> Datas { get; set; }
        public bool IsAsc { get; set; }
        public string  StrColumn { get; set; }
        public TrainbaseRequestType Type { get; set; }
    }

    public enum TrainbaseRequestType
    {
        period=1,
        subject,
        genre,
        grade
    }




}