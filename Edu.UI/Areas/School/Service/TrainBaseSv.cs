using System;
using Edu.BLL.TrainBase;
using Edu.Entity;
using Edu.Entity.TrainBase;
using Edu.UI.Areas.School.Models;
using Edu.UI.Models;
using PagedList;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Edu.UI.Areas.School.Service
{
    public class TrainBaseSv
    {
        private ApplicationDbContext _db;
        private string _userid;

        public TrainBaseSv()
        {

        }
        public TrainBaseSv(string maker)
        {
            _userid = maker;
        }


        public T GetBaseT<T>(string k) where T : ITrainBase, new()
        {
            string id = k;

            ITrainBase m = default(T);
            if (m == null) m = new T();
            using (_db = new ApplicationDbContext())
            {
                if (m.GetType() == typeof(Base_Period))
                {
                    m = _db.Base_Period.Find(id);
                }

                if (m.GetType() == typeof(Base_Subject))
                {
                    m = _db.Base_Subject.Find(id);
                }

                if (m.GetType() == typeof(Base_Grade))
                {
                    m = _db.Base_Grades.Find(id);
                }

                if (m.GetType() == typeof(Base_Genre))
                {
                    m = _db.Base_Genres.Find(id);
                }

                return (T)m;
            }

        }

        public AppConfigs.OperResult Del<T>(string id) where T : ITrainBase, new()
        {
            if (string.IsNullOrEmpty(id))
            {
                return AppConfigs.OperResult.failDueToArgu;
            }
          

            ITrainBase m = default(T);
            if (m == null) m = new T();
            using (_db = new ApplicationDbContext())
            {
                if (m.GetType() == typeof(Base_Period))
                {
                   var mdl = _db.Base_Period.Find(id);
                   if (mdl == null)
                   {
                       return AppConfigs.OperResult.failDueToExist;
                   }
                    if (_db.Base_DataBind.Any(a => a.PeriodId == mdl.Id))
                   {
                       return AppConfigs.OperResult.failDueToFk;
                    }

                    _db.Base_Period.Remove(mdl);
                }

                if (m.GetType() == typeof(Base_Subject))
                {
                    var mdl = _db.Base_Subject.Find(id);
                    if (mdl == null)
                    {
                        return AppConfigs.OperResult.failDueToExist;
                    }
                    if (_db.Base_DataBind.Any(a => a.SubjectId == mdl.Id))
                    {
                        return AppConfigs.OperResult.failDueToFk;
                    }

                    _db.Base_Subject.Remove(mdl);
                }

                if (m.GetType() == typeof(Base_Grade))
                {
                    var mdl = _db.Base_Grades.Find(id);
                    if (mdl == null)
                    {
                        return AppConfigs.OperResult.failDueToExist;
                    }
                    if (_db.Base_DataBind.Any(a => a.GradeId == mdl.Id))
                    {
                        return AppConfigs.OperResult.failDueToFk;
                    }
                    _db.Base_Grades.Remove(mdl);
                }

                if (m.GetType() == typeof(Base_Genre))
                {
                    var mdl = _db.Base_Genres.Find(id);

                    if (mdl == null)
                    {
                        return AppConfigs.OperResult.failDueToExist;
                    }
                    if (_db.Base_DataBind.Any(a => a.GenreId == mdl.Id))
                    {
                        return AppConfigs.OperResult.failDueToFk;
                    }
                    _db.Base_Genres.Remove(mdl);
                }
                return _db.SaveChanges()>0?AppConfigs.OperResult.success:AppConfigs.OperResult.failUnknown;
            
            }


        }

        #region Bind data


        public DataBindViewModel DataBindRaw()
        {
            using(_db=new ApplicationDbContext())
            {
                return new DataBindViewModel {
                     Base_Periods = _db.Base_Period.OrderBy(a => a.OrderCode).ThenByDescending(a => a.OrderCode.HasValue).Where(a => a.IsEnabled == true).ToList(),
                     Base_Genres = _db.Base_Genres.OrderBy(a => a.OrderCode).ThenByDescending(a => a.OrderCode.HasValue).Where(a => a.IsEnabled == true).ToList(),
                     Base_Grades = _db.Base_Grades.OrderBy(a => a.OrderCode).ThenByDescending(a => a.OrderCode.HasValue).Where(a => a.IsEnabled == true).ToList(),
                     Base_Subjects = _db.Base_Subject.OrderBy(a => a.OrderCode).ThenByDescending(a => a.OrderCode.HasValue).Where(a => a.IsEnabled == true).ToList()
                };
            }        
        }

        /// <summary>
        /// saving the model except the replicated.
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        private int SaveBindingModels(List<Base_DataBind> mdl)
        {
           
            using (_db = new ApplicationDbContext())
            {
                _db.Base_DataBind.AddRange(mdl);
                return _db.SaveChanges();
            
            }
       
        }

        public int SaveBindingData(Base_DataBindViewModel dbModel)
        {          
            var bdBindBll=new Base_DataBindBLL();
            List<Base_DataBind> dbList=new List<Base_DataBind>();
            if ( dbModel.GenreId.Count()> 0)
            {
                foreach (string i in dbModel.GenreId)
                {                   
                    var mdl =new Base_DataBind
                     {
                         GenreId = i,
                         GradeId = dbModel.GradeId,
                         PeriodId = dbModel.PeriodId,
                         SchoolId = dbModel.SchoolId,
                         SubjectId = dbModel.SubjectId
                     };

                    //check if data exists
                    string whr = bdBindBll.ArrayToWhere(mdl);
                    bool exist = bdBindBll.GetBindingId("1=1 " + whr) != null;

                    if (!exist) { //not exists
                       dbList.Add(mdl);
                    }                        
                }
                if(dbList.Count>0)
                return  SaveBindingModels(dbList);
            }

            return 0;
        }
     

        /// <summary>
        /// get binded id that will be deleted.
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        private int[] GetBindId(Base_DataBindViewModel mdl)
        {
            List<int> result=new List<int>();
            if (mdl.GenreId.Count == 0)
            {
                return null;
            }

            var bind = new Base_DataBind();
            var bindBLL=new Base_DataBindBLL();
            foreach (var i in mdl.GenreId)
            {
                bind = new Base_DataBind
                {
                    GenreId = i,
                    GradeId = mdl.GradeId,
                    PeriodId = mdl.PeriodId,
                    SchoolId = mdl.SchoolId,
                    SubjectId = mdl.SubjectId
                };

                string whr = bindBLL.ArrayToWhere(bind);

                var id = bindBLL.GetBindingId("1=1 " + whr);

                if (id != null)
                {
                    result.Add(int.Parse(id));
                }
            }

            return result.ToArray();
        }

        public int DelBinding(Base_DataBindViewModel base_DataBindViewModel)
        {
            int[] needDelId = GetBindId(base_DataBindViewModel);
            int f = 0;
            if (needDelId!=null && needDelId.Length > 0)
            {
                LessonBLL lsnBll=new LessonBLL();
              
                for (int i = 0; i < needDelId.Length; i++)
                {
                    //check there is lesson binded.
                    if (!lsnBll.ExistByFk(needDelId[i]))
                    {
                        using (_db = new ApplicationDbContext())
                        {
                            var m = _db.Base_DataBind.Find(needDelId[i]);
                            if (m != null)
                            {
                                _db.Entry(m).State = EntityState.Deleted;
                                f+=_db.SaveChanges();
                            }
                        }
                    }  
                }

            }

            return f;
        }

        #endregion

        public IPagedList<DbBase> OrderList(int type, int pg, bool asc = true)
        {
            using (_db = new ApplicationDbContext())
            {
                if (asc)
                {
                    switch ((TrainbaseRequestType)type)
                    {
                        case TrainbaseRequestType.period:
                            return _db.Base_Period
                                .OrderBy(a => a.OrderCode)
                                .ToPagedList(pg, 10);
                        case TrainbaseRequestType.grade:
                            return _db.Base_Grades.OrderBy(a => a.OrderCode).ToPagedList(pg, 10);
                        case TrainbaseRequestType.subject:
                            return _db.Base_Subject.OrderBy(a => a.OrderCode).ToPagedList(pg, 10);
                        case TrainbaseRequestType.genre:
                            return _db.Base_Genres.OrderBy(a => a.OrderCode).ToPagedList(pg, 10);
                    }
                }
                else
                {
                    switch ((TrainbaseRequestType)type)
                    {
                        case TrainbaseRequestType.period:
                            return _db.Base_Period
                                .OrderByDescending(a => a.OrderCode)
                                .ToPagedList(pg, 10);
                        case TrainbaseRequestType.grade:
                            return _db.Base_Grades.OrderByDescending(a => a.OrderCode).ToPagedList(pg, 10);
                        case TrainbaseRequestType.subject:
                            return _db.Base_Subject.OrderByDescending(a => a.OrderCode).ToPagedList(pg, 10);
                        case TrainbaseRequestType.genre:
                            return _db.Base_Genres.OrderByDescending(a => a.OrderCode).ToPagedList(pg, 10);
                    }
                }
            }
    
            return new List<DbBase>().ToPagedList(1,10);
        }

        #region prd
        public IPagedList<Base_Period> QueryPeriod(int pg,bool asc) {
            using(_db=new ApplicationDbContext())
            {
                if(asc)
                    return _db.Base_Period.OrderBy(a => a.OrderCode).ThenByDescending(a=>a.OrderCode.HasValue).ToPagedList(pg, 10);
                {
                    return _db.Base_Period.OrderBy(a => a.OrderCode.HasValue).OrderByDescending(a => a.OrderCode).ToPagedList(pg, 10);
                }
            }
        }

        public int AddPeriod(Base_Period base_Period)
        {
            using (_db = new ApplicationDbContext()) {
                _db.Base_Period.Add(base_Period);
                return _db.SaveChanges();
            }
        }
        public int DelPeriod(string id)
        {
            using (_db = new ApplicationDbContext()) {
                var d = _db.Base_Period.Find(id);
                if (d != null)
                {
                    _db.Entry(d).State = System.Data.Entity.EntityState.Deleted;
                    return _db.SaveChanges();
                }
                return 0;
            }
        }

        public int EditPeriod(Base_Period base_Period)
        {
            using (_db=new ApplicationDbContext())
            {
                _db.Entry(base_Period).State = System.Data.Entity.EntityState.Modified;
                return _db.SaveChanges();
            }
        }

        #endregion

        #region gerne
        public IPagedList<Base_Genre> QueryGenre(int pg,bool asc)
        {
            using (_db = new ApplicationDbContext())
            {
                if (asc)
                {
                    return _db.Base_Genres.OrderBy(a => a.OrderCode).ThenBy(a => a.MakeDay).ToPagedList(pg, 10);
                }
                return _db.Base_Genres.OrderBy(a => a.OrderCode.HasValue).OrderByDescending(a => a.MakeDay).ToPagedList(pg, 10);
            }
        }

       
        public int AddGenre(Base_Genre genre)
        {
            using (_db = new ApplicationDbContext())
            {
                _db.Base_Genres.Add(genre);
                return _db.SaveChanges();
            }
        }

        public int EditGenre(Base_Genre mdl)
        {
            using(_db=new ApplicationDbContext())
            {
                _db.Entry(mdl).State = System.Data.Entity.EntityState.Modified;
                return _db.SaveChanges();
            }
        }

        public int DelGenre(string k)
        {
            using (_db = new ApplicationDbContext()) {
                var md=_db.Base_Genres.Find(k);
                if (md != null)
                {
                    // _db.Base_Genres.Remove(md);
                    _db.Entry(md).State = System.Data.Entity.EntityState.Deleted;
                    return _db.SaveChanges();
                }
                return 0;
            }
        }

        #endregion

        #region Grade
        public IPagedList<Base_Grade> QueryGrade(int pg,bool asc)
        {
            using (_db = new ApplicationDbContext())
            {
                if (asc)
                {
                    return _db.Base_Grades.OrderBy(a => a.OrderCode).ThenBy(a => a.MakeDay).ToPagedList(pg, 10);
                }
                return _db.Base_Grades.OrderBy(a => a.OrderCode.HasValue).OrderByDescending(a => a.MakeDay).ToPagedList(pg, 10);
            }

        }


        public int AddGrade(Base_Grade base_Grade) {
            using (_db = new ApplicationDbContext()) {
                 _db.Base_Grades.Add(base_Grade);
                return _db.SaveChanges();
            }
        }

        public int EditGrade(Base_Grade mdl)
        {
            using (_db=new ApplicationDbContext())
            {
                _db.Entry(mdl).State = System.Data.Entity.EntityState.Modified;
                return _db.SaveChanges();
            }
        }

        #endregion

        #region subjct
        public IPagedList<Base_Subject> QuerySubject(int pg,bool asc)
        {
            using (_db = new ApplicationDbContext())
            {
                if (asc)
                {
                    return _db.Base_Subject.OrderBy(a => a.OrderCode).ThenBy(a => a.MakeDay)
                        .ToPagedList(pg, 10);
                }
                return _db.Base_Subject.OrderBy(a => a.OrderCode.HasValue).OrderByDescending(a => a.MakeDay)
                    .ToPagedList(pg, 10);
            }
        }


        public int AddSubject(Base_Subject mdl) {
            using (_db = new ApplicationDbContext()) {
                _db.Base_Subject.Add(mdl);
                return _db.SaveChanges();
            }
        }

        public int EditSubject(Base_Subject mdl)
        {
            using(_db=new ApplicationDbContext())
            {
                _db.Entry(mdl).State = System.Data.Entity.EntityState.Modified;
                return _db.SaveChanges() ;
            }
        }

        #endregion

        #region Lesson
        /// <summary>
        /// get top period list for lesson partial view.
        /// </summary>
        /// <param name="schoolid"></param>
        /// <returns></returns>
        public IEnumerable<Base_Period> GetDataBindTop()
        {
            using (_db = new ApplicationDbContext())
            {
                return _db.Base_Period.OrderBy(a=>a.OrderCode).Where(a => a.IsEnabled == true ).ToList();
            }
         
                
        }
        #endregion
    }
}