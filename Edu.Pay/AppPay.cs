using Edu.Entity;
using Edu.Entity.SchoolFinance;
using System;

namespace Edu.Pay
{
    public class AppPay : IEduPay<FINCard>
    {
        public bool Notify()
        {
            throw new NotImplementedException();
        }

        public void Pay(AppConfigs.PayBy payBy, FINCard mdl)
        {
            throw new NotImplementedException();
        }
    }
}
