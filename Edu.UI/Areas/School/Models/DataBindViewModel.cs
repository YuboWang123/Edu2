using Edu.Entity.TrainBase;
using System.Collections.Generic;

namespace Edu.UI.Areas.School.Models
{
    public class DataBindViewModel
    {
        public IEnumerable<Base_Period> Base_Periods { get; set; }
        public IEnumerable<Base_Grade> Base_Grades { get; set; }
        public IEnumerable<Base_Subject> Base_Subjects { get; set; }
        public IEnumerable<Base_Genre> Base_Genres { get; set; }
        
    }


    /// <summary>
    /// 对多个类别的model
    /// </summary>
    public class Base_DataBindViewModel:Base_DataBind
    {               
        public new IList<string> GenreId { get; set; }
    }

}