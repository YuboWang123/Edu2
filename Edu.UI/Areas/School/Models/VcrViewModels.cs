using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edu.UI.Areas.School.Models.VcrViewModels
{
    /// <summary>
    /// 视频及其测试题和资源
    /// </summary>
    public class Vcr_Resource_TestViewModel
    {
        public KeyValuePair<string,string> VcrData { get; set; }
        public IEnumerable<VcrFile> vcrFiles { get; set; }
        public IEnumerable<VcrTest> vcrTests { get; set; }
    }



    public class VcrListPag
    {
        public IEnumerable<Vcr> Vcrs { get; set; }
        public string Pager { get; set; }
    }

    public class VcrListViewModel
    {
        public IEnumerable<Vcr> Vcrs { get; set; }
        public VcrListPag VcrListPag { get; set; }
        public string  LessonName { get; set; }
        public string LessonId { get; set; }
    }

    public class TestUploadViewModel
    {
      public enum uploadAnalyze
      {
          success,
          error
      }

        public uploadAnalyze AnalyzeResult { get; set; }
        public IEnumerable<VcrTest> Tests { get; set; }

    }

}