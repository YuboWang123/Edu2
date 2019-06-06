using System;
using System.Collections.Generic;
using System.Text;
using Edu.Entity;
using Edu.Entity.Live;

namespace Edu.BLL.Live
{
    /// <summary>
    /// start show with winform.
    /// </summary>
    public class HostBLL : IHost
    {
        public int Add(LiveHostShow mdl)
        {
            mdl.Id = Guid.NewGuid().ToString("n");
            mdl.IsEnabled = true;
            mdl.MakeDay = DateTime.Now;
            throw new NotImplementedException();
        }

        public AppConfigs.OperResult CreateLive(string uid)
        {
            throw new NotImplementedException();
        }

        public int Del(string id)
        {
            throw new NotImplementedException();
        }

        public bool EnterLive(string uid, string pwd)
        {
            throw new NotImplementedException();
        }

        public LiveHostShow Single(string k)
        {
            throw new NotImplementedException();
        }

        public bool StreamSuccess()
        {
            throw new NotImplementedException();
        }

        public int Update(LiveHostShow mdl)
        {
            throw new NotImplementedException();
        }
    }
}
