using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.TrainLesson
{
    public interface IVcr:IDbData<Vcr>,IFkExist<VcrFile>,IFkExist<VcrTest>
    {      
        IEnumerable<VcrFile> vcrFiles(string vcrId);
        IEnumerable<VcrTest> vcrTests(string vcrId);

      
    }

    public interface IVcrPath
    {
        string VcrPath(string vcrId);
    }
}
