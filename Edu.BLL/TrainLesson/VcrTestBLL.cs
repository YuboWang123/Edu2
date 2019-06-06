using Edu.Common;
using Edu.DAL.TrainLesson;
using Edu.Entity;
using Edu.Entity.TrainLesson;
using System;
using System.Collections.Generic;
using System.Text;
using Say.Office;


namespace Edu.BLL.TrainLesson
{

    /// <summary>
    /// analyze the text file and extract the test items.
    /// </summary>
    public class VcrTestBLL :IVcrTest
    {
        private readonly VcrTestDAL vcrTestDal;

        public VcrTestBLL()
        {
            vcrTestDal = new VcrTestDAL();
        }

        public int Add(VcrTest mdl)
        {
            return vcrTestDal.Add(mdl);
        }

       

        public VcrTest Single(string k)
        {
            return vcrTestDal.Single(k);
        }

        public int Update(VcrTest mdl)
        {
            return vcrTestDal.Update(mdl);
        }
        

        public int BulkInsertTest(List<VcrTest> testsList)
        {
           return vcrTestDal.BulkInsertTest(testsList);        
        }
        public int BulkDel(List<string> idlist)
        {
            return new VcrTestDAL().BulkDel(idlist);
        }


        /// <summary>
        /// analyze text in the doc and Bulk insert Vcr Test.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="vcrid"></param>
        /// <returns></returns>
        public List<VcrTest> BulkInsert(string filepath, string vcrid)
        {
            Result<List<SplitWord>> result = WordHelper.FindInAll(filepath);          
            List<VcrTest> list=new List<VcrTest>();
            if (result.Code == 0)
            {
                try
                {
                    string answer = string.Empty;
                    string option = string.Empty;
                    string ans = string.Empty;
                    VcrTest vcrTestModel;

                    foreach (var item in result.Data)
                    {
                        vcrTestModel = new VcrTest();
                        answer = Utility.DropHTML(item.Answer).Replace("：", ":");

                        if (!string.IsNullOrEmpty(answer))
                        {
                            var dalist = answer.Split(':');
                            if (dalist.Length == 2)
                            {
                                option = string.Join(",", Utility.GetLetters(Convert.ToInt32(dalist[0])).ToArray());
                                ans = dalist[1].Replace("\\", ",");//将英文反斜杠替换成逗号                        
                            }
                            else
                            {
                                option = string.Join(",", Utility.GetLetters());
                                ans = dalist[0].Replace("\\", ",");//将英文反斜杠替换成逗号                              
                            }
                         
                            
                            vcrTestModel.Analyze = item.Analytic;
                            vcrTestModel.Answer =ans ;
                            vcrTestModel.AnswerLetter =option;
                            vcrTestModel.Id = Guid.NewGuid().ToString("n");
                            vcrTestModel.IsCorrect = false;
                            vcrTestModel.IsEnabled = true;                                                        
                            vcrTestModel.Qustion = item.Question;
                            vcrTestModel.VcrId = vcrid;
                            
                           
                        }

                        list.Add(vcrTestModel);
                    }

                    return list;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            return null;
        }

        public int Del(string id)
        {
            return vcrTestDal.Del(id);
        }

        public int BulkInsert(string filepath, string vcrid, string uid)
        {
            var list = BulkInsert(filepath, vcrid);
            if (list.Count > 0)
            {
                list.ForEach(a => a.Maker = uid);
                list.ForEach(a => a.MakeDay = DateTime.Now);
                list.ForEach(a => a.UpdateDay = DateTime.Now);
                return this.BulkInsertTest(list);
            }

            return 0;
        }

        public List<VcrTest> QueryList(string vcrid)
        {
            return vcrTestDal.QueryList(vcrid);
        }



        public IEnumerable<TestItem> QueryAnswer(string vcrid)
        {
            return vcrTestDal.QueryAnswer(vcrid);
        }

            public int FkExists(string vcrId)
        {
            return vcrTestDal.FkExists(vcrId);
        }
    }
}
