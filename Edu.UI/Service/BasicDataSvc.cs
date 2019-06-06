using Edu.BLL.TrainBase;
using Edu.Entity.TrainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Service
{
    /// <summary>
    /// basic subjects data,binding data service. 
    /// </summary>
    public class BasicDataSvc
    {
        private readonly BasePeriodBLL XueduanBLL;
        private readonly Base_DataBindBLL BindingBLL;
       
        public BasicDataSvc()
        {
            XueduanBLL = new BasePeriodBLL();
            BindingBLL = new Base_DataBindBLL("1");
        }


        #region 学段
        public IEnumerable<Base_Period> GetBase_Periods(string whr)
        {
            return XueduanBLL.Query(whr);
        }
        #endregion
   

        #region Binds

        public Dictionary<string,List<KeyValuePair<string,string>> >GetBindings(string last_a, string[] upperids) {

           return BindingBLL.GetDownLevels(last_a, upperids);       
        }
        #endregion
    }
}