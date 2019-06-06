using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.TrainLesson
{
    public interface IVcrTest:IDbData<VcrTest>, IFkExist<VcrTest>
    {
        List<VcrTest> BulkInsert(string filepath, string vcrid);
        int BulkInsert(string filepath, string vcrid, string uid);
        List<VcrTest> QueryList(string vcrid);
       
    }


    public interface ITestItem
    {
        string Id { get; set; }       
        string AnswerLetter { get; set; }
     
    }



}
