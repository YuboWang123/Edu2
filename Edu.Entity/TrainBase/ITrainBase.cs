
using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.TrainBase
{
    public interface ITrainBase 
    {
       
        string Maker { get; set; }
        DateTime MakeDay { get; set; }
        string Memo { get; set; } 
        int? OrderCode { get; set; }


    }
}
