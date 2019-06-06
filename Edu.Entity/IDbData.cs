using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edu.Entity
{
    /// <summary>
    /// for db logic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDbData<T> where T: new()
    {
        int Add(T mdl);
        int Del(string id);
        T Single(string k);
        int Update(T mdl);
       

    }


 

    

}
