using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.BLL
{
    public interface IBLL<T> where T:new()
    {
        List<T> GetModelList();
    }
}
