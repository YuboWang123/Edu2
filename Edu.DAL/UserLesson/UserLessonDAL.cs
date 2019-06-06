using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Edu.Entity.UserLesson;
using Wyb.DbUtility;
using System.Data;

namespace Edu.DAL.UserLesson
{
    public class UserLessonDAL : BaseDAL
    {
        private readonly DbFunc _dbFun;

        public UserLessonDAL()
        {
            _sb = new StringBuilder();
            _dbFun=new DbFunc();
        }


        public IEnumerable<Entity.UserLesson.UserLesson> GetList(int pgsz,string whr, out int ttl, string orderby, bool isAsc=false, int pg = 1)
        {
            _sb = new StringBuilder();
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = " LastViewDay";
            }
            _sb.AppendFormat(@"SELECT ROW_NUMBER() over (order by {0} {1}) od,UserId, TrainBaseLessonId, VcrId, TimeSpanViewed, LastViewDay, Memo FROM UserLessons", orderby,isAsc?"asc":"desc");

            if (!string.IsNullOrEmpty(whr))
            {
                _sb.Append(" where " + whr);
            }
            ttl = GetRecordCount(_sb.ToString());

            StringBuilder stringBuilder2 = new StringBuilder();

            stringBuilder2.Append("with tmp as(");
            stringBuilder2.Append(_sb);
            stringBuilder2.Append($")select * from tmp where od>{(pg - 1) * pgsz} and od<={ pg * pgsz }");
            _dbFun.ConnectionString = connstr;
            var dt = _dbFun.ExecuteDataTable(stringBuilder2.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                return TableToModel<Entity.UserLesson.UserLesson>.FillModel(dt);
            }
            return null;
        }
    }
}
