using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Edu.BLL.TrainLesson;
using Edu.Entity.TrainLesson;

namespace Edu.UI.Service.VcrService
{
    public class VcrTestSvc
    {
        private string _vcrId;
        private string _answers;
        private VcrTestBLL _testBLL;

        public VcrTestSvc()
        {
            _testBLL = new VcrTestBLL();
        }

        public VcrTestSvc(string vcrId,string answers):this()
        {
            _vcrId = vcrId;
            _answers = answers;
        }

        /// <summary>
        /// get user test score.
        /// </summary>
        /// <returns></returns>
        //public int GetScore()
        //{
        //    var dbTest = QueryDbAnswer();
        //    var userTest = GetUserAnswer();
        //    UserTestCheck<TestItem> userTestCheck =
        //        new UserTestCheck<TestItem>(dbTest.ToList(), userTest.ToList());

        //    return userTestCheck.GetScore();
        //}

        ///// <summary>
        ///// get error test id and score.
        ///// </summary>
        ///// <param name="i"></param>
        ///// <returns></returns>
        //public List<string> GetScoreWithErrors(out int i)
        //{           
        //    var dbTest = QueryDbAnswer();
        //    var userTest = GetUserAnswer();
        //    UserTestCheck userTestCheck = new UserTestCheck(dbTest.ToList(),userTest.ToList());
        //    return userTestCheck.GetScore(out i);
        //}

        private IEnumerable<TestItem> QueryDbAnswer()
        {
            if (string.IsNullOrEmpty(_vcrId))
            {
                return _testBLL.QueryAnswer(_vcrId);
            }
            return null;
        }

        /// <summary>
        /// get user answer list.
        /// </summary>
        /// <param name="answrs"></param>
        /// <returns></returns>
        public IEnumerable<TestItem> GetUserAnswer()
        {
            if (!string.IsNullOrWhiteSpace(_answers))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                var testList = (List<TestItem>)javaScriptSerializer.Deserialize<ITestItem>(_answers);
                return testList;
            }
            else
            {
                throw new Exception("no user test answer string provided");
            }           
        }




   

    }
}