
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Common
{
    public  interface IDbFunc<T> where T:new()
    {
        int Add(T mdl);
        int Del(string k);
        int Update(T mdl);
        T Single(string k);
    }
}
