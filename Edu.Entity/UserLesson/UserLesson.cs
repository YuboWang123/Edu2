using System;
using System.ComponentModel.DataAnnotations;
using Edu.Entity.TrainLesson;

namespace Edu.Entity.UserLesson
{

    /// <summary>
    /// lesson vcr that user viewed.
    /// </summary>
    public class UserLesson
    {
        public enum Status
        {
            NotStart,
            Paused,
            Fininshed
        }

        [Key]public string UserId { get; set; }
        [Key]public string TrainBaseLessonId { get; set; }
        public TrainBaseLesson TrainBaseLesson { get; set; }
        [Key]public string VcrId { get; set; }
        public Vcr Vcr { get; set; }
        //viewed time span.
        public double  TimeSpanViewed { get; set; }
        public DateTime EventDateTime { get; set; }
        public Status EventStatus { get; set; }
        [MaxLength(500)]public string  Memo { get; set; }

    }
}
