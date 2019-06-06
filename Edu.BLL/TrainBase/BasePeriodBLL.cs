using Edu.DAL.TrainBase;
using Edu.Entity.TrainBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.BLL.TrainBase
{
    public class BasePeriodBLL
    {
        private Base_PeriodDAL _Dal;

        public BasePeriodBLL()
        {
            _Dal = new Base_PeriodDAL();

        }

        public IEnumerable<Base_Period> Query(string whr)
        {
            return _Dal.Query(whr);
        }
            public int Add(Base_Period model)
        {
            return _Dal.Add(model);
        }

        public int Del(string Key)
        {
            return _Dal.Del(Key);
        }

        public Base_Period Single(string Key)
        {
            return _Dal.Single(Key);
        }

        public int Update(Base_Period model)
        {
            return _Dal.Update(model);
        }
    }
}
