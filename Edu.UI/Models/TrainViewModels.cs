using Edu.Entity.TrainBase;
using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Models.TrainViewModels
{
    public class LessonVcrViewModel
    {
        public TrainBaseLesson baseLesson { get; set; }
        public IEnumerable<Vcr> vcrs { get; set; }
        public string UserRole { get; set; }
    }

    public class BindingLessonViewModel
    {
        public IEnumerable<TrainBaseLesson> TrainBaseLessons { get; set; }
        public string Pager { get; set; }
    }

    public class VcrPlayViewModel
    {
        public IEnumerable<Vcr> vcrs { get; set; }
        public TrainBaseLesson LessonInfo { get; set; }
        public VcrPlayContent  VcrPlayContent { get; set; }
        public Vcr SelectedVcr { get; set; }
        public string UserRole { get; set; }
    }

    /// <summary>
    /// for playing vcr playing content
    /// </summary>
    public class VcrPlayContent
    {
        public IEnumerable<VcrTest> vcrTests { get; set; }
        public IEnumerable<VcrFile> vcrFiles { get; set; }
       
    }

}