using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Edu.Entity;
using Edu.Entity.Live;
using Edu.Entity.TrainBase;
using Edu.UI.Models;
using Wyb.DbUtility;

namespace Edu.UI.Service.Live
{
    public class HostShowRepository:IDbData<LiveHostShow>,IPush
    {
        private readonly ApplicationDbContext _dbcontext;

        public HostShowRepository()
        {
            _dbcontext = new ApplicationDbContext();
        }

        public bool RecordShow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Add(LiveHostShow mdl)
        {
            _dbcontext.Entry(mdl).State = EntityState.Added;
            return _dbcontext.SaveChanges();
         
        }

        public int Del(string id)
        {
            throw new NotImplementedException();
        }

        public string GenNewId()
        {
            int i = _dbcontext.LiveHostShows.Count();
            return i.ToString().PadLeft(4, '0');
        }

        public Task<int> GetUser(PushUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LiveAudiance>> GetUserList(string hostid, int pg=1, int pgsz=10)
        {
            return await Task.Run(
                () => _dbcontext.LiveAudiances.Where(a => a.HostShowId == hostid).Skip((pg - 1) * pgsz).Take(pgsz).ToListAsync());
           // throw new NotImplementedException();
        }


        /// <summary>
        /// get all shows  for the index list.
        /// </summary>
        /// <param name="ttl"></param>
        /// <param name="pg"></param>
        /// <returns></returns>
        public List<LiveHostShow> GetShows(out int ttl,int pg=1)
        {
            var mdl = _dbcontext.LiveHostShows.OrderByDescending(a => a.StartDate).ToList();
            ttl = mdl.Count;
            return mdl;
        }


        public bool IsTeacher(string uid)
        {
            throw new NotImplementedException();
        }

        public LiveHostShow Single(string k)
        {
            return _dbcontext.LiveHostShows.SingleOrDefault(a =>a.UserId==k)??new LiveHostShow();
        }


        public bool Exist(string uid)
        {
            return _dbcontext.LiveHostShows.Any(a => a.UserId == uid);
        }

        public int Update(LiveHostShow mdl)
        {
            _dbcontext.Entry(mdl).State = EntityState.Modified;
            return _dbcontext.SaveChanges();
        }

    }
}