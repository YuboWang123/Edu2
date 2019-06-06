using Edu.BLL.TrainLesson;
using Edu.Entity.TrainLesson;
using System.Collections.Generic;
using System.Linq;

namespace Edu.UI.Areas.School.Service
{
    public class VcrTestSv 
    {
        private VcrTestBLL vcrTestBLL;
        private string _vid;
        private readonly List<TestItem> _userAswers;

        public VcrTestSv()
        {
            vcrTestBLL = new VcrTestBLL();
        }

        public VcrTestSv(string Id,List<TestItem> userAnswers):this()
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new System.Exception("argument is null");
            }
            _vid = Id;
            _userAswers = userAnswers;
        }

        /// <summary>
        /// get right list.
        /// </summary>
        /// <returns></returns>
        public List<string> GetScoreWithRight()
        {
            var ansr =vcrTestBLL.QueryAnswer(_vid) ;
            var lst = new UserTestCheck(ansr.ToList(), _userAswers);
            List<string> rightList = lst.GetRightList();
            return rightList;
        }
    }
}