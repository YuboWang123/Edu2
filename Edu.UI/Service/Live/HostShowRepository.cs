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
            mdl.Id = Guid.NewGuid().ToString("n");
            mdl.MakeDay = DateTime.Now;
            _dbcontext.Entry(mdl).State = EntityState.Added;
            return _dbcontext.SaveChanges();
            //throw new NotImplementedException();
        }

        public int Del(string id)
        {
            throw new NotImplementedException();
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

        public bool IsTeacher(string uid)
        {
            throw new NotImplementedException();
        }

        public LiveHostShow Single(string k)
        {
            return _dbcontext.LiveHostShows.SingleOrDefault(a =>a.UserId==k);
        }

        public int Update(LiveHostShow mdl)
        {
            _dbcontext.Entry(mdl).State = EntityState.Modified;
            return _dbcontext.SaveChanges();
        }

    }
}