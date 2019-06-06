using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Edu.Entity.TrainBase;
using Edu.Entity.TrainLesson;

namespace Edu.UI.Areas.School.Models.TrainLessonViewModels
{
    public class TrainLessonViewModel
    {
        public TrainBaseLesson TrainBaseLesson { get; set; }
        public bool IsEdit { get; set; }
        public string LessonInfo { get; set; } //path info.
    }
}