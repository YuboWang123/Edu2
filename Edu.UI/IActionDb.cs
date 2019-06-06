using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Edu.UI
{
    public interface IActionDb<T> where T : new()
    {
        ActionResult Add(T mdl);
        ActionResult Del(string key);
        ActionResult Update(T mdl);
        ActionResult Single(string key);
    }
}
