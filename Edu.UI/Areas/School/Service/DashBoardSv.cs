using System.Collections.Generic;
using Edu.BLL.DashBoard;
using Edu.Entity;
using Edu.Entity.Message;

namespace Edu.UI.Areas.School.Service
{
    public class DashBoardSv 
    {
        private UserMsgBLL _BLL;

        public DashBoardSv()
        {
            _BLL = new UserMsgBLL();
        }
        public List<UserMessage> Query(string whr, string orderby, int pg, out int ttl, int pgsz = 10)
        {
            return _BLL.Query(whr, orderby, pg ,out ttl, pgsz);
        }
    }
}