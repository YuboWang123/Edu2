using Edu.DAL.TrainBase;
using Edu.Entity.TrainBase;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Edu.BLL.TrainBase
{
    public class Base_DataBindBLL
    {
       
        private Base_DataBindDAL base_DataBindDAL;

        public string SchoolId { get; private set; }

        public Base_DataBindBLL()
        {
            base_DataBindDAL = new Base_DataBindDAL();
        }

        public Base_DataBindBLL(string school) : this()
        {
            SchoolId = school;
        }

        public  DataTable GetBindingData(string tablename,string whichid,string whr)
        {
            return base_DataBindDAL.GetBindingData(tablename,whichid, whr);          
        }
 

        public List<string> RelatedSubject(string whr, string column)
        {
            var dt= base_DataBindDAL.GetDataTable(whr,column);
            return TableHelper(dt);
        }

        public List<string> RelatedGrade(string whr,string column)
        {
            var dt = base_DataBindDAL.GetDataTable(whr, column);
            return TableHelper(dt);
        }
        public List<string> RelatedGenre(string whr, string column)
        {
            var dt = base_DataBindDAL.GetDataTable(whr, column);
            return TableHelper(dt);
        }

        /// <summary>
        /// transform rows to int list.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<string> TableHelper(DataTable dt)
        {
            List<string> vs = new List<string>();
            foreach(DataRow item in dt.Rows)
            {
                vs.Add(item[0].ToString());
            }
            return vs;
          
        }

        private Base_DataBind ArrayToModel(string[] listId)
        {
            var model = new Base_DataBind();
            if (listId != null && listId.Count() > 0)
            {
                foreach (var item in listId)
                {
                    var idarr = item.Split('_');
                    if (idarr.Length == 2)
                    {

                        switch (idarr[0])
                        {
                            case "xd":
                                model.PeriodId = idarr[1];
                                break;
                            case "xk":
                                model.SubjectId = idarr[1];
                                break;
                            case "nj":
                                model.GradeId = idarr[1];
                                break;
                            case "lb":
                                model.GenreId = idarr[1];
                                break;

                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upperIdList"></param>
        /// <returns></returns>
        public string ArrayToWhere(Base_DataBind model)
        {
            string whr = string.Empty;
            var props = model.GetType().GetProperties();
            foreach (var property in props)
            {
                var prpName = property.Name;
                var prpVal = property.GetValue(model);
                
                if (prpName!="Id" && prpName!= "SchoolId" && prpVal!=null)
                {
                    switch (prpName)
                    {
                        case nameof(model.PeriodId):
                            whr += $" and periodid='{model.PeriodId}' ";
                            break;
                        case nameof(model.GradeId):
                            whr += $" and gradeId='{model.GradeId}' ";
                            break;
                        case nameof(model.SubjectId):
                            whr += $" and subjectId='{model.SubjectId}' ";
                            break;
                        case nameof(model.GenreId):
                            whr += $" and genreid='{model.GenreId}' ";
                            break;
                    }
                }
            }

            return whr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="key"></param>
        /// <returns>{[xk_3ef6eed815f74523a1a03bff0d79a5a0, 语文]},
        /// {[xk_9c33f6b296104a15a5df4c8165b03f44, 数学]}</returns>
        public List<KeyValuePair<string, string>> TableToKeyValuePair(DataTable dt, string key)
        {
            var kv = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                kv.Add(new KeyValuePair<string, string>(key + "_" + dr["Id"].ToString(), dr["TitleOrName"].ToString()));
            }

            return kv;
        }

        public string GetBindingId(string whr)
        {
            return base_DataBindDAL.GetBindingId(whr);
        }



        /// <summary>
        /// get one down-level binded data.
        /// </summary>
        /// <param name="clickedId">last id of 'a' clicked: nj_942ce7a6dab347f8a24b4e3ff6003268</param>
        /// <param name="upperIds">upper a's ids:["abe98d00a0d04854902ac168e010da49"]</param>
        /// <returns></returns>
        public KeyValuePair<string, List<string>> GetDownLevel(string clickedId,string[] upperIds)
        {
            KeyValuePair<string, List<string>> res;
            string whr = "schoolid=1";
            string last_a = clickedId.Split('_')[1];

            if (upperIds == null && clickedId.StartsWith("xd")) //get level grade
            {
                whr += " and periodid='" + last_a + "'";
                res = new KeyValuePair<string, List<string>>("nj", RelatedGrade(whr, "gradeid"));
            }
            else if (upperIds.Length == 1 && clickedId.StartsWith("nj")) //get level subject
            {
                whr += (" and periodid='" + upperIds[0] + "' and gradeid='" + last_a + "' ");
                res = new KeyValuePair<string, List<string>>("xk", RelatedSubject(whr, "subjectid"));
            }
            else if (upperIds.Length == 2 && clickedId.StartsWith("xk")) //get genre
            {
                whr += " and subjectId='" + last_a + "' and periodid='" + upperIds[0] + "' and gradeid='" + upperIds[1] + "'";
                res = new KeyValuePair<string, List<string>>("lb", RelatedGenre(whr, "genreid"));
            }
            return res;
        }




     

        ///
        /// <summary>
        /// get jsn data of the related data in choose panel-Front Func.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="upperIds"></param>
        /// <returns></returns>
        public Dictionary<string, List<KeyValuePair<string, string>>> GetDownLevels(string last_a, string[] upperIds)
        {
            var idlist = new Dictionary<string, List<KeyValuePair<string, string>>>();
            var upperIdList = upperIds == null ? new List<string>() : upperIds.ToList();
            upperIdList.Add(last_a);
            var datas = ArrayToModel(upperIdList.ToArray()); //make a new array .
            string whr = " schoolid=" + SchoolId;

            DataTable dt = new DataTable();

            string key = string.Empty;
            string id = string.Empty;

            #region
            if (last_a.StartsWith("xd")) //error.
            {
                whr += ArrayToWhere(datas);
                key = "nj";
                dt = GetBindingData("Base_Grade", "gradeid", whr);
                idlist.Add(key, TableToKeyValuePair(dt, key));

                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["Id"].ToString();
                    datas.GradeId = id;
                    last_a = "nj_" + id;
                }

                whr = "1=1";
            }

            if (last_a.StartsWith("nj")) //error.
            {
                whr += ArrayToWhere(datas);
                key = "xk";
                dt = GetBindingData("Base_Subject", "subjectid", whr);
                idlist.Add(key, TableToKeyValuePair(dt, key));

                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["Id"].ToString();
                    datas.SubjectId = id;
                    last_a = "xk_" + id;
                }

                whr = "1=1";
            }

            if (last_a.StartsWith("xk")) //error
            {
                whr += ArrayToWhere(datas);
                key = "lb";
                dt = GetBindingData("Base_Genre", "genreid", whr);
                idlist.Add(key, TableToKeyValuePair(dt, key));
            }

            return idlist;



            #endregion



        }



    }
}
