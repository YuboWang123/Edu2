using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edu.Entity;

namespace Edu.Pay
{
    public interface IEduPay<T> where T:class
    {
        void Pay(AppConfigs.PayBy payBy, T mdl);
        bool Notify();
    }
}
