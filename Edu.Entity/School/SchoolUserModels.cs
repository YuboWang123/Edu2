using System;
using System.Collections.Generic;
using System.Text;
using Edu.Entity.Account;

namespace Edu.Entity.School.SchoolUserModels
{
    /// <summary>
    /// users in the educational service.
    /// </summary>
   
    public class SchoolStudent : Aspnetuser
    {
        public DateTime MakeDay
        {
            get
            {
              return DateTime.Now;
            }
        }
    }

    /// <summary>
    /// teacher's info, trainer can add students.
    /// </summary>
    public class SchoolTrainer : Aspnetuser
    {
        public DateTime MakeDay
        {
            get
            {
                return DateTime.Now;
            }
        }
        
        public int[] SubjectId { get; set; } //teaching subjects
        public bool IsEnabled { get; set; }
      
    }

    /// <summary>
    /// principle informations
    /// </summary>
    public class SchoolPrincipal :Aspnetuser
    {
        public DateTime MakeDay { get; set; }
        public bool IsEnabled { get; set; }
    }
}
