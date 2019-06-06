using Edu.Entity.TrainBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.TrainLesson
{
    public abstract class LessonBase:DbBase
    {      
        public abstract string BindingId { get; set; }       
        public abstract bool? IsAlive { get; set; }       
      
    }
}
