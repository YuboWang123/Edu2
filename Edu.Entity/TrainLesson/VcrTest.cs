using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Script.Serialization;

namespace Edu.Entity.TrainLesson
{    
    public class UserTestCheck
    {
        readonly List<TestItem> _testItems;
        readonly List<TestItem> _userItems;

        public UserTestCheck(List<TestItem> testItems, List<TestItem> useranswers)
        {
            _testItems = testItems;
            _userItems = useranswers;
        }

        Func<List<TestItem>, ITestItem, string> checkOneWithRightId => (t, u) =>
        {
            string id = string.Empty;
            foreach (var item in t)
            {
                if (item.Id == u.Id)
                {
                    if (IsRight(item.AnswerLetter, u.AnswerLetter)) {
                        id = item.Id;
                    };
                }           
            }

            return id;
            
        };

       


        /// <summary>
        ///Get all error id.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<string> GetRightList()
        {
            List<string> rightListItems = new List<string>();
            string Id = string.Empty;
          
            foreach (var item in _userItems)
            {
                Id = checkOneWithRightId(_testItems, item);
                if (!string.IsNullOrEmpty(Id))
                {
                    rightListItems.Add(Id);
                }
            }

            return rightListItems;
        }



  

        private bool IsRight(string AnswerLetter,string UserLetters)
        {
            if (string.IsNullOrWhiteSpace(AnswerLetter))
            {
                return false;
            }

            string a = AnswerLetter.Replace("、", "").ToLower();
            string u = UserLetters.Replace(",", "").ToLower();

            if (a.Length != u.Length)
            {
                return false;
            }
            else
            {
                return a == u;
                //if (!AnswerLetter.Contains(",")&&!AnswerLetter.Contains("、")) //single char
                //{
                   
                //}
                //else
                //{
                //    string[] A = AnswerLetter.Split('、');
                //    string[] B = UserLetters.Split(',');
                //    Array.Sort(A);
                //    Array.Sort(B);
                //    return String.Join("", A) == String.Join("", B);
                //}
            }           
        }  

    }

    public class TestItem : ITestItem
    {
        public string Id { get;set;}
        public string AnswerLetter { get;set;}
       
    }


    /// <summary>
    /// vcr测试题目.
    /// </summary>
    [Serializable]
    public class VcrTest:ITestItem
    {
        [Key]
        public string Id { get; set; }
        public string VcrId { get; set; }
        [Required]
        [Display(Name = "题文内容")]
        public string Qustion { get; set; }
        [Required]
        [Display(Name = "选项")]
        public string Answer { get; set; }
        [Required]
        public string AnswerLetter { get; set; }      
        public string Analyze { get; set; }
        public string Maker { get; set; }
        public DateTime MakeDay { get; set; }
        public DateTime? UpdateDay { get; set; }    
        public bool? IsEnabled { get; set; }      
        public bool? IsCorrect { get; set; } //for checking the answer is right.        
    }
}
