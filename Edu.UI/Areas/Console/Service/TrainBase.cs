using Edu.Entity;
using Edu.Entity.TrainBase;
using Edu.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Edu.UI.Areas.Console.Service
{
    /// <summary>
    /// all the basic data of lesson helper.
    /// period,subject,grade,gerne,
    /// </summary>
    public class TrainBase
    {
        ApplicationDbContext _DB;
        public TrainBase()
        {
             
        }
        #region period
        public Base_Period GetSinglePeriod(int id)
        {
            using(_DB=new ApplicationDbContext())
            {
                return _DB.Base_Period.Find(id);
            }           
        }

        public bool UpdatePeriod(Base_Period base_Period)
        {
            using (_DB = new ApplicationDbContext())
            {
                _DB.Base_Period.Add(base_Period);
                return _DB.SaveChanges() > 0;
            }
        }

        public bool DelPeriod(int id)
        {
            using (_DB = new ApplicationDbContext())
            {
                var d= _DB.Base_Period.Find(id);
                if (d != null)
                {
                    _DB.Base_Period.Remove(d);
                }
            }
            throw new NotImplementedException();
        }


        public IEnumerable<Base_Period> GetPagePeriod(string uid)
        {
            using (_DB = new ApplicationDbContext())
            {
                return _DB.Base_Period.Where(a=>a.Maker==uid);
            }      
        }

        #endregion

        #region subject

        public DataTable GetSubjectList(int pg) {
            throw new NotImplementedException();
        }

        public int UpdateSubject() {
            throw new NotImplementedException();
        }

        public bool AddSubject(Base_Subject mdl) {
            throw new NotImplementedException();
        }

        public  Base_Subject GetSingleSubject(int id)
        {
            throw new NotImplementedException();
        }
        public  bool UpdateSubject(Base_Subject mdl)
        {
            throw new NotImplementedException();
        }
        public bool DelSubject(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region version

        #endregion

        #region databind

        #endregion

    }
}