using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edu.Entity.Live
{
    public interface IPush
    {
        Task<int> GetUser(PushUser user);
        Task<List<LiveAudiance>> GetUserList(string hostid,int pg,int pgsz);
        bool IsTeacher(string uid);
        bool RecordShow { get; set; }
    }



}
