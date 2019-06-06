using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.TrainLesson
{
    public interface IFkExist<T> where T:class
    {
        int FkExists(string k);
    }
}
