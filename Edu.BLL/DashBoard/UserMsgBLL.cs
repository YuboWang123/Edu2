using Edu.DAL.DashBoard;
using Edu.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Edu.Entity.Message;

namespace Edu.BLL.DashBoard
{
   public  class UserMsgBLL
    {
        private UserMsgDAL _dal;

        public UserMsgBLL()
        {
            _dal = new UserMsgDAL();
        }
        public List<UserMessage> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            return _dal.Query(whr, orderby, pg, out ttl, pgsz);
        }

        public int count(string whr)
        {
            return _dal.count(whr);
        }

    }
}
