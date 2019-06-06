using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.TrainLesson
{
    public interface IVcrFile:IDbData<VcrFile>, IFkExist<VcrFile>
    {
        IEnumerable<VcrFile> QueryList(string vid);     
    }
}
