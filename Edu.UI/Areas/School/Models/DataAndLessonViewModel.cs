using Edu.Entity.TrainLesson;
using System.Collections.Generic;

namespace Edu.UI.Areas.School.Models
{
    /// <summary>
    /// binded data with its lessons
    /// </summary>
    public class DataAndLessonViewModel
    {
        public string BindingId { get; set; } //FK
        public IEnumerable<TrainBaseLesson> TrainLesson { get; set; }
    }
}
